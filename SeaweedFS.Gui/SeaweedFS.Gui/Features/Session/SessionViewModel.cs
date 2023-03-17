using System;
using System.Reactive.Linq;
using ReactiveUI;
using SeaweedFS.Gui.Features.Main;
using SeaweedFS.Gui.Features.ShellCopy;
using Zafiro.Avalonia;
using Zafiro.Core.Mixins;

namespace SeaweedFS.Gui.Features.Session;

public class SessionViewModel : ISessionViewModel
{
    public SessionViewModel(IRoot root, IStorage storage, INotificationService notificationService)
    {
        this.AddressHistory = new AddressHistory("");
        var observable = this.WhenAnyValue(x => x.AddressHistory.CurrentFolder)
            .Select(root.Get)
            .Switch();

        CurrentFolder = observable
            .WhereSuccess()
            .Select(folder => new NewFolderViewModel(folder));
    }

    public IObservable<INewFolderViewModel> CurrentFolder { get; }

    public AddressHistory AddressHistory { get; }
}