using System;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ReactiveUI;
using Zafiro.UI;

namespace SeaweedFS.Gui.ViewModels;

public class Transfer
{
    public Transfer(string name, Func<Task<Stream>> originFactory, Func<Task<Stream>> destinationFactory)
    {
        Name = name;
        var copier = new StreamCopier(originFactory, destinationFactory);
        Start = copier.Start;
        Eta = copier.Eta;
        Percent = copier.Percent;
        Cancel = copier.Cancel;
        ErrorMessage = copier.ErrorMessage;
        Start.Execute().Subscribe();
    }

    public IObservable<string> ErrorMessage { get; }

    public string Name { get; }

    public ReactiveCommand<Unit, Unit> Cancel { get; }

    public IObservable<double> Percent { get; }

    public IObservable<TimeSpan> Eta { get; }

    public ReactiveCommand<Unit, Result> Start { get; }

    public TransferKey Key => new TransferKey(Name);
}