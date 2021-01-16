using System;

namespace MpxFlash
{
    class Tools
    {
        public static void DumpBuffer(byte[] buffer)
        {
            for (int row = 0; row < buffer.Length; row += 16)
            {
                for (int col = 0; col < 16; col++)
                {
                    if (row + col < buffer.Length)
                    {
                        Console.Write("{0:X2}", buffer[row + col]);
                        Console.Write(row + col + 1 < buffer.Length && col < 15 && (col + 1) % 4 == 0 ? '-' : ' ');
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                }

                Console.Write(' ');

                for (int col = 0; col < 16; col++)
                {
                    if (row + col < buffer.Length)
                    {
                        var b = buffer[row + col];
                        Console.Write(b >= 0x20 ? (char)b : '.');
                    }
                }

                Console.WriteLine();
            }
        }
    }

    static class ArrayHelper
    {
        public static T[] Subset<T>(this T[] array, int startIndex)
        {
            var subset = new T[array.Length - startIndex];
            Array.Copy(array, startIndex, subset, 0, array.Length - startIndex);
            return subset;
        }
    }
}
