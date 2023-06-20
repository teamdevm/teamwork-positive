using System.Collections.Generic;

namespace Documently.Models;

class TemplateFiller
{
    public static string Expand (string input, Dictionary<string, string> mapping)
    {
        string result = string.Empty;
        string field = string.Empty;
        int cursor = 0;
        int ending = 0;
        // Search forward for the dollar sign
        cursor = input.IndexOf('$', cursor);
        while (cursor != -1)
        {
            // If it was found, eat up a portion of string before it
            result += input.Substring(ending, cursor - ending);
            // Search for the second dollar sign
            ending = input.IndexOf('$', ++cursor);
            // Extract the field name
            field = input.Substring(cursor, ending - cursor);
            // Substitute a value for this field
            result += mapping[field];
            // Search for the next field
            cursor = input.IndexOf('$', ++ending);
        }
        // Eat up the rest of the string
        result += input.Substring(ending);
        return result;
    }
}