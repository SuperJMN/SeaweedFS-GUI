using System;
using System.Windows.Input;
using SeaweedFS.Gui.Features.Main;

namespace SeaweedFS.Gui.Features.Shell;

public interface IShellViewModel
{
    public IObservable<SessionViewModel> Session { get; }
    ICommand Connect { get; }
    public string? TypedAddress { get; set; }
    public IObservable<bool> IsConnected { get; }
}