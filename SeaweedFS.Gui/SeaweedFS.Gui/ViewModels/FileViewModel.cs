using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using SeaweedFS.Gui.SeaweedFS;
using Zafiro.Avalonia;
using Zafiro.Core;
using Zafiro.Core.Mixins;
using Zafiro.FileSystem;
using Zafiro.UI;

namespace SeaweedFS.Gui.ViewModels;

class FileViewModel : EntryViewModel, IFileViewModel
{
    private readonly ITransferManager transferManager;
    private readonly IStorage storage;
    private ISeaweed seaweed;

    public FileViewModel(string path, ITransferManager transferManager, IStorage storage, ISeaweed seaweed)
    {
        this.transferManager = transferManager;
        this.storage = storage;
        this.seaweed = seaweed;
        Path = path;
        
        var download = ReactiveCommand.CreateFromObservable(DownloadMe);
        download
            .Do(Add)
            .Subscribe();

        Download = download;
    }

    private void Add(Transfer transfer)
    {
        transferManager.Add(transfer);
    }
    
    private IObservable<Transfer> DownloadMe()
    {
        var defaultExtension = ((ZafiroPath)Name).Extension();
        return storage
            .PickForSave(Name, defaultExtension, new FileTypeFilter("*." + defaultExtension, "*." +  defaultExtension))
            .Values()
            .Select(GetTransfer);
    }

    private Transfer GetTransfer(IStorable s)
    {
        var name = s.Path.RouteFragments.Last();
        async Task<Stream> OriginFactory()
        {
            var httpResponseMessage = await seaweed.GetFileContent(Path);
            var httpResponseMessageStream = await HttpResponseMessageStream.Create(httpResponseMessage);
            return httpResponseMessageStream;
        }

        var transfer = new Transfer(name, OriginFactory, s.OpenWrite);
        return transfer;
    }

    public ICommand Download { get; }

    public string Path { get; }

    public string Name => System.IO.Path.GetFileName(Path);
}