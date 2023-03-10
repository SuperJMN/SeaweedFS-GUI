using System.Windows.Input;

namespace SeaweedFS.Gui.ViewModels;

internal interface IFileViewModel : IEntryViewModel
{
    public ICommand Download { get; }
}