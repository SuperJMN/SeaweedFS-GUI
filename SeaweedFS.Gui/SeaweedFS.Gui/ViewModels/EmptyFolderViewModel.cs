using System.Collections.Generic;

namespace SeaweedFS.Gui.ViewModels;

public class EmptyFolderViewModel : IFolderViewModel
{
    public string Path => "";
    public IEnumerable<EntryViewModel> Items => new List<EntryViewModel>();
}