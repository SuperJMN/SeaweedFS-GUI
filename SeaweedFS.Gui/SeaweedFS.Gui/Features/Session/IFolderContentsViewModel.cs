using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using CSharpFunctionalExtensions;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Session;

public interface IFolderContentsViewModel
{
    ReadOnlyObservableCollection<IEntryViewModelHost> Children { get; }
    IReactiveCommand Upload { get; }
    IReactiveCommand CreateFolder { get; }
    string? NewFolderName { get; set; }
    ReactiveCommand<Unit, IList<Result>> DeleteSelected { get; }
    bool IsMultiselectionEnabled { get; set; }
}