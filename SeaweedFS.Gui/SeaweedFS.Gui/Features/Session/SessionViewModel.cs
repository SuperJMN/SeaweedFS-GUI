using System;
using System.Reactive.Linq;
using CSharpFunctionalExtensions;
using ReactiveUI;
using SeaweedFS.Gui.Features.Main;
using SeaweedFS.Gui.Features.Transfer;
using SeaweedFS.Gui.Model;
using Zafiro.Avalonia;
using Zafiro.Core.Mixins;

namespace SeaweedFS.Gui.Features.Session;

public class SessionViewModel : ISessionViewModel
{
    public SessionViewModel(IRoot root, IStorage storage, INotificationService notificationService)
    {
        AddressHistory = new AddressHistory("");
        TransferManager = new TransferManager();

        SetCurrentFolder = ReactiveCommand.CreateFromTask<string, Result<IFolderContentsViewModel>>(async s =>
        {
            var result = await root.Get(s);
            var map = result.Map(model => (IFolderContentsViewModel)new FolderContentsViewModel(model, notificationService, TransferManager, storage, path => AddressHistory.CurrentFolder = path));
            return map;
        });

        CurrentFolder = SetCurrentFolder.WhereSuccess();
        SetCurrentFolder.WhereFailure().Do(notificationService.ShowMessage).Subscribe();
        GoBack = AddressHistory.GoBack;

        this.WhenAnyValue(x => x.AddressHistory.CurrentFolder)
            .InvokeCommand(SetCurrentFolder);
    }

    public IReactiveCommand GoBack { get; }
    public ITransferManager TransferManager { get; }

    public ReactiveCommand<string, Result<IFolderContentsViewModel>> SetCurrentFolder { get; }

    public IObservable<IFolderContentsViewModel> CurrentFolder { get; }

    public AddressHistory AddressHistory { get; }
}