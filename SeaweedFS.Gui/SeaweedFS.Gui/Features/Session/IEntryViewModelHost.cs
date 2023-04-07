using System.ComponentModel;

namespace SeaweedFS.Gui.Features.Session;

public interface IEntryViewModelHost : INotifyPropertyChanged
{
    public IEntryViewModel ViewModel { get; set; }

    public bool IsSelected { get; set; }
}