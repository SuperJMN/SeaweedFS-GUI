using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Refit;
using SeaweedFS.Gui.SeaweedFS;
using Zafiro.Avalonia;
using Zafiro.Core.Mixins;
using Zafiro.FileSystem;
using Zafiro.UI;
using Observable = System.Reactive.Linq.Observable;

namespace SeaweedFS.Gui.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IOpenFilePicker filePicker;
    private readonly ISaveFilePicker savePicker;
    private readonly ISeaweed seaweedFs;

    public MainViewModel(ISeaweed seaweedFs, IOpenFilePicker filePicker, ISaveFilePicker savePicker, INotificationService notificationService)
    {
        this.seaweedFs = seaweedFs;
        this.filePicker = filePicker;
        this.savePicker = savePicker;
        History = new History(new EmptyFolderViewModel());

        TransferManager = new TransferManager();
        Refresh = ReactiveCommand.CreateFromObservable(() => OnRefresh(seaweedFs, History.CurrentFolder, savePicker));

        Contents = this
            .WhenAnyValue(x => x.History.CurrentFolder)
            .Merge(Refresh)
            .SelectMany(f => OnRefresh(seaweedFs, f, savePicker));

        GoBack = History.GoBack;
        Upload = ReactiveCommand.CreateFromObservable(PickAndUpload);
        CreateFolder = ReactiveCommand.CreateFromTask(() => seaweedFs.CreateFolder(GetFolderName()), this.WhenAnyValue(x => x.NewFolderName).SelectNotEmpty());
        Notify = ReactiveCommand.Create(() => notificationService.ShowMessage("Yepa, cómo vas?"));
    }

    public TransferManager TransferManager { get; }

    public ReactiveCommand<Unit, Unit> Notify { get; }

    public History History { get; }

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
        return System.Reactive.Linq.Observable.FromAsync(() => DoUpload(file, History.CurrentFolder.Path, seaweedFs));
    }

    private async Task<Result> DoUpload(IStorable zafiroFile, string path, ISeaweed seaweed)
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

    private IObservable<IFolderViewModel> OnRefresh(ISeaweed client, IFolderViewModel folderViewModel, ISaveFilePicker saveFilePicker)
    {
        var fromService = System.Reactive.Linq.Observable
            .FromAsync(() => client.GetContents(folderViewModel.Path))
            .Select(folder => new FolderViewModel(folder.Path, GetChildren(folder, client, saveFilePicker), this));

        var composed = System.Reactive.Linq.Observable.Defer(() => fromService.Catch((Exception _) => System.Reactive.Linq.Observable.Return(History.PreviousFolder)));

        return composed;
    }

    private List<EntryViewModel> GetChildren(Folder folder, ISeaweed seaweed, ISaveFilePicker saveFilePicker)
    {
        if (folder.Entries != null)
        {
            return folder.Entries.Select(Factory).OrderBy(x => x is FolderViewModel ? 0 : 1).ToList();
        }

        return new List<EntryViewModel>();
    }


    private EntryViewModel Factory(Entry entry)
    {
        if (entry.Chunks == null)
        {
            return new FolderViewModel(entry.FullPath[1..], new List<EntryViewModel>(), this);
        }

        return new FileViewModel(entry.FullPath[1..], seaweedFs, savePicker, TransferManager);
    }
}