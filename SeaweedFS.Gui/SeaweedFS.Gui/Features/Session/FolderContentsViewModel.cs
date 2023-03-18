using System;
using System.Collections.ObjectModel;
using DynamicData;
using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

public class FolderContentsViewModel : IFolderContentsViewModel
{
    public FolderContentsViewModel(IFolder folder)
    {
        folder.Children
            .Transform(x => (IEntryViewModelHost)new EntryViewModelHost(GetViewModelModel(x)))
            .Bind(out var children)
            .Subscribe();

        Children = children;
    }

    private static IEntryViewModel GetViewModelModel(IEntry entryModel)
    {
        return entryModel switch
        {
            IFolder fo => new FolderViewModel(fo),
            IFileModel fi => new FileViewModel(fi),
            _ => throw new NotSupportedException()
        };
    }

    public ReadOnlyObservableCollection<IEntryViewModelHost> Children { get; }
}