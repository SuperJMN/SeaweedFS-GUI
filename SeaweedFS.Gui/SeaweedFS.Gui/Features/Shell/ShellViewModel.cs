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
        var p = this.WhenAnyValue(x => x.Address)
            .WhereNotNull()
            .Select(x => Observable
            .Using(
                () => new HttpClient { BaseAddress = new Uri(x)}, 
                httpClient => Observable.Return(new MainViewModel(new SeaweedFSClient(httpClient), storage, new NotificationService(new WindowNotificationManager(topLevel)))).Concat(Observable.Never<MainViewModel>())));

        Session = p.Switch();

        var connect = ReactiveCommand.Create(() => Address = TypedAddress, this.WhenAnyValue(x => x.Address, x => x.TypedAddress, (a, b) => a != b));
        Connect = connect;
        IsConnected = connect.Any().StartWith(false);
    }

    public ICommand Connect { get; }

    [Reactive]
    public string? Address { get; set; }

    [Reactive]
    public string? TypedAddress { get; set; }

    public IObservable<bool> IsConnected { get; }

    public IObservable<MainViewModel> Session { get; }
}