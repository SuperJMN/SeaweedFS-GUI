using System.Reactive;
using System.Windows.Input;
using CSharpFunctionalExtensions;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Main;

internal interface IFileViewModel : IEntryViewModel
{
    public ICommand Download { get; }
    public ReactiveCommand<Unit, Result> Delete { get; }
}