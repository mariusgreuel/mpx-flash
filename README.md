# MPX Flash Tool

The MPX Flash Tool is a tool that allows you to flash various Multiplex devices, such as M-Link transmitter modules and M-Link receivers.

Features include:

- ID the device.
- Send a command via a HEX string to the device.
- Flash the device via the device bootloader.
- Split a HEX collection file into individual HEX files.

## Usage

To use this tool, you need to connect the device to your Windows PC via a USB Serial Convertor, such as the **Multiplex USB PC-Cable (85149)**.

To display the usage of this tool, run `mpx-flash` from a command-prompt.

## Downloads

To download a pre-build executable, go to the [releases](https://github.com/rc-hacks/mpx-flash/releases) folder.

### Prerequisites

- On Windows, you need Windows 7 or higher with the .NET Framework installed.
- On Linux or macOS, you need Mono to be installed. You can execute **mpx-flash** by running `mono mpx-flash.exe`.

## Building the software

This software is written in C# using Visual Studio 2019.

## License

[GNU GPLv3](./LICENSE)

Copyright (C) 2018 Marius Greuel.
