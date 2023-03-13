using System;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;

namespace SeaweedFS.Gui.Features.Transfer;

public class TransferManagerDesign : ITransferManager
{
    public TransferManagerDesign()
    {
        TransferList
            .ToObservableChangeSet()
            .Bind(out var items)
            .Subscribe();

        Transfers = items;
    }

    public ObservableCollection<ITransferViewModel> TransferList { get; set; } = new();

    public ReadOnlyObservableCollection<ITransferViewModel> Transfers { get; }

    public void Add(ITransferViewModel copier)
    {
    }
}