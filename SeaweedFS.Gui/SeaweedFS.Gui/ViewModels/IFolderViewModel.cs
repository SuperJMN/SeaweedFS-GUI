using System.Collections.Generic;
using System.Windows.Input;

namespace SeaweedFS.Gui.ViewModels;

public interface IFolderViewModel : IEntryViewModel
{
    string Path { get; }
    IEnumerable<IEntryViewModel> Items { get; }
    public ICommand Navigate { get; }
}