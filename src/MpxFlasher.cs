using System;
using System.IO;
using System.Threading;

namespace MpxFlash
{
    class MpxFlasher : MpxDevice
    {
        public MpxFlasher(string port) : base(port)
        {
        }

        public void FlashDevice(string filename, bool force = false)
        {
            PrintIdentification();
            var version = GetIdentificationString();

            if (!force)
            {
                var fileID = Path.GetFileNameWithoutExtension(filename).ToUpperInvariant();
                var deviceID = version.ToUpperInvariant();
                if (!fileID.EndsWith(deviceID))
                {
                    throw new ApplicationException($"Filename does not match ID '{version}', use -force to override.");
                }
            }

            if (version.StartsWith("THFM"))
            {
                FlashTransmitter(filename);
            }
            else if (version.StartsWith("RM"))
            {
                FlashReceiver(filename);
            }
            else
            {
                throw new ApplicationException($"Unknown device version string '{version}'.");
            }
        }

        void FlashTransmitter(string filename)
        {
            using (var reader = new StreamReader(filename))
            {
                Console.WriteLine("Entering programming mode, please wait...");
                PrintResponseDump(SendCommand("p"));
                Thread.Sleep(100);

                Console.WriteLine("Programming device...");

                int record = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Trim();
                    if (line == ":00000001FF")
                    {
                        Console.WriteLine("Sending final record...");
                        PrintAndVerifyResponse(SendCommand(line + '\r'), "ok");
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Sending record {record}...");
                        PrintAndVerifyResponse(SendCommand(line + '\r'), "ok");
                        record++;
                    }
                }
            }

            Console.WriteLine("Done.");
        }

        void FlashReceiver(string filename)
        {
            Console.WriteLine("Entering bootloader...");
            PrintResponseDump(SendCommand(cmdEnterBootloader));

            Console.WriteLine("Getting bootloader version...");
            PrintAndVerifyResponse(SendCommand("v"), "ok");

            Console.WriteLine($"Reading file '{filename}'...");

            int record = 0;
            using (var reader = new StreamReader(filename))
            {
                Console.WriteLine("Entering programming mode, please wait...");
                PrintResponseDump(SendCommand("p"));
                Thread.Sleep(6000);

                Console.WriteLine("Programming device...");
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Trim();
                    if (line == ":00000001FF")
                    {
                        Console.WriteLine("Sending final record...");
                        PrintResponseDump(SendCommand(line + '\r'));
                        Thread.Sleep(1000);
                        PrintAndVerifyResponse(ReadData(), "Z");
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Sending record {record}...");
                        PrintAndVerifyResponse(SendCommand(line + '\r'), "G");
                        record++;
                    }
                }
            }

            Thread.Sleep(1000);
            PrintResponseDump(SendCommand("u"));

            Thread.Sleep(100);
            PrintResponseDump(SendCommand(cmdExitBootloader));

            Console.WriteLine("Done.");
        }

        static readonly byte[] cmdEnterBootloader = new byte[] { 0x02, 0xF2, 0x2D, 0x56, 0x8A, 0x03 };
        static readonly byte[] cmdExitBootloader = new byte[] { 0x02, 0xF3, 0x4F, 0x4B, 0x72, 0x03 };
    }
}
