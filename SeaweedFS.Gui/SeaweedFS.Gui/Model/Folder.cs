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

public class Folder : IFolder
{
    private readonly ISeaweedFS seaweed;
    private readonly SourceCache<IEntry, string> sourceCache;

    public Folder(FolderDto folderDto, ISeaweedFS seaweed)
    {
        this.seaweed = seaweed;
        Path = folderDto.Path;

        sourceCache = new SourceCache<IEntry, string>(entry => entry.Name);
        var initial = GetEntries(folderDto).ToObservable();
        sourceCache.PopulateFrom(initial);

        Children = sourceCache
            .Connect()
            .Sort(SortExpressionComparer<IEntry>.Descending(p => p is IFolder)
                .ThenByAscending(p => p.Name));
    }

    public Task<Result> Delete(string name)
    {
        return Result
            .Try(() => seaweed.Delete(name))
            .Tap(() => sourceCache.Remove(name));
    }

    private IEnumerable<IEntry> GetEntries(FolderDto folderDto)
    {
        if (folderDto.Entries is null)
        {
            return Enumerable.Empty<IEntry>();
        }

        return folderDto.Entries.Select(GetEntry);
    }

    private IEntry GetEntry(EntryDto entryDto)
    {
        if (entryDto.Chunks == null)
        {
            return new Folder(new FolderDto (){ Path = entryDto.FullPath }, seaweed);
        }

        return new File(entryDto.FullPath, seaweed, entryDto.FileSize);
    }

    public string Path { get; set; }
    public IObservable<IChangeSet<IEntry, string>> Children { get; }

    public async Task<Result<IEntry>> Add(string name, Stream contents, CancellationToken cancellationToken)
    {
        var fullPath = PathUtils.Combine(Path, name);

        var result = await Result
            .Try(() => seaweed.Upload(fullPath, new StreamPart(contents, "file"), cancellationToken))
            .Map(() =>
            {
                var seaweedFile = new File(fullPath, seaweed, contents.Length);
                sourceCache.AddOrUpdate(seaweedFile);
                return (IEntry)seaweedFile;
            });

        return result;
    }

    public Task<Result> CreateFolder(string path)
    {
        return Result
            .Try(() => seaweed.CreateFolder(path))
            .Tap(() => sourceCache.AddOrUpdate(new Folder(new FolderDto { Path = path }, seaweed)));
    }

    public static Task<Result<IFolder>> Create(string path, ISeaweedFS seaweedFs)
    {
        return Result
            .Try(() => seaweedFs.GetContents(path))
            .Map(folder => (IFolder)new Folder(folder, seaweedFs));
    }
}