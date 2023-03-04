using System;
using System.Collections.Generic;
using System.Reactive;
using CSharpFunctionalExtensions;
using ReactiveUI;

namespace SeaweedFS.Gui.ViewModels;

public interface IMainViewModel
{
    ITransferManager TransferManager { get; }
    IHistory History { get; }
    ReactiveCommand<Unit, Unit> CreateFolder { get; set; }
    string? NewFolderName { get; set; }
    ReactiveCommand<Unit, IList<Result>> Upload { get; set; }
    ReactiveCommand<Unit, Unit> GoBack { get; }
    IObservable<IFolderViewModel> Contents { get; }
    ReactiveCommand<Unit, IFolderViewModel> Refresh { get; }
}