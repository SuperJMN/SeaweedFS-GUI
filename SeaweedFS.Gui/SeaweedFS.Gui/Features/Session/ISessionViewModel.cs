using System;

namespace SeaweedFS.Gui.Features.Session;

public interface ISessionViewModel
{
    IObservable<IFolderViewModel> CurrentFolder { get; }
}