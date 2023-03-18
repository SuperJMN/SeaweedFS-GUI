using System;
using System.Collections.Generic;

namespace SeaweedFS.Gui.SeaweedFS;

public class EntryDto
{
    public string FullPath { get; set; }
    public DateTime Mtime { get; set; }
    public DateTime Crtime { get; set; }
    public long Mode { get; set; }
    public long Uid { get; set; }
    public long Gid { get; set; }
    public string Mime { get; set; }
    public long TtlSec { get; set; }
    public string UserName { get; set; }
    public object GroupNames { get; set; }
    public string SymlinkTarget { get; set; }
    public object Md5 { get; set; }
    public long FileSize { get; set; }
    public long Rdev { get; set; }
    public long Inode { get; set; }
    public object Extended { get; set; }
    public object HardLinkId { get; set; }
    public long HardLinkCounter { get; set; }
    public object Content { get; set; }
    public object Remote { get; set; }
    public long Quota { get; set; }
    public List<ChunkDto>? Chunks { get; set; }
}