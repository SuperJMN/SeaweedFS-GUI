using System.Collections.ObjectModel;
using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

public interface INewFolderViewModel
{
    ReadOnlyObservableCollection<IEntryViewModel> Children { get; }
}