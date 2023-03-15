using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SeaweedFS.Gui.Features.Main;
using SeaweedFS.Gui.SeaweedFS;
using Zafiro.Avalonia;

namespace SeaweedFS.Gui.Features.Shell;

public class ShellViewModel : ViewModelBase, IShellViewModel
{
    public ShellViewModel(IStorage storage, TopLevel topLevel)
    {
        TypedAddress = "http://192.168.1.31:8888";
        var notificationService = new NotificationService(new WindowNotificationManager(topLevel) { Position = NotificationPosition.BottomRight });

        Session = this
            .WhenAnyValue(x => x.Address)
            .WhereNotNull()
            .Select(x => Observable
                .Using(
                    () => new HttpClient { BaseAddress = new Uri(x)}, 
                    httpClient => CreateSession(storage, httpClient, notificationService).Concat(Observable.Never<SessionViewModel>())))
            .Switch();

        var startSession = ReactiveCommand.Create(() => Address = TypedAddress, this.WhenAnyValue(x => x.Address, x => x.TypedAddress, (old, @new) => old != @new));
        Connect = startSession;
        IsConnected = startSession.Any().StartWith(false);
    }

    private static IObservable<SessionViewModel> CreateSession(IStorage storage, HttpClient httpClient, INotificationService notificationService)
    {
        var sessionViewModel = new SessionViewModel(new SeaweedFSClient(httpClient), storage, notificationService);
        return Observable.Return(sessionViewModel);
    }

    public ICommand Connect { get; }

    [Reactive]
    public string? Address { get; set; }

    [Reactive]
    public string? TypedAddress { get; set; }

    public IObservable<bool> IsConnected { get; }

    public IObservable<SessionViewModel> Session { get; }
}