using System;
using System.Reactive;
using CSharpFunctionalExtensions;
using ReactiveUI;
using SeaweedFS.Gui.Features.Transfer;

namespace SeaweedFS.Gui.Features.Main;

public interface IMainViewModel
{
    ITransferManager TransferManager { get; }
    IAddressHistory History { get; }
    IReactiveCommand CreateFolder { get; }
    string? NewFolderName { get; set; }
    IReactiveCommand Upload { get; }
    IReactiveCommand GoBack { get; }
    IObservable<IFolderViewModel> Contents { get; }
    ReactiveCommand<Unit, Result<IFolderViewModel>> Refresh { get; }
}