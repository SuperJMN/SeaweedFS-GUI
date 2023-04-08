using System;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Session;

public class EntryViewModelHostDesign : ReactiveObject, IEntryViewModelHost
{
    public IEntryViewModel ViewModel { get; set; }
    public bool IsSelected { get; set; }
    public IObservable<bool> IsSelectionEnabled { get; }
}