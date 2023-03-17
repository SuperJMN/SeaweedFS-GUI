using System;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

class NewFolderViewModelDesign : INewFolderViewModel
{
    public NewFolderViewModelDesign()
    {
        ChildrenItems
            .ToObservableChangeSet()
            .Bind(out var children)
            .Subscribe();

        Children = children;
    }

    public ObservableCollection<IEntryViewModel> ChildrenItems { get; } = new();
    public ReadOnlyObservableCollection<IEntryViewModel> Children { get; }
}