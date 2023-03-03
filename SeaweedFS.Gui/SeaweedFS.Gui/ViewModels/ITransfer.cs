using System;
using System.Reactive;
using CSharpFunctionalExtensions;
using ExtendedXmlSerializer.Core;
using ReactiveUI;

namespace SeaweedFS.Gui.ViewModels;

public interface ITransfer
{
    IObservable<string> ErrorMessage { get; }
    string Name { get; }
    System.Windows.Input.ICommand Cancel { get; }
    IObservable<double> Percent { get; }
    IObservable<TimeSpan> Eta { get; }
    System.Windows.Input.ICommand Start { get; }
    IObservable<bool> IsTransferring { get; }
    IObservable<bool> IsIndeterminate { get; }
    TransferKey Key { get; }
    IObservable<string> TransferButtonText { get; }
}