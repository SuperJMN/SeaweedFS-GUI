using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SeaweedFS.Gui.Features.Session;

public class EntryViewModelHost : ReactiveObject, IEntryViewModelHost
{
    public EntryViewModelHost(FolderContentsViewModel folderContentsViewModel, IEntryViewModel viewModel)
    {
        FolderContentsViewModel = folderContentsViewModel;
        ViewModel = viewModel;
        IsSelectionEnabled = this.WhenAnyValue(x => x.FolderContentsViewModel.IsMultiselectionEnabled);
    }

    public FolderContentsViewModel FolderContentsViewModel { get; }
    public IEntryViewModel ViewModel { get; set; }

    [Reactive] public bool IsSelected { get; set; }

    public IObservable<bool> IsSelectionEnabled { get; }
}