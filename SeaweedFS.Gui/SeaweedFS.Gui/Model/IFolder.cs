using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DynamicData;

namespace SeaweedFS.Gui.Model;

public interface IFolder : IEntry
{
    string Path { get; }
    IObservable<IChangeSet<IEntry, string>> Children { get; }
    Task<Result> DeleteFile(string name);
    Task<Result<IEntry>> Add(string name, Stream contents, CancellationToken cancellationToken);
    Task<Result> CreateFolder(string name);
    Task<Result> DeleteFolder(string name);
}