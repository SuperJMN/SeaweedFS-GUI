using ReactiveUI;

namespace SeaweedFS.Gui.Features.Session;

internal interface IFileViewModel : IEntryViewModel
{
    public IReactiveCommand Download { get; }
    public IReactiveCommand Delete { get; }
}