using System;
using System.Windows.Input;

namespace SeaweedFS.Gui.ViewModels;

public class TransferDesign : ITransfer
{
    public IObservable<string> ErrorMessage { get; set; }
    public string Name { get; set;  }
    public ICommand Cancel { get; }
    public IObservable<double> Percent { get; set; }
    public IObservable<TimeSpan> Eta { get; set; }
    public ICommand Start { get; }
    public IObservable<bool> IsTransferring { get; set; }
    public IObservable<bool> IsIndeterminate { get; set; }
    public TransferKey Key { get; }
    public IObservable<string> TransferButtonText { get; set; }
}