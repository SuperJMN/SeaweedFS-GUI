namespace SeaweedFS.Gui.Model;

public static class PathUtils
{
    public static string Combine(string first, string second)
    {
        if (first == "" || first == "/")
        {
            return second;
        }

        if (first.EndsWith("/"))
        {
            return first + second;
        }

        return first + "/" + second;
    }

    public static string Normalize(string entryDtoFullPath)
    {
        return entryDtoFullPath.StartsWith("/") ? entryDtoFullPath[1..] : entryDtoFullPath;
    }
}