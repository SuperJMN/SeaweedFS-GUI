using ReactiveUI;

namespace SeaweedFS.Gui.Model;

class EntryViewModel : ReactiveObject, IEntryViewModel
{
    public IEntry Entry { get; set; }
    public bool IsSelected { get; set; }
}