using System;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using SeaweedFS.Gui.Features.Session;

namespace SeaweedFS.Gui.Features.ShellCopy;

public class ShellViewModelDesign : IShellViewModel
{
    public ReactiveCommand<Unit, IObservable<SessionViewModel>> Connect { get; }
    public string TypedAddress { get; set; }
    public IObservable<bool> IsConnected { get; }
    public IObservable<SessionViewModel> Session { get; set; }
    ICommand IShellViewModel.Connect => Connect;
}