using System;
using System.Reactive;
using System.Windows.Input;
using CSharpFunctionalExtensions;
using ReactiveUI;
using Zafiro.UI.Transfers;

namespace SeaweedFS.Gui.Features.Transfer;

public class TransferDesign : ITransferViewModel
{
    public TransferDesign()
    {
        Cancel = ReactiveCommand.Create(() => { });
        Start = ReactiveCommand.Create(Result.Success);
    }

    public IObservable<string> ErrorMessage { get; set; }
    public string Name { get; set; }
    public ReactiveCommand<Unit, Unit> Cancel { get; }
    public IObservable<double> Progress { get; set; }
    public IObservable<TimeSpan> Eta { get; set; }
    public ReactiveCommand<Unit, Result> Start { get; }
    public IObservable<bool> IsTransferring { get; set; }
    public IObservable<bool> IsIndeterminate { get; set; }
    public TransferKey Key { get; }
    public IObservable<string> TransferButtonText { get; set; }
    public string Icon { get; set; }
    public ICommand RemoveCommand { get; }
}