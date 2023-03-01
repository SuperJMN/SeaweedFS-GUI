using System.Collections.Generic;

namespace SeaweedFS.Gui.SeaweedFS;

public class Folder
{
    public string Path { get; set; }
    public List<Entry>? Entries { get; set; }
    public long Limit { get; set; }
    public string LastFileName { get; set; }
    public bool ShouldDisplayLoadMore { get; set; }
    public bool EmptyFolder { get; set; }
}