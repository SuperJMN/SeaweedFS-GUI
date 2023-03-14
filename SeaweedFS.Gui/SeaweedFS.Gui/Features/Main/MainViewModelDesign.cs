using System;
using System.Windows.Input;
using ReactiveUI;
using SeaweedFS.Gui.Features.Transfer;

namespace SeaweedFS.Gui.Features.Main;

public class MainViewModelDesign : IMainViewModel
{
    public ITransferManager TransferManager { get; set; }
    public IHistory History { get; set; }
    public IReactiveCommand CreateFolder { get; set; }
    public string? NewFolderName { get; set; }
    public IReactiveCommand Upload { get; set; }
    public IReactiveCommand GoBack { get; }
    public IObservable<IFolderViewModel> Contents { get; set; }
    public IReactiveCommand Refresh { get; }
}