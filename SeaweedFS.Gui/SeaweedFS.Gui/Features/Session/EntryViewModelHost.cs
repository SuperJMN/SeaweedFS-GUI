using ReactiveUI;

namespace SeaweedFS.Gui.Features.Session;

class EntryViewModelHost : ReactiveObject, IEntryViewModelHost
{
    public EntryViewModelHost(IEntryViewModel viewModel)
    {
        ViewModel = viewModel;
    }

    public IEntryViewModel ViewModel { get; set; }
    public bool IsSelected { get; set; }
}