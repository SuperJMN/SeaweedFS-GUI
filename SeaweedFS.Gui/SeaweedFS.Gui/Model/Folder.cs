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
        Path = PathUtils.Normalize(folderDto.Path);

        sourceCache = new SourceCache<IEntry, string>(entry => entry.Name);
        var initial = GetEntries(folderDto).ToObservable();
        sourceCache.PopulateFrom(initial);

        Children = sourceCache
            .Connect()
            .Sort(SortExpressionComparer<IEntry>.Descending(p => p is IFolder)
                .ThenByAscending(p => p.Name));
    }

    public Task<Result> DeleteFile(string name)
    {
        return Result
            .Try(() => seaweed.DeleteFile(PathUtils.Combine(Path, name)))
            .Tap(() => sourceCache.Remove(name));
    }

    public string Path { get; }
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
                return (IEntry) seaweedFile;
            });

        return result;
    }

    public Task<Result> CreateFolder(string name)
    {
        return Result
            .Try(() => seaweed.CreateFolder(PathUtils.Combine(Path, name)))
            .Tap(() => sourceCache.AddOrUpdate(new Folder(new FolderDto { Path = PathUtils.Combine(Path, name) }, seaweed)));
    }

    public Task<Result> DeleteFolder(string name)
    {
        return Result
            .Try(() => seaweed.DeleteFolder(PathUtils.Combine(Path, name)))
            .Tap(() => sourceCache.Remove(name));
    }

    public static Task<Result<IFolder>> Create(string path, ISeaweedFS seaweedFs)
    {
        return Result
            .Try(() => seaweedFs.GetContents(path))
            .Map(folder => (IFolder) new Folder(folder, seaweedFs));
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
            return new Folder(new FolderDto { Path = PathUtils.Normalize(entryDto.FullPath) }, seaweed);
        }

        return new File(PathUtils.Normalize(entryDto.FullPath), seaweed, entryDto.FileSize);
    }
}