using Mpx;
using System;

namespace MpxFlash
{
    public class Options : CommandLineOptions
    {
        public bool Validate()
        {
            if (split != null)
                return true;

            if (port != null && id)
                return true;

            if (port != null && send != null)
                return true;

            if (port != null && flash != null)
                return true;

            return false;
        }

        [CommandLineOption(Arguments = "COMx", Description = "Specifies the COM port to be used")]
        public string port = null;

        [CommandLineOption(Description = "Print the device identification")]
        public bool id = false;

        [CommandLineOption(Arguments = "command", Description = "Send a string of HEX bytes to the device")]
        public string send = null;

        [CommandLineOption(Arguments = "filename", Description = "Flash the specified HEX file into the device")]
        public string flash = null;

        [CommandLineOption(Arguments = "filename", Description = "Split a flash collection file into individual HEX files")]
        public string split = null;

        [CommandLineOption(Description = "Force operation to complete")]
        public bool force = false;

        [CommandLineOption(Alias = "?", Category = "Miscellaneous", Description = "Display full help")]
        public bool h = false;
    }
}
