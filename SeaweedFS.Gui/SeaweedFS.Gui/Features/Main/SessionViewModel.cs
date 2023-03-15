using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using CSharpFunctionalExtensions;
using MoreLinq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SeaweedFS.Gui.Features.Transfer;
using SeaweedFS.Gui.SeaweedFS;
using Zafiro.Avalonia;
using Zafiro.Core.Mixins;
using Zafiro.FileSystem;
using Zafiro.UI;

namespace SeaweedFS.Gui.Features.Main;

public class SessionViewModel : ViewModelBase, IMainViewModel
{
    private readonly ISeaweedFS seaweed;
    private readonly IStorage storage;

    public SessionViewModel(ISeaweedFS seaweed, IStorage storage, INotificationService notificationService)
    {
        this.seaweed = seaweed;
        this.storage = storage;
        History = new AddressHistory("");

        TransferManager = new TransferManager();
        var refresh = ReactiveCommand.CreateFromObservable(() => OnRefresh(seaweed, History.CurrentFolder));
        Refresh = refresh;

        var contents = this
            .WhenAnyValue(x => x.History.CurrentFolder)
            .SelectMany(f => OnRefresh(seaweed, f))
            .Merge(refresh);

        Contents = contents.WhereSuccess();

        GoBack = History.GoBack;
        var upload = ReactiveCommand.CreateFromObservable(DoUpload);
        upload
            .Do(transfers => transfers.ForEach(t => TransferManager.Add(t)))
            .Subscribe();

        Upload = upload;

        var createFolder = ReactiveCommand.CreateFromObservable(
            () => Observable
                .FromAsync(() => seaweed.CreateFolder(GetFolderName()))
                .Timeout(TimeSpan.FromSeconds(5))
                .Select(_ => Result.Success())
                .Catch((Exception e) => Observable.Return(Result.Failure(e.Message))),
            this.WhenAnyValue(x => x.NewFolderName).SelectNotEmpty());

        createFolder.WhereFailure()
            .Merge(contents.WhereFailure())
            .ObserveOn(RxApp.MainThreadScheduler)
            .Do(notificationService.ShowMessage)
            .Subscribe();

        CreateFolder = createFolder;
    }

    public IReactiveCommand Upload { get; }

    public ITransferManager TransferManager { get; }

    public IAddressHistory History { get; }

    public IReactiveCommand CreateFolder { get; }

    [Reactive] public string? NewFolderName { get; set; }

    public IReactiveCommand GoBack { get; }

    public IObservable<IFolderViewModel> Contents { get; }

    public ReactiveCommand<Unit, Result<IFolderViewModel>> Refresh { get; }

    private IObservable<IEnumerable<ITransferViewModel>> DoUpload()
    {
        return storage
            .PickForOpenMultiple(new FileTypeFilter("All files", "*.*"))
            .Select(list => list.Select(GetTransfer));
    }

    private ITransferViewModel GetTransfer(IStorable s)
    {
        var name = s.Name;
        return new Upload(name, s.OpenRead, (streamPart, ct) => seaweed.Upload(Path.Combine(History.CurrentFolder, name), streamPart, ct), TransferManager.Remove);
    }

    private string GetFolderName()
    {
        if (History.CurrentFolder == "")
        {
            return NewFolderName! + "/";
        }

        if (History.CurrentFolder.EndsWith("/"))
        {
            return History.CurrentFolder + NewFolderName! + "/";
        }

        return History.CurrentFolder + "/" + NewFolderName! + "/";
    }

    private IObservable<Result<IFolderViewModel>> OnRefresh(ISeaweedFS client, string path)
    {
        var fromService = Observable
            .FromAsync(() => client.GetContents(path))
            .Select(folder => Result.Success((IFolderViewModel)new FolderViewModel(folder.Path, GetChildren(folder), this)));

        return Observable.Defer(() => fromService
            .Timeout(TimeSpan.FromSeconds(5))
            .Catch((Exception ex) => Observable.Return(Result.Failure<IFolderViewModel>(ex.Message))));
    }

    private List<IEntryViewModel> GetChildren(Folder folder)
    {
        if (folder.Entries != null)
        {
            return folder.Entries.Select(Factory).OrderBy(x => x is IFolderViewModel ? 0 : 1).ToList();
        }

        return new List<IEntryViewModel>();
    }


    private IEntryViewModel Factory(Entry entry)
    {
        if (entry.Chunks == null)
        {
            return new FolderViewModel(entry.FullPath[1..], new List<IEntryViewModel>(), this);
        }

        return new FileViewModel(entry.FullPath[1..], entry.FileSize, TransferManager, storage, seaweed);
    }
}