using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DynamicData;
using DynamicData.Binding;
using Refit;
using SeaweedFS.Gui.SeaweedFS;

namespace SeaweedFS.Gui.Model;

public class FolderModel : IFolderModel
{
    private readonly ISeaweedFS seaweed;
    private readonly SourceCache<IEntryModel, string> sourceCache;

    public FolderModel(Folder folder, ISeaweedFS seaweed)
    {
        this.seaweed = seaweed;
        Path = folder.Path;

        sourceCache = new SourceCache<IEntryModel, string>(entry => entry.Path);
        var initial = GetEntries(folder).ToObservable();
        sourceCache.PopulateFrom(initial);

        Children = sourceCache
            .Connect()
            .Sort(SortExpressionComparer<IEntryModel>.Descending(p => p is IFolderModel)
                .ThenByAscending(p => p.Name));
    }

    public Task<Result> Delete(IEntryModel entryModel)
    {
        return Result
            .Try(() => seaweed.Delete(entryModel.Path))
            .Tap(() => sourceCache.Remove(entryModel.Path));
    }

    private IEnumerable<IEntryModel> GetEntries(Folder folder)
    {
        if (folder.Entries is null)
        {
            return Enumerable.Empty<IEntryModel>();
        }

        return folder.Entries.Select(GetEntry);
    }

    private IEntryModel GetEntry(Entry entry)
    {
        if (entry.Chunks == null)
        {
            return new FolderModel(new Folder (){ Path = entry.FullPath }, seaweed);
        }

        return new FileModel(entry.FullPath);
    }

    public string Path { get; set; }
    public IObservable<IChangeSet<IEntryModel, string>> Children { get; }

    public Task<Result<IEntryModel>> Add(string name, MemoryStream contents, CancellationToken cancellationToken)
    {
        var fullPath = PathUtils.Combine(Path, name);

        return Result
            .Try(() => seaweed.Upload(fullPath, new StreamPart(contents, "file"), cancellationToken))
            .Map(() =>
            {
                var seaweedFile = new FileModel(fullPath);
                sourceCache.AddOrUpdate(seaweedFile);
                return (IEntryModel)seaweedFile;
            });
    }

    public static Task<Result<IFolderModel>> Create(string path, ISeaweedFS seaweedFs)
    {
        return Result
            .Try(() => seaweedFs.GetContents(path))
            .Map(folder => (IFolderModel)new FolderModel(folder, seaweedFs));
    }
}