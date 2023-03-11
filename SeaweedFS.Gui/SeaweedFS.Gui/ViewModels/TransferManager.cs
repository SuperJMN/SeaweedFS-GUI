using System;
using System.Collections.ObjectModel;
using DynamicData;
using Zafiro.UI.Transfers;

namespace SeaweedFS.Gui.ViewModels;

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

    public void Add(ITransfer copier)
    {
        sourceCache.AddOrUpdate(copier);
    }
}