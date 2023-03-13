using System.Collections.Generic;
using System.Windows.Input;

namespace SeaweedFS.Gui.Features.Main;

public interface IFolderViewModel : IEntryViewModel
{
    string Path { get; }
    IEnumerable<IEntryViewModel> Items { get; }
    public ICommand Navigate { get; }
}