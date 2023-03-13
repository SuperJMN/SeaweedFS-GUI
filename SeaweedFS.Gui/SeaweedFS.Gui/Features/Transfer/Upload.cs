using System;
using System.IO;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ReactiveUI;
using Refit;
using SeaweedFS.Gui.SeaweedFS;
using Zafiro.UI.Transfers;

namespace SeaweedFS.Gui.Features.Transfer;

public class Upload : ITransferViewModel
{
    private readonly ITransfer inner;

    public Upload(string name, Func<Task<Stream>> inputFactory, Func<StreamPart, CancellationToken, Task> func)
    {
        inner = new RefitUploadUnit(name, inputFactory, func);
    }

    public string Icon => "/Assets/upload.svg";

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