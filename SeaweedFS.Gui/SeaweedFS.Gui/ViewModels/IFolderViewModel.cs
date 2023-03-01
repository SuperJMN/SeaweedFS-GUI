using System.Collections.Generic;

namespace SeaweedFS.Gui.ViewModels;

public interface IFolderViewModel
{
    string Path { get; }
    IEnumerable<EntryViewModel> Items { get; }
}