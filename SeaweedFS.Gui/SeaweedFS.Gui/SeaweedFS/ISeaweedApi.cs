using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace SeaweedFS.Gui.SeaweedFS;

[Headers("Accept: application/json")]
public interface ISeaweedApi
{
    [Get("/{directoryPath}?pretty=y")]
    Task<FolderDto> GetContents(string directoryPath);

    [Multipart]
    [Post("/{path}")]
    Task Upload(string path, StreamPart stream, CancellationToken cancellationToken);

    [Multipart]
    [Post("/{directoryPath}/")]
    Task CreateFolder(string directoryPath);

    [Delete("/{filePath}")]
    Task DeleteFile(string filePath);

    [Delete("/{directoryPath}?recursive=true")]
    Task DeleteFolder(string directoryPath);
}