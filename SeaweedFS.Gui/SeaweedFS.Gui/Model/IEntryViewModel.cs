﻿namespace SeaweedFS.Gui.Model;

public interface IEntryViewModel
{
    public IEntryModel EntryModel { get; set; }
    public bool IsSelected { get; set; }
}