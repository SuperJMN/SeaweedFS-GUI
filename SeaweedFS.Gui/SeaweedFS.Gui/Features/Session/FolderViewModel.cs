﻿using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

internal class FolderViewModel : IEntryViewModel
{
    public FolderViewModel(IFolder fo)
    {
    }

    public string Path { get; }
}