using System;
using System.Reactive;
using System.Reactive.Linq;
using CSharpFunctionalExtensions;
using ReactiveUI;
using SeaweedFS.Gui.Features.Main;
using SeaweedFS.Gui.Model;
using Zafiro.Avalonia;
using Zafiro.Core.Mixins;

namespace SeaweedFS.Gui.Features.Session;

public class SessionViewModel : ISessionViewModel
{
    public SessionViewModel(IRoot root, IStorage storage, INotificationService notificationService)
    {
        this.AddressHistory = new AddressHistory("");

        SetCurrentFolder = ReactiveCommand.CreateFromTask<string, Result<IFolderViewModel>>(async s =>
        {
            var result = await root.Get(s);
            return result.Map(model => (IFolderViewModel)new FolderViewModel(model));
        });

        CurrentFolder = SetCurrentFolder.WhereSuccess();

        this.WhenAnyValue(x => x.AddressHistory.CurrentFolder)
            .InvokeCommand(SetCurrentFolder);
    }

    public ReactiveCommand<string, Result<IFolderViewModel>> SetCurrentFolder { get; }

    public IObservable<IFolderViewModel> CurrentFolder { get; }

    public AddressHistory AddressHistory { get; }
}