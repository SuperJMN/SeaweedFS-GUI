using ReactiveUI;

namespace SeaweedFS.Gui.Model;

class EntryViewModel : ReactiveObject, IEntryViewModel
{
    public IEntryModel EntryModel { get; set; }
    public bool IsSelected { get; set; }
}