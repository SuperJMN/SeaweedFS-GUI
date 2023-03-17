using System.Collections.ObjectModel;
using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

public interface IFolderViewModel
{
    ReadOnlyObservableCollection<IEntryViewModel> Children { get; }
}