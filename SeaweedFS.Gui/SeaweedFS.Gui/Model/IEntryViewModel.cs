namespace SeaweedFS.Gui.Model;

public interface IEntryViewModel
{
    public IEntry Entry { get; set; }
    public bool IsSelected { get; set; }
}