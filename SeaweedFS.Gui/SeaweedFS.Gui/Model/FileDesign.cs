using System.IO;
using System.Threading.Tasks;

namespace SeaweedFS.Gui.Model;

public class FileDesign : IFile
{
    public string Path { get; set; }
    public Task<Stream> GetStream()
    {
        throw new System.NotImplementedException();
    }

    public long Size { get; }
}