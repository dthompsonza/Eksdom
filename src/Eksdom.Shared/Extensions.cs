using System.Text.RegularExpressions;

namespace Eksdom.Shared;

public static class Extensions
{
    public static bool IsNullOrFalse(this bool? obj)
    {
        if (obj is not null && !obj.Value)
        {
            return true;
        }
        return false;
    }

    public static byte[] ToBytesHeader(this uint value)
    {
        return new byte[4]        
        {
            (byte)(value & 0xFF),
            (byte)((value >> 8) & 0xFF),
            (byte)((value >> 16) & 0xFF),
            (byte)((value >> 24) & 0xFF)
        };
    }

    public static uint FromBytesHeader(this byte[] bytesHeader)
    {
        return ((uint)bytesHeader[0]) |
               ((uint)bytesHeader[1] << 8) |
               ((uint)bytesHeader[2] << 16) |
               ((uint)bytesHeader[3] << 24);
    }

    public static bool ValidateLicenceKey(this string licenceKey, out string? validatedKey)
    {
        var pattern = @"^[a-fA-F0-9]{8}-[a-fA-F0-9]{8}-[a-fA-F0-9]{8}-[a-fA-F0-9]{8}$";
        var testKey = licenceKey.Trim().ToUpperInvariant();
        if (Regex.IsMatch(testKey, pattern))
        {
            validatedKey = testKey;
            return true;
        };

        validatedKey = null;
        return false;
    }
}
