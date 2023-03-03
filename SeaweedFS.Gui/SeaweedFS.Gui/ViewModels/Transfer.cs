using System;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using CSharpFunctionalExtensions;
using ReactiveUI;
using Zafiro.UI;

namespace SeaweedFS.Gui.ViewModels;

public class Transfer : ITransfer
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
        copier.Start.Execute().Subscribe(result => {});
        ErrorMessage.Subscribe(s => { });
        IsTransferring = copier.Start.IsExecuting;
    }

    public IObservable<string> ErrorMessage { get; }

    public string Name { get; }

    public ICommand Cancel { get; }

    public IObservable<double> Percent { get; }

    public IObservable<TimeSpan> Eta { get; }

    public ICommand Start { get; }
    public IObservable<bool> IsTransferring { get; }

    public TransferKey Key => new TransferKey(Name);
}