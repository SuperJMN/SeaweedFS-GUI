using System.Windows.Input;

namespace SeaweedFS.Gui.Features.Main;

internal interface IFileViewModel : IEntryViewModel
{
    public ICommand Download { get; }
}