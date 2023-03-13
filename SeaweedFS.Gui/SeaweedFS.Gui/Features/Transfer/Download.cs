using System;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ReactiveUI;
using Zafiro.Core.IO;
using Zafiro.UI.Transfers;

namespace SeaweedFS.Gui.Features.Transfer;

public class Download : ITransferViewModel
{
    private readonly ITransfer inner;

    public Download(string name, Func<Task<Stream>> inputFactory, Func<Stream, Task<ProgressNotifyingStream>> outputFactory)
    {
        inner = new StreamTransferUnit(name, inputFactory, outputFactory);
    }

    public string Icon => "/Assets/download.svg";

    public IObservable<string> ErrorMessage => inner.ErrorMessage;

    public string Name => inner.Name;

    public ReactiveCommand<Unit, Unit> Cancel => inner.Cancel;

    public IObservable<double> Progress => inner.Progress;

    public IObservable<TimeSpan> Eta => inner.Eta;

    public ReactiveCommand<Unit, Result> Start => inner.Start;

    public IObservable<bool> IsTransferring => inner.IsTransferring;

    public IObservable<bool> IsIndeterminate => inner.IsIndeterminate;

    public TransferKey Key => inner.Key;

    public IObservable<string> TransferButtonText => inner.TransferButtonText;
}