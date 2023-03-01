using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;

namespace SeaweedFS.Gui.ViewModels;

public class FolderViewModel : EntryViewModel, IFolderViewModel
{
    public FolderViewModel(string path, IEnumerable<EntryViewModel> children, MainViewModel mainViewModel)
    {
        Path = path;
        Navigate = ReactiveCommand.Create(() => mainViewModel.History.CurrentFolder = this);
        Items = children;
    }

    public IEnumerable<EntryViewModel> Items { get; }
    public ReactiveCommand<Unit, IFolderViewModel> Navigate { get; }
    public string Path { get; }
    public string Name => Path;
}