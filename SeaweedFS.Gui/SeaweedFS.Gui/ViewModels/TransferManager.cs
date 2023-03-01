using System;
using System.Collections.ObjectModel;
using DynamicData;

namespace SeaweedFS.Gui.ViewModels;

public class TransferManager
{
    private readonly SourceCache<Transfer, TransferKey> sourceCache = new(x => x.Key);

    public TransferManager()
    {
        sourceCache
            .Connect()
            .Bind(out var transfers)
            .Subscribe();

        Transfers = transfers;
    }

    public ReadOnlyObservableCollection<Transfer> Transfers { get; }

    public void Add(Transfer copier)
    {
        sourceCache.AddOrUpdate(copier);
    }
}