namespace SeaweedFS.Gui.Model;

public class File : IFile
{
    public File(string fullPath)
    {
        Path = fullPath;
    }

    public string Path { get; }
}