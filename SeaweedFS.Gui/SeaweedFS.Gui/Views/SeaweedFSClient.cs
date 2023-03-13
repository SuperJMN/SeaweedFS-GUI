using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;
using SeaweedFS.Gui.SeaweedFS;

namespace SeaweedFS.Gui.Views;

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

    public Task Upload(string path, StreamPart stream)
    {
        return inner.Upload(path, stream);
    }

    public Task CreateFolder(string directoryPath)
    {
        return inner.CreateFolder(directoryPath);
    }

    public Task<Stream> GetFileContent(string filePath)
    {
        return httpClient.GetStreamAsync(filePath);
    }
}