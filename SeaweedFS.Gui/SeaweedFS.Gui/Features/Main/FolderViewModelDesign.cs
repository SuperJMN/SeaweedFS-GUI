using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Main;

class FolderViewModelDesign : IFolderViewModel
{
    public FolderViewModelDesign()
    {
        ItemsList
            .ToObservableChangeSet()
            .Bind(out var items)
            .Subscribe();

        Items = items;
    }

    public ObservableCollection<IEntryViewModel> ItemsList { get; } = new();

    public string Path { get; set; }
    public IEnumerable<IEntryViewModel> Items { get; }
    public ICommand Navigate => ReactiveCommand.Create(() => { });
    public string Name { get; set; }
}