using System;
using System.Collections.ObjectModel;
using DynamicData;
using SeaweedFS.Gui.Features.Transfer;
using SeaweedFS.Gui.Model;
using Zafiro.Avalonia;

namespace SeaweedFS.Gui.Features.Session;

public class FolderContentsViewModel : IFolderContentsViewModel
{
    private readonly ITransferManager transferManager;
    private readonly Action<string> onGo;

    public FolderContentsViewModel(IFolder folder, ITransferManager transferManager, IStorage storage, Action<string> onGo)
    {
        this.transferManager = transferManager;
        this.onGo = onGo;
        folder.Children
            .Transform(x => (IEntryViewModelHost)new EntryViewModelHost(GetViewModelModel(x, storage)))
            .Bind(out var children)
            .Subscribe();

        Children = children;
    }

    private IEntryViewModel GetViewModelModel(IEntry entryModel, IStorage storage)
    {
        return entryModel switch
        {
            IFolder fo => new FolderViewModel(fo, onGo),
            IFile fi => new FileViewModel(fi, storage, transferManager),
            _ => throw new NotSupportedException()
        };
    }

    public ReadOnlyObservableCollection<IEntryViewModelHost> Children { get; }
}