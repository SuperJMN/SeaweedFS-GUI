using System.Reactive;
using CSharpFunctionalExtensions;
using ReactiveUI;
using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

public interface IEntryViewModel : IEntry
{
    ReactiveCommand<Unit, Result> Delete { get; }
}