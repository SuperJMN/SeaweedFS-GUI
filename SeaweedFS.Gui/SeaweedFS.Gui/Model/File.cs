using System.IO;
using System.Threading.Tasks;
using SeaweedFS.Gui.SeaweedFS;

namespace SeaweedFS.Gui.Model;

public class File : IFile
{
    private readonly ISeaweedFS seaweed;
    private readonly long size;

    public File(string fullPath, ISeaweedFS seaweed, long size)
    {
        this.seaweed = seaweed;
        this.size = size;
        Path = fullPath;
    }

    public string Path { get; }
    public Task<Stream> GetStream()
    {
        return seaweed.GetFileContent(Path);
    }

    public long Size => size;
}