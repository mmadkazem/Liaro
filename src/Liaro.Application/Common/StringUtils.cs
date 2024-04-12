using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Liaro.Application.Common;


public class StringUtils
{
    public static string GetUniqueKey(int size)
    {
        byte[] data = new byte[4 * size];
        var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        using (var crypto = RandomNumberGenerator.Create())
        {
            crypto.GetBytes(data);
        }
        StringBuilder result = new(size);
        for (int i = 0; i < size; i++)
        {
            var rnd = BitConverter.ToUInt32(data, i * 4);
            var idx = rnd % chars.Length;

            result.Append(chars[idx]);
        }

        return result.ToString();
    }

    public static string? GetRightMobileNumber(string phone)
    {
        if (phone == null) return null;

        var number = phone.Replace("+", "")
            .Replace(" ", "")
            .Replace("(", "")
            .Replace(")", "")
            .Replace("-", "")
            .Replace("۰", "0")
            .Replace("۱", "1")
            .Replace("۲", "2")
            .Replace("۳", "3")
            .Replace("۴", "4")
            .Replace("۵", "5")
            .Replace("۶", "6")
            .Replace("۷", "7")
            .Replace("۸", "8")
            .Replace("۹", "9")
            .TrimStart('0');

        if (number.Length < 10) return null;

        number = number.Substring(number.Length - 10, 10);
        if (number.Substring(0, 1) != "9") return null;

        try
        {
            Int64.Parse(number);
        }
        catch (Exception)
        {
            return null;
        }
        return "0" + number;
    }

    public static bool IsValidEmail(string s)
    {
        if (string.IsNullOrEmpty(s)) return false;
        return Regex.IsMatch(s, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
    }

    public static bool IsValidPhone(string s)
    {
        if (string.IsNullOrEmpty(s)) return false;
        return !string.IsNullOrEmpty(GetRightMobileNumber(s));
    }

    public static bool LinkMustBeAUri(string link)
    {
        if (string.IsNullOrWhiteSpace(link)) return false;

        Uri? outUri;
        return Uri.TryCreate(link, UriKind.Absolute, out outUri)
                && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps);
    }
}