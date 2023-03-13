using System.Reactive;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Main;

public interface IHistory
{
    ReactiveCommand<Unit, Unit> GoBack { get; }
    IFolderViewModel CurrentFolder { get; set; }
    IFolderViewModel PreviousFolder { get; }
}

class HistoryDesign : IHistory
{
    public ReactiveCommand<Unit, Unit> GoBack { get; }
    public IFolderViewModel CurrentFolder { get; set; }
    public IFolderViewModel PreviousFolder { get; }
}