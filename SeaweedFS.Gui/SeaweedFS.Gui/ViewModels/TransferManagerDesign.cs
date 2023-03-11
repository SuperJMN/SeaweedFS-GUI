using System;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using Zafiro.UI.Transfers;

namespace SeaweedFS.Gui.ViewModels;

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

    public ObservableCollection<ITransfer> TransferList { get; set; } = new();

    public ReadOnlyObservableCollection<ITransfer> Transfers { get; }

    public void Add(ITransfer copier)
    {
    }
}