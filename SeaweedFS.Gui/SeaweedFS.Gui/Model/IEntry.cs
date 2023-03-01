namespace SeaweedFS.Gui.Model;

public interface IEntry
{
    public string Path { get; }
    public string Name => this.Name();
}