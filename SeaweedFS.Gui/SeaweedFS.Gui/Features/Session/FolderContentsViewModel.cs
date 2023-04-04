using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
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
    private readonly ITransferManager transferManager;
    private readonly IStorage storage;
    private readonly Action<string> onGo;

    public FolderContentsViewModel(IFolder folder, INotificationService notifyNotificationService, ITransferManager transferManager, IStorage storage, Action<string> onGo)
    {
        this.folder = folder;
        this.transferManager = transferManager;
        this.storage = storage;
        this.onGo = onGo;
        folder.Children
            .Transform(x => (IEntryViewModelHost)new EntryViewModelHost(GetViewModelModel(x, storage)))
            .Bind(out var children)
            .Subscribe();

        Children = children;
        
        var upload = ReactiveCommand.CreateFromObservable(DoUpload);
        upload
            .Do(transfers => transfers.ForEach(transferManager.Add))
            .Subscribe();

        Upload = upload;

        var createFolder = ReactiveCommand.CreateFromTask(() => folder.CreateFolder(NewFolderName!), this.WhenAnyValue(x => x.NewFolderName).NullOrWhitespace().Not());
        createFolder.WhereFailure()
            .Do(notifyNotificationService.ShowMessage)
            .Subscribe();

        CreateFolder = createFolder;
    }

    public IReactiveCommand CreateFolder { get; }

    [Reactive]
    public string? NewFolderName { get; set; }

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
    
    public IReactiveCommand Upload { get; }

    private IEntryViewModel GetViewModelModel(IEntry entryModel, IStorage storage)
    {
        return entryModel switch
        {
            IFolder fo => new FolderViewModel(fo, onGo, s => folder.DeleteFolder(s)),
            IFile fi => new FileViewModel(fi, storage, transferManager, file => folder.DeleteFile(file.Name)),
            _ => throw new NotSupportedException()
        };
    }

    public ReadOnlyObservableCollection<IEntryViewModelHost> Children { get; }
}