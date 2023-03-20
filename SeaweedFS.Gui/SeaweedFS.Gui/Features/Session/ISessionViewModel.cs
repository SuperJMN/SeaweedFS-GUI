using System;
using ReactiveUI;
using SeaweedFS.Gui.Features.Transfer;

namespace SeaweedFS.Gui.Features.Session;

public interface ISessionViewModel
{
    IObservable<IFolderContentsViewModel> CurrentFolder { get; }
    public IReactiveCommand GoBack { get; }
    public ITransferManager TransferManager { get; }
}