using System.IO;
using System.Threading.Tasks;

namespace SeaweedFS.Gui.Model;

public interface IFile : IEntry
{
    long Size { get; }
    Task<Stream> GetStream();
}