namespace SeaweedFS.Gui.Model;

public static class PathUtils
{
    public static string Combine(string first, string second)
    {
        if (first == "")
        {
            return second;
        }

        if (first.EndsWith("/"))
        {
            return first + second;
        }

        return first + "/" + second;
    }
}