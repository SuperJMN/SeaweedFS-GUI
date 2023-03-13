using System;
using System.Windows.Input;

namespace SeaweedFS.Gui.ViewModels;

public class MainViewModelDesign : IMainViewModel
{
    public ITransferManager TransferManager { get; set; }
    public IHistory History { get; set; }
    public ICommand CreateFolder { get; set; }
    public string? NewFolderName { get; set; }
    public ICommand Upload { get; set; }
    public ICommand Upload2 { get; }
    public ICommand GoBack { get; }
    public IObservable<IFolderViewModel> Contents { get; set; }
    public ICommand Refresh { get; }
}