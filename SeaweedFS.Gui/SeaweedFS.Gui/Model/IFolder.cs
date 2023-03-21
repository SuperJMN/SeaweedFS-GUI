using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DynamicData;

namespace SeaweedFS.Gui.Model;

public interface IFolder : IEntry
{
    Task<Result> Delete(string name);
    string Path { get; }
    IObservable<IChangeSet<IEntry, string>> Children { get; }
    Task<Result<IEntry>> Add(string name, Stream contents, CancellationToken cancellationToken);
}