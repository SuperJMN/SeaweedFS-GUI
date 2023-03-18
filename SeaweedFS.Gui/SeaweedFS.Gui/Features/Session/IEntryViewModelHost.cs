namespace SeaweedFS.Gui.Features.Session;

public interface IEntryViewModelHost
{
    public IEntryViewModel ViewModel { get; set; }
    public bool IsSelected { get; set; }
}