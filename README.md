# MPX Flash Tool

The MPX Flash Tool is a tool that allows you to flash various Multiplex devices, such as M-Link transmitter modules and M-Link receivers.

Features include:

- ID the device.
- Send a command via a HEX string to the device.
- Flash the device via the device bootloader.
- Split a HEX collection file into individual HEX files.

## Usage

To use this tool, you need to connect the device to your Windows PC via a USB Serial Convertor, such as the **Multiplex USB PC-Cable (85149)**.

The correct procedure is as follows:

- Turn the device off
- Connect the USB Serial Convertor to the PC and your device
- Turn the device on

When the procedure is performed correctly, the bootloader starts listening for commands and the `mpx-flash` can be run.

To display the usage of this tool, run `mpx-flash` from a command-prompt.

## Downloads

To download a pre-build executable, go to the [releases](https://github.com/mariusgreuel/mpx-flash/releases) folder.

### Prerequisites

- On Windows, you need Windows 7 or higher with the .NET Framework installed.
- On Linux or macOS, you need Mono to be installed. You can execute **mpx-flash** by running `mono mpx-flash.exe`.

## Building the software

This software is written in C# using Visual Studio 2019.

## MPX Bootloader Notes

The MPX bootloader connects via a one-wire serial connection at 38400 baud (8,1,N).

The precense of a USB Serial Convertor is performed by sensing the serial wire: If the wire is pulled high at power-up, the bootloader starts listening on the serial connection for commands. Otherwise, the application is started.

The bootloader accepts two basic single-character commands, which can be send via a simple terminal program:

- `v`: Print software version information
- `x`: Execute application

Depending on the device type, there are several other device type specific commands. Inspect the source code for the flash commands.

## License

[GNU GPLv3](./LICENSE)

Copyright (C) 2018 Marius Greuel.
