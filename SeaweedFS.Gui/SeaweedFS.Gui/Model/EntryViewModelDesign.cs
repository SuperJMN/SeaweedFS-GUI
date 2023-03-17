namespace SeaweedFS.Gui.Model;

public class EntryViewModelDesign : IEntryViewModel
{
    public IEntryModel EntryModel { get; set; }
    public bool IsSelected { get; set; }
}