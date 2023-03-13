using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using SeaweedFS.Gui.Views;
using Zafiro.Avalonia;
using Zafiro.Core.IO;
using Zafiro.Core.Mixins;
using Zafiro.FileSystem;
using Zafiro.UI;
using Zafiro.UI.Transfers;

namespace SeaweedFS.Gui.ViewModels;

class FileViewModel : EntryViewModel, IFileViewModel
{
    private readonly ITransferManager transferManager;
    private readonly IStorage storage;
    private ISeaweedFS seaweed;

    public FileViewModel(string path, long size, ITransferManager transferManager, IStorage storage, ISeaweedFS seaweed)
    {
        this.transferManager = transferManager;
        this.storage = storage;
        this.seaweed = seaweed;
        Path = path;
        Size = size;

        var download = ReactiveCommand.CreateFromObservable(DownloadMe);
        download
            .Do(Add)
            .Subscribe();

        Download = download;
    }

    private void Add(ITransfer streamTransfer)
    {
        transferManager.Add(streamTransfer);
    }
    
    private IObservable<ITransfer> DownloadMe()
    {
        var defaultExtension = ((ZafiroPath)Name).Extension();
        return storage
            .PickForSave(Name, defaultExtension, new FileTypeFilter("*." + defaultExtension, "*." +  defaultExtension))
            .Values()
            .Select(GetTransfer);
    }

    private ITransfer GetTransfer(IStorable s)
    {
        var name = s.Path.RouteFragments.Last();
        return new RegularTransferUnit(name, () => seaweed.GetFileContent(Path), async _ => new ProgressNotifyingStream(await s.OpenWrite(), () => Size));
    }

    public ICommand Download { get; }

    public string Path { get; }
    public long Size { get; }

    public string Name => System.IO.Path.GetFileName(Path);
}