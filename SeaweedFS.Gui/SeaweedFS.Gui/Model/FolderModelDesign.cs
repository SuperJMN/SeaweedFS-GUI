using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DynamicData;

namespace SeaweedFS.Gui.Model;

class FolderModelDesign : IFolderModel
{
    public Task<Result> Delete(IEntryModel entryModel)
    {
        throw new NotImplementedException();
    }

    public string Path { get; set; }
    public IObservable<IChangeSet<IEntryModel, string>> Children { get; }
    public Task<Result<IEntryModel>> Add(string name, MemoryStream contents, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}