using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using CSharpFunctionalExtensions;
using DynamicData;
using MoreLinq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SeaweedFS.Gui.Features.Transfer;
using SeaweedFS.Gui.Model;
using Zafiro.Avalonia;
using Zafiro.Core.Mixins;
using Zafiro.FileSystem;
using Zafiro.UI;

namespace SeaweedFS.Gui.Features.Session;

public class FolderContentsViewModel : ViewModelBase, IFolderContentsViewModel
{
    private readonly IFolder folder;
    private readonly Action<string> onGo;
    private readonly ReadOnlyObservableCollection<IEntryViewModelHost> selectables;
    private readonly IStorage storage;
    private readonly ITransferManager transferManager;

    public FolderContentsViewModel(IFolder folder, INotificationService notifyNotificationService, ITransferManager transferManager, IStorage storage, Action<string> onGo)
    {
        this.folder = folder;
        this.transferManager = transferManager;
        this.storage = storage;
        this.onGo = onGo;

        var itemChanges = folder
            .Children
            .Transform(x => (IEntryViewModelHost) new EntryViewModelHost(this, GetEntryViewModel(x, storage)))
            .Replay()
            .RefCount();

        itemChanges
            .Bind(out selectables)
            .Subscribe();

        var selectedChanges = itemChanges
            .AutoRefresh(x => x.IsSelected)
            .Filter(x => x.IsSelected);

        selectedChanges
            .Bind(out var selection)
            .Subscribe();

        var upload = ReactiveCommand.CreateFromObservable(DoUpload);
        upload
            .Do(transfers => transfers.ForEach(transferManager.Add))
            .Subscribe();

        Upload = upload;

        var createFolder = ReactiveCommand.CreateFromTask(() => folder.CreateFolder(NewFolderName!), this.WhenAnyValue(x => x.NewFolderName).NullOrWhitespace().Not());
        createFolder.WhereFailure()
            .Do(notifyNotificationService.ShowMessage)
            .Subscribe();

        DeleteSelected = ReactiveCommand.CreateFromObservable(() =>
        {
            return selection
                .ToObservable()
                .SelectMany(x => x.ViewModel.Delete.Execute())
                .ToList();
        }, selectedChanges
            .ToCollection()
            .Select(x => x.Any()));

        DeleteSelected
            .Where(list => list.Any(result => result.IsFailure))
            .Any()
            .Do(_ => notifyNotificationService.ShowMessage("Some elements could not be deleted"))
            .Do(_ => IsMultiselectionEnabled = false)
            .Subscribe();

        CreateFolder = createFolder;
    }

    public ReactiveCommand<Unit, IList<Result>> DeleteSelected { get; }

    [Reactive] public bool IsMultiselectionEnabled { get; set; }

    public IReactiveCommand CreateFolder { get; }

    [Reactive] public string? NewFolderName { get; set; }

    public IReactiveCommand Upload { get; }

    public ReadOnlyObservableCollection<IEntryViewModelHost> Children => selectables;

    private IObservable<IEnumerable<ITransferViewModel>> DoUpload()
    {
        return storage
            .PickForOpenMultiple(new FileTypeFilter("All files", "*.*"))
            .Select(list => list.Select(GetTransfer));
    }

    private ITransferViewModel GetTransfer(IStorable s)
    {
        var name = s.Name;
        return new Upload(name, s.OpenRead, async (contentStream, ct) =>
        {
            var result = await folder.Add(name, contentStream, ct);
            return result;
        }, key => transferManager.Remove(key));
    }

    private IEntryViewModel GetEntryViewModel(IEntry entryModel, IStorage storage)
    {
        return entryModel switch
        {
            IFolder fo => new FolderViewModel(fo, onGo, s => folder.DeleteFolder(s)),
            IFile fi => new FileViewModel(fi, storage, transferManager, file => folder.DeleteFile(file.Name)),
            _ => throw new NotSupportedException()
        };
    }
}