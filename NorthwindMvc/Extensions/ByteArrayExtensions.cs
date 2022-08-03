using System;
using System.Text;

namespace NorthwindMvc.Extensions
{
    public static class ByteArrayExtensions
    {
        public static bool HasHeader(this byte[] source, byte[] header)
        {
            if (source.Length < header.Length)
            {
                return false;
            }

            for (int i = 0; i < header.Length; i++)
            {
                if (source[i] != header[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
