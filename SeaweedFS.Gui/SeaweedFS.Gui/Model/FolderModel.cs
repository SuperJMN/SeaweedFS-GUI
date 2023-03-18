﻿using System;
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

public class FolderModel : IFolder
{
    private readonly ISeaweedFS seaweed;
    private readonly SourceCache<IEntry, string> sourceCache;

    public FolderModel(FolderDto folder, ISeaweedFS seaweed)
    {
        this.seaweed = seaweed;
        Path = folder.Path;

        sourceCache = new SourceCache<IEntry, string>(entry => entry.Path);
        var initial = GetEntries(folder).ToObservable();
        sourceCache.PopulateFrom(initial);

        Children = sourceCache
            .Connect()
            .Sort(SortExpressionComparer<IEntry>.Descending(p => p is IFolder)
                .ThenByAscending(p => p.Name));
    }

    public Task<Result> Delete(IEntry entryModel)
    {
        return Result
            .Try(() => seaweed.Delete(entryModel.Path))
            .Tap(() => sourceCache.Remove(entryModel.Path));
    }

    private IEnumerable<IEntry> GetEntries(FolderDto folder)
    {
        if (folder.Entries is null)
        {
            return Enumerable.Empty<IEntry>();
        }

        return folder.Entries.Select(GetEntry);
    }

    private IEntry GetEntry(EntryDto entryDto)
    {
        if (entryDto.Chunks == null)
        {
            return new FolderModel(new FolderDto { Path = entryDto.FullPath }, seaweed);
        }

        return new FileModel(entryDto.FullPath);
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
                var seaweedFile = new FileModel(fullPath);
                sourceCache.AddOrUpdate(seaweedFile);
                return (IEntry)seaweedFile;
            });
    }

    public static Task<Result<IFolder>> Create(string path, ISeaweedFS seaweedFs)
    {
        return Result
            .Try(() => seaweedFs.GetContents(path))
            .Map(folder => (IFolder)new FolderModel(folder, seaweedFs));
    }
}