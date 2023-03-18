using System;

namespace SeaweedFS.Gui.Model;

public static class EntryMixin
{
    public static string Name(this IEntry self) => self.Path[(self.Path.LastIndexOf("/", StringComparison.InvariantCulture) + 1)..];
}