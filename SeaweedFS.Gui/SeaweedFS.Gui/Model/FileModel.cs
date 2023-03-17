namespace SeaweedFS.Gui.Model;

public class FileModel : IFileModel
{
    public FileModel(string fullPath)
    {
        Path = fullPath;
    }

    public string Path { get; }
}