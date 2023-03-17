using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DynamicData;
using Refit;
using SeaweedFS.Gui.SeaweedFS;

namespace SeaweedFS.Gui.Model;

public class SeaweedFolder : IEntry
{
    private readonly ISeaweedFS seaweed;
    private readonly SourceCache<IEntry, string> sourceCache;

    public SeaweedFolder(string path, ISeaweedFS seaweed)
    {
        this.seaweed = seaweed;
        Path = path;

        sourceCache = new SourceCache<IEntry, string>(entry => entry.Path);
        var initial = Observable
            .FromAsync(() => seaweed.GetContents(Path)).SelectMany(GetEntries);

        sourceCache.PopulateFrom(initial);

        Children = sourceCache.Connect();
    }

    public Task<Result> Delete(IEntry entry)
    {
        return Result
            .Try(() => seaweed.Delete(entry.Path))
            .Tap(() => sourceCache.Remove(entry.Path));
    }

    private IEnumerable<IEntry> GetEntries(Folder folder)
    {
        if (folder.Entries is null)
        {
            return Enumerable.Empty<IEntry>();
        }

        return folder.Entries.Select(GetEntry);
    }

    private IEntry GetEntry(Entry entry)
    {
        if (entry.Chunks == null)
        {
            return new SeaweedFolder(entry.FullPath, seaweed);
        }

        return new SeaweedFile(entry.FullPath);
    }

    public string Path { get; set; }
    public IObservable<IChangeSet<IEntry, string>> Children { get; }

    public Task<Result<IEntry>> Add(string name, MemoryStream contents, CancellationToken cancellationToken)
    {
        var fullPath = PathUtils.Combine(Path, name);

        return Result
            .Try(() => seaweed.Upload(fullPath, new StreamPart(contents, "file"), cancellationToken))
            .Map(() =>
            {
                var seaweedFile = new SeaweedFile(fullPath);
                sourceCache.AddOrUpdate(seaweedFile);
                return (IEntry)seaweedFile;
            });
    }
}