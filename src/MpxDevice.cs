using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace MpxFlash
{
    class MpxDevice : IDisposable
    {
        public MpxDevice(string port)
        {
            Console.WriteLine($"Opening COM port '{port}'...");
            serialPort.PortName = port;
            serialPort.BaudRate = 38400;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Parity = Parity.None;
            serialPort.ReadTimeout = 50;
            serialPort.WriteTimeout = 50;
            serialPort.Open();
        }

        public void PrintIdentification()
        {
            try
            {
                Console.WriteLine("Getting firmware version...");
                PrintAndVerifyResponse(SendCommand("v"), "ok");
            }
            catch (ApplicationException)
            {
                try
                {
                    Console.WriteLine("Resetting device...");
                    PrintResponseDump(SendCommand("x"));
                    Thread.Sleep(1000);

                    Console.WriteLine("Getting firmware version...");
                    PrintAndVerifyResponse(SendCommand("v"), "ok");
                }
                finally
                {
                    Console.Error.WriteLine("ERROR: Check the power and the serial communication link. You may need to power-cycle the device.");
                }
            }
        }

        public void DumpData()
        {
            Console.WriteLine("Getting device data...");
            PrintResponseDump(SendCommand(new byte[] { 0x02, 0x80, 0x7F, 0x03 }));
            PrintResponseDump(SendCommand(new byte[] { 0x02, 0x84, 0x7B, 0x03 }));
            PrintResponseDump(SendCommand(new byte[] { 0x02, 0x88, 0x77, 0x03 }));
            PrintResponseDump(SendCommand(new byte[] { 0x02, 0xA2, 0x5D, 0x03 }));
            PrintResponseDump(SendCommand(new byte[] { 0x02, 0xB2, 0x4D, 0x03 }));
            PrintResponseDump(SendCommand(new byte[] { 0x02, 0xC2, 0x3D, 0x03 }));
            PrintResponseDump(SendCommand(new byte[] { 0x02, 0x8C, 0x73, 0x03 }));
        }

        public void SendHex(string hexValues)
        {
            List<byte> values = new List<byte>();

            while (hexValues.Length >= 2)
            {
                values.Add(byte.Parse(hexValues.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture));
                hexValues = hexValues.Substring(2);
            }

            PrintResponseDump(SendCommand(values.ToArray()));
        }

        protected string GetIdentificationString()
        {
            var data = FormatData(SendCommand("v")).Trim();

            if (data.ToLowerInvariant().StartsWith("v"))
            {
                data = data.Substring(1).Trim();
            }

            return data.Substring(0, 6).Trim().Replace(' ', '_');
        }

        protected byte[] SendCommand(string text)
        {
            return SendCommand(Encoding.ASCII.GetBytes(text));
        }

        protected byte[] SendCommand(byte[] buffer)
        {
            WriteData(buffer);
            var response = ReadData();
            return response.Length > buffer.Length ? response.Subset(buffer.Length) : new byte[] { };
        }

        protected void PrintResponseDump(byte[] buffer)
        {
            Tools.DumpBuffer(buffer);
        }

        protected void PrintAndVerifyResponse(byte[] buffer, string expectedResponse)
        {
            var response = FormatData(buffer);
            Console.WriteLine(response.Trim());

            if (!response.EndsWith(expectedResponse))
            {
                throw new ApplicationException("Device returned unexpected response");
            }
        }

        protected byte[] ReadData()
        {
            var bytes = new List<byte>();

            try
            {
                while (true)
                {
                    var ch = serialPort.ReadByte();
                    if (ch < 0)
                        break;

                    bytes.Add((byte)ch);
                }
            }
            catch (TimeoutException)
            {
            }

            return bytes.ToArray();
        }

        protected void WriteData(byte[] buffer)
        {
            serialPort.Write(buffer, 0, buffer.Length);
        }

        static string FormatData(byte[] buffer)
        {
            var text = Encoding.ASCII.GetString(buffer);
            return text.Replace("\r", Environment.NewLine);
        }

        void IDisposable.Dispose()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        SerialPort serialPort = new SerialPort();
    }
}
