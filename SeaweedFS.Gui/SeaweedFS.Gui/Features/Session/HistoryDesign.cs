using System.Reactive;
using CSharpFunctionalExtensions;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Session;

internal class HistoryDesign : IHistory<string>
{
    public ReactiveCommand<Unit, Unit> GoBack { get; }
    public string CurrentFolder { get; set; }
    public Maybe<string> PreviousFolder { get; }
}