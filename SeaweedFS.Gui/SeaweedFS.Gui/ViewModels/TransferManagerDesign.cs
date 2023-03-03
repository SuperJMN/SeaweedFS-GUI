using System;
using System.Collections.ObjectModel;
using System.Reactive;
using CSharpFunctionalExtensions;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace SeaweedFS.Gui.ViewModels;

public class TransferManagerDesign : ITransferManager
{
    public TransferManagerDesign()
    {
        TransferList
            .ToObservableChangeSet()
            .Bind(out var items)
            .Subscribe(set => { });

        Transfers = items;
    }

    public ObservableCollection<ITransfer> TransferList { get; set; } = new();

    public ReadOnlyObservableCollection<ITransfer> Transfers { get; }
}