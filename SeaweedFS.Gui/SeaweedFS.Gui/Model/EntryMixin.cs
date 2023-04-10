using System;
using MoreLinq;
using Zafiro.Core.Mixins;

namespace SeaweedFS.Gui.Model;

public static class EntryMixin
{
    public static string Name(this IEntry self)
    {
        if (self.Path.EndsWith("/"))
        {
            return GetName(self.Path.SkipLast(1).AsString());
        }

        return GetName(self.Path);
    }

    private static string GetName(string path)
    {
        return path[(path.LastIndexOf("/", StringComparison.InvariantCulture) + 1)..];
    }
}