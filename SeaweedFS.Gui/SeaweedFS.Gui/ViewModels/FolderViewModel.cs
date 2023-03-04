using System.Collections.Generic;
using System.Windows.Input;
using ReactiveUI;

namespace SeaweedFS.Gui.ViewModels;

public class FolderViewModel : EntryViewModel, IFolderViewModel
{
    public FolderViewModel(string path, IEnumerable<IEntryViewModel> children, IMainViewModel mainViewModel)
    {
        Path = path;
        Navigate = ReactiveCommand.Create(() => mainViewModel.History.CurrentFolder = this);
        Items = children;
    }

    public IEnumerable<IEntryViewModel> Items { get; }
    public ICommand Navigate { get; }
    public string Path { get; }
    public string Name => Path;
}