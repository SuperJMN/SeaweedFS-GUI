using System.Reactive;
using CSharpFunctionalExtensions;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Session;

public interface IHistory<T>
{
    ReactiveCommand<Unit, Unit> GoBack { get; }
    T CurrentFolder { get; set; }
    Maybe<T> PreviousFolder { get; }
}