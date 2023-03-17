using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DynamicData;

namespace SeaweedFS.Gui.Model;

public interface IFolderModel : IEntryModel
{
    Task<Result> Delete(IEntryModel entryModel);
    string Path { get; set; }
    IObservable<IChangeSet<IEntryModel, string>> Children { get; }
    Task<Result<IEntryModel>> Add(string name, MemoryStream contents, CancellationToken cancellationToken);
}