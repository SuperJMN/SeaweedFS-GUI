using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CSharpFunctionalExtensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Refit;
using SeaweedFS.Gui.SeaweedFS;
using SeaweedFS.Gui.Views;
using Zafiro.Avalonia;
using Zafiro.Core;
using Zafiro.Core.Mixins;
using Zafiro.FileSystem;
using Zafiro.UI;
using Zafiro.UI.Transfers;

namespace SeaweedFS.Gui.ViewModels;

public class MainViewModel : ViewModelBase, IMainViewModel
{
    private readonly IOpenFilePicker filePicker;
    private readonly ISeaweedFS seaweed;
    private readonly IStorage storage;

    public MainViewModel(ISeaweedFS seaweed, IOpenFilePicker filePicker, ISaveFilePicker savePicker, INotificationService notificationService, IStorage storage)
    {
        this.seaweed = seaweed;
        this.filePicker = filePicker;
        this.storage = storage;
        History = new History(new EmptyFolderViewModel());

        TransferManager = new TransferManager();
        Refresh = ReactiveCommand.CreateFromObservable(() => OnRefresh(seaweed, History.CurrentFolder, savePicker));

        Contents = this
            .WhenAnyValue(x => x.History.CurrentFolder)
            .Merge(Refresh)
            .SelectMany(f => OnRefresh(seaweed, f, savePicker));

        GoBack = History.GoBack;
        Upload = ReactiveCommand.CreateFromObservable(PickAndUpload);
        var upload = ReactiveCommand.CreateFromObservable(() => UploadMe());
        upload
            .Do(transfer => TransferManager.Add(transfer))
            .Subscribe();
            
        Upload2 = upload;
        CreateFolder = ReactiveCommand.CreateFromTask(() => seaweed.CreateFolder(GetFolderName()), this.WhenAnyValue(x => x.NewFolderName).SelectNotEmpty());
        Notify = ReactiveCommand.Create(() => notificationService.ShowMessage("Yepa, cómo vas?"));
    }

    public ICommand Upload2 { get; }

    private IObservable<ITransfer> UploadMe()
    {
        return storage
            .PickForOpen(new FileTypeFilter("All files", "*.*"))
            .Values()
            .Select(GetTransfer);
    }

    private ITransfer GetTransfer(IStorable s)
    {
        var name = s.Path.RouteFragments.Last();
        var upload = new SendTransfer(name, s.OpenRead, new Uri(new Uri("http://192.168.1.31:8888"), History.CurrentFolder.Path));
        return upload;
    }
    
    public ITransferManager TransferManager { get; }

    public ReactiveCommand<Unit, Unit> Notify { get; }

    public IHistory History { get; }

    public ReactiveCommand<Unit, Unit> CreateFolder { get; set; }

    [Reactive] public string? NewFolderName { get; set; }

    public ReactiveCommand<Unit, IList<Result>> Upload { get; set; }

    public ReactiveCommand<Unit, Unit> GoBack { get; }

    public IObservable<IFolderViewModel> Contents { get; }

    public ReactiveCommand<Unit, IFolderViewModel> Refresh { get; }

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

    private IObservable<IList<Result>> PickAndUpload()
    {
        return filePicker
            .PickMultiple(new FileTypeFilter("All files", "*"))
            .SelectMany(UploadFiles);
    }

    private IObservable<IList<Result>> UploadFiles(IEnumerable<IStorable> files)
    {
        var observables = files
            .ToObservable()
            .SelectMany(UploadFile);
        return observables.ToList();
    }

    private IObservable<Result> UploadFile(IStorable file)
    {
        return System.Reactive.Linq.Observable.FromAsync(() => DoUpload(file, History.CurrentFolder.Path, seaweed));
    }

    private async Task<Result> DoUpload(IStorable zafiroFile, string path, ISeaweedFS seaweed)
    {
        try
        {
            using (var openRead = await zafiroFile.OpenRead())
            {
                var streamPart = new StreamPart(openRead, zafiroFile.Path.RouteFragments.Last());
                await seaweed.Upload(path, streamPart);
                return Result.Success();
            }
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    private IObservable<IFolderViewModel> OnRefresh(ISeaweedFS client, IFolderViewModel folderViewModel, ISaveFilePicker saveFilePicker)
    {
        var fromService = Observable
            .FromAsync(() => client.GetContents(folderViewModel.Path))
            .Select(folder => new FolderViewModel(folder.Path, GetChildren(folder, client, saveFilePicker), this));

        var composed = Observable.Defer(() => fromService.Catch((Exception _) => Observable.Return(History.PreviousFolder)));

        return composed;
    }

    private List<IEntryViewModel> GetChildren(Folder folder, ISeaweedFS seaweed, ISaveFilePicker saveFilePicker)
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

        return new FileViewModel(entry.FullPath[1..], TransferManager, storage, seaweed);
    }
}