using System.Text;

namespace FicharioDigital.Utils;

public static class StringUtils
{
    public static string NormalizeString(string input)
    {
        var normalized = input.ToLower().Trim().Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalized)
        {
            if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString();
    }
}