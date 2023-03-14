using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Platform.Storage;
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

public class MainViewModel : ViewModelBase, IMainViewModel
{
    private readonly ISeaweedFS seaweed;
    private readonly IStorage storage;

    public MainViewModel(ISeaweedFS seaweed, IStorage storage, INotificationService notificationService)
    {
        this.seaweed = seaweed;
        this.storage = storage;
        History = new History(new EmptyFolderViewModel());

        TransferManager = new TransferManager();
        var refresh = ReactiveCommand.CreateFromObservable(() => OnRefresh(seaweed, History.CurrentFolder));
        Refresh = refresh;

        Contents = this
            .WhenAnyValue(x => x.History.CurrentFolder)
            .Merge(refresh)
            .SelectMany(f => OnRefresh(seaweed, f));

        GoBack = History.GoBack;
        var upload = ReactiveCommand.CreateFromObservable(DoUpload);
        upload
            .Do(transfer => transfer.ForEach(t => TransferManager.Add(t)))
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
            .Do(notificationService.ShowMessage)
            .Subscribe();

        CreateFolder = createFolder;
    }

    public IReactiveCommand Upload { get; }

    public ITransferManager TransferManager { get; }

    public IHistory History { get; }

    public IReactiveCommand CreateFolder { get; }

    [Reactive] public string? NewFolderName { get; set; }

    public IReactiveCommand GoBack { get; }

    public IObservable<IFolderViewModel> Contents { get; }

    public IReactiveCommand Refresh { get; }

    private IObservable<IEnumerable<ITransferViewModel>> DoUpload()
    {
        return storage
            .PickForOpenMultiple(new FileTypeFilter("All files", "*.*"))
            .Select(list => list.Select(GetTransfer));
    }

    private ITransferViewModel GetTransfer(IStorable s)
    {
        var name = s.Name;
        return new Upload(name, s.OpenRead, (streamPart, ct) => seaweed.Upload(Path.Combine(History.CurrentFolder.Path, name), streamPart, ct), TransferManager.Remove);
    }

    private string GetFolderName()
    {
        if (History.CurrentFolder.Path == "")
        {
            return NewFolderName! + "/";
        }

        if (History.CurrentFolder.Path.EndsWith("/"))
        {
            return History.CurrentFolder.Path + NewFolderName! + "/";
        }

        return History.CurrentFolder.Path + "/" + NewFolderName! + "/";
    }

    private IObservable<IFolderViewModel> OnRefresh(ISeaweedFS client, IFolderViewModel folderViewModel)
    {
        var fromService = Observable
            .FromAsync(() => client.GetContents(folderViewModel.Path))
            .Select(folder => new FolderViewModel(folder.Path, GetChildren(folder), this));

        var composed = Observable.Defer(() => fromService.Catch((Exception _) => Observable.Return(History.PreviousFolder)));

        return composed;
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