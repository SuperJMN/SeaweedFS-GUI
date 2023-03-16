using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace SeaweedFS.Gui.SeaweedFS;

internal class SeaweedFSClient : ISeaweedFS
{
    private readonly HttpClient httpClient;
    private readonly ISeaweedApi inner;

    public SeaweedFSClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        inner = RestService.For<ISeaweedApi>(httpClient);
    }

    public Task<Folder> GetContents(string directoryPath)
    {
        return inner.GetContents(directoryPath);
    }

    public Task Upload(string path, StreamPart stream, CancellationToken cancellationToken)
    {
        return inner.Upload(path, stream, cancellationToken);
    }

    public Task CreateFolder(string directoryPath)
    {
        return inner.CreateFolder(directoryPath);
    }

    public Task<Stream> GetFileContent(string filePath)
    {
        return httpClient.GetStreamAsync(filePath);
    }

    public Task Delete(string filePath)
    {
        return inner.Delete(filePath);
    }
}