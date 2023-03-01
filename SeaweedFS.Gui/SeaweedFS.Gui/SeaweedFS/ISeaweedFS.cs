using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace SeaweedFS.Gui.SeaweedFS;

public interface ISeaweedFS
{
    Task<FolderDto> GetContents(string directoryPath);
    Task Upload(string path, StreamPart stream, CancellationToken cancellationToken);
    Task CreateFolder(string directoryPath);
    Task<Stream> GetFileContent(string filePath);
    Task DeleteFile(string filePath);
    Task DeleteFolder(string directoryPath);
}