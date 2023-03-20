﻿using System;
using System.Linq;
using System.Reactive.Linq;
using CSharpFunctionalExtensions;
using ReactiveUI;
using SeaweedFS.Gui.Features.Transfer;
using SeaweedFS.Gui.Model;
using Zafiro.Avalonia;
using Zafiro.Core.IO;
using Zafiro.Core.Mixins;
using Zafiro.FileSystem;
using Zafiro.UI;

namespace SeaweedFS.Gui.Features.Session;

internal class FileViewModel : IFileViewModel
{
    private readonly IFile file;
    private readonly IStorage storage;
    private readonly ITransferManager transferManager;

    public FileViewModel(IFile file, IStorage storage, ITransferManager transferManager)
    {
        this.file = file;
        this.storage = storage;
        this.transferManager = transferManager;
        Path = file.Path;
        var download = ReactiveCommand.CreateFromObservable(DoDownload);
        download
            .Do(Add)
            .Subscribe();

        Download = download;
    }

    private void Add(ITransferViewModel streamTransfer)
    {
        transferManager.Add(streamTransfer);
    }

    private IObservable<ITransferViewModel> DoDownload()
    {
        var defaultExtension = ((ZafiroPath)this.Name()).Extension();
        return storage
            .PickForSave(this.Name(), defaultExtension, new FileTypeFilter(defaultExtension.Match(ext => "*." + ext, () => "*.*"), defaultExtension.Match(ext => "*." + ext, () => "*.*")))
            .Values()
            .Select(GetTransfer);
    }

    private ITransferViewModel GetTransfer(IStorable s)
    {
        
        var name = s.Path.RouteFragments.Last();
        return new Download(name, () => file.GetStream(), async _ => new ProgressNotifyingStream(await s.OpenWrite(), () => file.Size), transferManager.Remove);
    }

    public string Path { get; }
    public IReactiveCommand Download { get; }
}