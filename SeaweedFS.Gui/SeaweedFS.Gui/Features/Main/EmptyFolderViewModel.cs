using System.Collections.Generic;
using System.Windows.Input;

namespace SeaweedFS.Gui.Features.Main;

public class EmptyFolderViewModel : IFolderViewModel
{
    public string Path => "";
    public IEnumerable<IEntryViewModel> Items => new List<IEntryViewModel>();
    public ICommand Navigate { get; }
    public string Name { get; }
}