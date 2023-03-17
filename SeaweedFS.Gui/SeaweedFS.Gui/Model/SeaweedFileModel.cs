namespace SeaweedFS.Gui.Model;

public class SeaweedFileModel : IFileModel
{
    public SeaweedFileModel(string fullPath)
    {
        Path = fullPath;
    }

    public string Path { get; }
}