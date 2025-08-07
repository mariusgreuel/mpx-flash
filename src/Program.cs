using Mpx;
using System;

namespace MpxFlash
{
    class Program
    {
        static int Main(string[] arguments)
        {
            return new Program().Run(arguments);
        }

        int Run(string[] arguments)
        {
            try
            {
                PrintLogo();

                options.ParseArguments(arguments);
                if (!options.Validate())
                {
                    PrintUsage();
                    return 2;
                }

                if (options.id)
                {
                    using (var device = new MpxDevice(options.port))
                    {
                        device.PrintIdentification();
                    }
                }
                else if (options.send != null)
                {
                    using (var device = new MpxDevice(options.port))
                    {
                        device.SendHex(options.send);
                    }
                }
                else if (options.flash != null)
                {
                    using (var flasher = new MpxFlasher(options.port))
                    {
                        flasher.FlashDevice(options.flash, options.force);
                    }
                }
                else if (options.split != null)
                {
                    MpxSplitter.SplitFile(options.split);
                }

                return 0;
            }
            catch (Exception e)
            {
                using (var coloredConsole = new ColoredConsole(ConsoleColor.Red))
                {
                    Console.Error.WriteLine($"ERROR: {e.Message}");
                }

                return 1;
            }
        }

        void PrintLogo()
        {
            Console.WriteLine("MPX Flash Tool, V1.0");
            Console.WriteLine("Copyright (C) 2018 Marius Greuel. All rights reserved.");
        }

        void PrintUsage()
        {
            Console.WriteLine("Usage: mpx-flash [@response-file] [options] <files>");
            options.WriteUsage();
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  mpx-flash -port COM3 -id");
            Console.WriteLine("  mpx-flash -port COM3 -flash file.hex");
            Console.WriteLine("  mpx-flash -split file.rmc");
            Console.WriteLine();
            Console.WriteLine("Example HEX commands:");
            Console.WriteLine("  Get firmware version: mpx-flash -port COM3 -send 76");
            Console.WriteLine("  Enter bootloader: mpx-flash -port COM3 -send 02F22D568A03");
            Console.WriteLine("  Exit bootloader: mpx-flash -port COM3 -send 02F34F4B7203");
            Console.WriteLine("  Get configuration: mpx-flash -port COM3 -send 02926D03");
        }

        readonly Options options = new Options();
    }
}
