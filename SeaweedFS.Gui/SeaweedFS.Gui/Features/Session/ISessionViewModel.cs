using System;

namespace SeaweedFS.Gui.Features.Session;

public interface ISessionViewModel
{
    IObservable<INewFolderViewModel> CurrentFolder { get; }
}