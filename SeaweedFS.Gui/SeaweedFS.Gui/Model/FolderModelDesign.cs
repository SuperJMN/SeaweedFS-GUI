using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DynamicData;

namespace SeaweedFS.Gui.Model;

class FolderModelDesign : IFolder
{
    public Task<Result> DeleteFile(string path)
    {
        throw new NotImplementedException();
    }

    public string Path { get; set; }
    public IObservable<IChangeSet<IEntry, string>> Children { get; }
    public Task<Result<IEntry>> Add(string name, Stream contents, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result> CreateFolder(string name)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteFolder(string name)
    {
        throw new NotImplementedException();
    }
}