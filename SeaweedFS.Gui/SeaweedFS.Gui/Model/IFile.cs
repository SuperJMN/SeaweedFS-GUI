using System.IO;
using System.Threading.Tasks;

namespace SeaweedFS.Gui.Model;

public interface IFile : IEntry
{
    Task<Stream> GetStream();
    long Size { get; }
}