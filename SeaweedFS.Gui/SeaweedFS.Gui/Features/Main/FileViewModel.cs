using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using CSharpFunctionalExtensions;
using ReactiveUI;
using SeaweedFS.Gui.Features.Transfer;
using SeaweedFS.Gui.SeaweedFS;
using Zafiro.Avalonia;
using Zafiro.Core.IO;
using Zafiro.Core.Mixins;
using Zafiro.FileSystem;
using Zafiro.UI;

namespace SeaweedFS.Gui.Features.Main;

public class FileViewModel : EntryViewModel, IFileViewModel
{
    private readonly ITransferManager transferManager;
    private readonly IStorage storage;
    private readonly ISeaweedFS seaweed;

    public FileViewModel(string path, long size, ITransferManager transferManager, IStorage storage, ISeaweedFS seaweed)
    {
        this.transferManager = transferManager;
        this.storage = storage;
        this.seaweed = seaweed;
        Path = path;
        Size = size;

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
        var defaultExtension = ((ZafiroPath)Name).Extension();
        return storage
            .PickForSave(Name, defaultExtension, new FileTypeFilter(defaultExtension.Match(ext => "*." + ext, () => "*.*"), defaultExtension.Match(ext => "*." + ext, () => "*.*")))
            .Values()
            .Select(GetTransfer);
    }

    private ITransferViewModel GetTransfer(IStorable s)
    {
        var name = s.Path.RouteFragments.Last();
        return new Download(name, () => seaweed.GetFileContent(Path), async _ => new ProgressNotifyingStream(await s.OpenWrite(), () => Size), transferManager.Remove);
    }

    public ICommand Download { get; }

    public string Path { get; }
    public long Size { get; }

    public string Name => System.IO.Path.GetFileName(Path);
}