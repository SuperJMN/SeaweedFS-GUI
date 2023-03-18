namespace SeaweedFS.Gui.Features.Session;

public class EntryViewModelHostDesign : IEntryViewModelHost
{
    public IEntryViewModel ViewModel { get; set; }
    public bool IsSelected { get; set; }
}