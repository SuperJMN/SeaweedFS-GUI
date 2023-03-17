using System;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

public class FolderViewModelDesign : IFolderViewModel
{
    public FolderViewModelDesign()
    {
        ChildrenItems
            .ToObservableChangeSet()
            .Bind(out var children)
            .Subscribe();

        Children = children;
    }

    public string Path { get; set; }
    public ObservableCollection<IEntryViewModel> ChildrenItems { get; } = new();
    public ReadOnlyObservableCollection<IEntryViewModel> Children { get; }
    public IEntryModel EntryModel { get; set; }
    public bool IsSelected { get; set; }
}