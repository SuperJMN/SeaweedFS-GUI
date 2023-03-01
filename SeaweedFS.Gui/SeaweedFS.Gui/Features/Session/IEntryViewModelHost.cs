using System;
using System.ComponentModel;

namespace SeaweedFS.Gui.Features.Session;

public interface IEntryViewModelHost : INotifyPropertyChanged
{
    IEntryViewModel ViewModel { get; set; }
    bool IsSelected { get; set; }
    IObservable<bool> IsSelectionEnabled { get; }
}