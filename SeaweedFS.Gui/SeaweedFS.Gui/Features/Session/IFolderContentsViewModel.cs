using System.Collections.ObjectModel;

namespace SeaweedFS.Gui.Features.Session;

public interface IFolderContentsViewModel
{
    ReadOnlyObservableCollection<IEntryViewModelHost> Children { get; }
}