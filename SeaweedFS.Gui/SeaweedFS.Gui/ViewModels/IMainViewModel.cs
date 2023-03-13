using System;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;

namespace SeaweedFS.Gui.ViewModels;

public interface IMainViewModel
{
    ITransferManager TransferManager { get; }
    IHistory History { get; }
    ICommand CreateFolder { get; }
    string? NewFolderName { get; set; }
    ICommand Upload { get; }
    ICommand GoBack { get; }
    IObservable<IFolderViewModel> Contents { get; }
    ICommand Refresh { get; }
}