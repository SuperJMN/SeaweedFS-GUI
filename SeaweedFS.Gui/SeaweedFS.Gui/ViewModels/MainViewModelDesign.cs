using System;
using System.Collections.Generic;
using System.Reactive;
using CSharpFunctionalExtensions;
using ReactiveUI;

namespace SeaweedFS.Gui.ViewModels;

public class MainViewModelDesign : IMainViewModel
{
    public ITransferManager TransferManager { get; set; }
    public IHistory History { get; set; }
    public ReactiveCommand<Unit, Unit> CreateFolder { get; set; }
    public string? NewFolderName { get; set; }
    public ReactiveCommand<Unit, IList<Result>> Upload { get; set; }
    public ReactiveCommand<Unit, Unit> GoBack { get; }
    public IObservable<IFolderViewModel> Contents { get; set; }
    public ReactiveCommand<Unit, IFolderViewModel> Refresh { get; }
}