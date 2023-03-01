using System.Collections.Generic;

namespace SeaweedFS.Gui.SeaweedFS;

public class FolderDto
{
    public string Path { get; set; }
    public List<EntryDto>? Entries { get; set; }
    public long Limit { get; set; }
    public string LastFileName { get; set; }
    public bool ShouldDisplayLoadMore { get; set; }
    public bool EmptyFolder { get; set; }
}