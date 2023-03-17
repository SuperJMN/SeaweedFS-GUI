namespace SeaweedFS.Gui.Model;

public class SeaweedFile : IEntry
{
    public SeaweedFile(string fullPath)
    {
        Path = fullPath;
    }

    public string Path { get; }
}