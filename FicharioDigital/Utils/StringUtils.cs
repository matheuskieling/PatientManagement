using System.Text;

namespace FicharioDigital.Utils;

public static class StringUtils
{
    public static string NormalizeString(string input)
    {
        return input.ToLower().Trim().Normalize(NormalizationForm.FormD);
    }
}