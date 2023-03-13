using System;
using System.Collections.ObjectModel;
using DynamicData;
using Zafiro.UI.Transfers;

namespace SeaweedFS.Gui.Features.Transfer;

public class TransferManager : ITransferManager
{
    private readonly SourceCache<ITransferViewModel, TransferKey> sourceCache = new(x => x.Key);

    public TransferManager()
    {
        sourceCache
            .Connect()
            .Bind(out var transfers)
            .Subscribe();

        Transfers = transfers;
    }

    public ReadOnlyObservableCollection<ITransferViewModel> Transfers { get; }

    public void Add(ITransferViewModel copier)
    {
        sourceCache.AddOrUpdate(copier);
    }
}