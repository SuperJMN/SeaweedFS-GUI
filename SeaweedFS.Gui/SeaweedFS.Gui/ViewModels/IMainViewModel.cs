using System;
using System.Collections.Generic;
using System.Reactive;
using System.Windows.Input;
using CSharpFunctionalExtensions;
using ReactiveUI;

namespace SeaweedFS.Gui.ViewModels;

public interface IMainViewModel
{
    ITransferManager TransferManager { get; }
    IHistory History { get; }
    ReactiveCommand<Unit, Unit> CreateFolder { get; }
    string? NewFolderName { get; set; }
    ReactiveCommand<Unit, IList<Result>> Upload { get; }
    ICommand Upload2 { get; }
    ReactiveCommand<Unit, Unit> GoBack { get; }
    IObservable<IFolderViewModel> Contents { get; }
    ReactiveCommand<Unit, IFolderViewModel> Refresh { get; }
}