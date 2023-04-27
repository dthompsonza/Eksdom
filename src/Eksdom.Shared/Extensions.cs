using System.Security.Cryptography;

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

    public static byte[] ToBytesHeader(this ushort value)
    {
        return new byte[2] 
        { 
            (byte)(value & 0xFF), 
            (byte)((value >> 8) & 0xFF) 
        };
    }

    public static ushort ToUshort(this byte[] bytesHeader)
    {
        return (ushort)((bytesHeader[0]) | (bytesHeader[1] << 8));
    }
}
