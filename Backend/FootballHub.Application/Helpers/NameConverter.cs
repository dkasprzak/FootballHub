using System.Text.RegularExpressions;

namespace FootballHub.Application.Helpers;

public static class NameConverter
{
    public static string FormatFileNameToSnakeCase(string name)
    {
        name = name.Replace(".", " ").Replace("  ", " ").Replace(" ", "_");
        return name.ToLower();
    } 
}
