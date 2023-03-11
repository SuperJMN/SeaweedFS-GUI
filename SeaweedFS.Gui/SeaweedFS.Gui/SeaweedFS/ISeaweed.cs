using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace SeaweedFS.Gui.SeaweedFS;

[Headers("Accept: application/json")]
public interface ISeaweedApi
{
    [Get("/{directoryPath}?pretty=y")]
    Task<Folder> GetContents(string directoryPath);

    [Multipart]
    [Post("/{path}")]
    Task Upload(string path, StreamPart stream);

    [Post("/{directoryPath}")]
    Task CreateFolder(string directoryPath);
}

//class CustomDelegatingHandler : DelegatingHandler
//{
//    public CustomDelegatingHandler(HttpMessageHandler innerHandler = null) : base(innerHandler ?? new HttpClientHandler()) { }

//    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//    {
//        try
//        {
//            var httpResponseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
//            var asStr = await httpResponseMessage.Content.ReadAsStringAsync();
//            return httpResponseMessage;
//        }
//        catch (Exception e)
//        {
//            Console.WriteLine(e);
//            throw;
//        }
//    }
//}
