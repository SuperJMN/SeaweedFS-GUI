using System.Collections.ObjectModel;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Session;

public interface IFolderContentsViewModel
{
    ReadOnlyObservableCollection<IEntryViewModelHost> Children { get; }
    IReactiveCommand Upload { get; }
    IReactiveCommand CreateFolder { get; }
    string? NewFolderName { get; set; }
}