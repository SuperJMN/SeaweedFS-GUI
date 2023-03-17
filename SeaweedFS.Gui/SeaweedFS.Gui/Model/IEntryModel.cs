namespace SeaweedFS.Gui.Model;

public interface IEntryModel
{
    public string Path { get; }
    public string Name => this.Name();
}