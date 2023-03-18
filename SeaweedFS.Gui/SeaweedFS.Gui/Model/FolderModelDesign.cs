﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DynamicData;

namespace SeaweedFS.Gui.Model;

class FolderModelDesign : IFolder
{
    public Task<Result> Delete(IEntry entryModel)
    {
        throw new NotImplementedException();
    }

    public string Path { get; set; }
    public IObservable<IChangeSet<IEntry, string>> Children { get; }
    public Task<Result<IEntry>> Add(string name, MemoryStream contents, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}