using System;
using System.Windows.Input;
using SeaweedFS.Gui.Features.Transfer;

namespace SeaweedFS.Gui.Features.Main;

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