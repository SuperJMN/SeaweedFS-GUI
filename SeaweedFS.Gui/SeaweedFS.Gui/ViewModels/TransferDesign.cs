using System;
using System.Reactive;
using CSharpFunctionalExtensions;
using ReactiveUI;
using Zafiro.UI.Transfers;

namespace SeaweedFS.Gui.ViewModels;

public class TransferDesign : ITransfer
{
    public TransferDesign()
    {
        Cancel = ReactiveCommand.Create(() => { });
        Start = ReactiveCommand.Create(Result.Success);
    }

    public IObservable<string> ErrorMessage { get; set; }
    public string Name { get; set;  }
    public ReactiveCommand<Unit, Unit> Cancel { get; }
    public IObservable<double> Percent { get; set; }
    public IObservable<TimeSpan> Eta { get; set; }
    public ReactiveCommand<Unit, Result> Start { get; }
    public IObservable<bool> IsTransferring { get; set; }
    public IObservable<bool> IsIndeterminate { get; set; }
    public TransferKey Key { get; }
    public IObservable<string> TransferButtonText { get; set; }
}