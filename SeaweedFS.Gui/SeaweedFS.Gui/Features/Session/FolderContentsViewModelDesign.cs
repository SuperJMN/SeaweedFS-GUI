using System;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

public class FolderContentsViewModelDesign : IFolderContentsViewModel
{
    public FolderContentsViewModelDesign()
    {
        ChildrenItems
            .ToObservableChangeSet()
            .Bind(out var children)
            .Subscribe();

        Children = children;
    }

    public string Path { get; set; }
    public ObservableCollection<IEntryViewModelHost> ChildrenItems { get; } = new();
    public ReadOnlyObservableCollection<IEntryViewModelHost> Children { get; }
    public IEntry Entry { get; set; }
    public bool IsSelected { get; set; }
}