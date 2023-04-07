using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SeaweedFS.Gui.Features.Session;

public class EntryViewModelHost : ReactiveObject, IEntryViewModelHost
{
    private bool isSelected = true;

    public EntryViewModelHost(IEntryViewModel viewModel)
    {
        ViewModel = viewModel;
    }

    public IEntryViewModel ViewModel { get; set; }

    [Reactive]
    public bool IsSelected { get; set; }
}