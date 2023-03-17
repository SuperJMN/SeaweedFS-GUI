using System;
using System.Windows.Input;
using SeaweedFS.Gui.Features.Session;

namespace SeaweedFS.Gui.Features.ShellCopy;

public interface IShellViewModel
{
    public IObservable<ISessionViewModel> Session { get; }
    ICommand Connect { get; }
    public string? TypedAddress { get; set; }
    public IObservable<bool> IsConnected { get; }
}