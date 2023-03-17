using System;
using System.Collections.ObjectModel;
using DynamicData;
using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

public class FolderViewModel : INewFolderViewModel
{
    public FolderViewModel(IFolderModel folder)
    {
        folder.Children
            .Transform(x => (IEntryViewModel)new EntryViewModel(x))
            .Bind(out var children)
            .Subscribe();

        Children = children;
    }

    public ReadOnlyObservableCollection<IEntryViewModel> Children { get; }
}