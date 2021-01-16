using System;
using System.IO;

namespace MpxFlash
{
    class MpxSplitter
    {
        public static void SplitFile(string filename)
        {
            Console.WriteLine($"Reading file '{filename}'...");
            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line.Length <= 6)
                    {
                    }
                    else if (line.Length <= 13 && line[0] == ':')
                    {
                        string hexfile = GetFileName(filename, line.Substring(1));
                        Console.WriteLine($"Creating HEX file '{hexfile}'...");

                        using (var writer = new StreamWriter(hexfile))
                        {
                            while (!reader.EndOfStream)
                            {
                                string hex = reader.ReadLine();
                                if (hex.Length <= 6)
                                {
                                    break;
                                }

                                writer.WriteLine(hex);

                                if (hex == ":00000001FF")
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Done.");
        }

        static string GetFileName(string path, string line)
        {
            string prefix = "_";
            while (line.Length >= 2)
            {
                var ch = (line[0] - '0') * 10 + line[1] - '0';
                prefix += ch > 32 ? (char)ch : '_';
                line = line.Substring(2);
            }

            return Path.ChangeExtension(Path.GetFileNameWithoutExtension(path) + prefix, "hex");
        }
    }
}
