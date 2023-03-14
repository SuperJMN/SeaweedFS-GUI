using System;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using SeaweedFS.Gui.Features.Main;

namespace SeaweedFS.Gui.Features.Shell;

public class ShellViewModelDesign : IShellViewModel
{
    public ReactiveCommand<Unit, IObservable<MainViewModel>> Connect { get; }
    public string TypedAddress { get; set; }
    public IObservable<bool> IsConnected { get; }
    public IObservable<MainViewModel> Session { get; set; }
    ICommand IShellViewModel.Connect => Connect;
}