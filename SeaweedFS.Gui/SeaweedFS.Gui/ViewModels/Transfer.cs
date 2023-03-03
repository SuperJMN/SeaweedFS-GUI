using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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
        IsTransferring = copier.Start.IsExecuting;

        TransferButtonText = copier.Start.Any().Select(_ => "Re-download").StartWith("Download");
        IsIndeterminate = copier.Percent
            .CombineLatest(copier.Start.IsExecuting, (percent, isExecuting) => percent == 0 && isExecuting);

        copier.Start.Execute().Subscribe();
    }

    public IObservable<string> TransferButtonText { get; }
    public IObservable<string> ErrorMessage { get; }
    public string Name { get; }
    public ICommand Cancel { get; }
    public IObservable<double> Percent { get; }
    public IObservable<TimeSpan> Eta { get; }
    public ICommand Start { get; }
    public IObservable<bool> IsTransferring { get; }
    public IObservable<bool> IsIndeterminate { get; }
    public TransferKey Key => new TransferKey(Name);
}