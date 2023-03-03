using System;
using System.Collections.ObjectModel;
using DynamicData;

namespace SeaweedFS.Gui.ViewModels;

public interface ITransferManager
{
    ReadOnlyObservableCollection<ITransfer> Transfers { get; }
}

public class TransferManager : ITransferManager
{
    private readonly SourceCache<ITransfer, TransferKey> sourceCache = new(x => x.Key);

    public TransferManager()
    {
        sourceCache
            .Connect()
            .Bind(out var transfers)
            .Subscribe();

        Transfers = transfers;
    }

    public ReadOnlyObservableCollection<ITransfer> Transfers { get; }

    public void Add(Transfer copier)
    {
        sourceCache.AddOrUpdate(copier);
    }
}