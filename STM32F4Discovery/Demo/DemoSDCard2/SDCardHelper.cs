using System;
using System.Runtime.CompilerServices;
using Microsoft.SPOT.Hardware;

public static class SDCardHelper
{
    private static string _sdCardPath;

    public static void Mount(string path, SPI.SPI_module spi, Cpu.Pin chipSelect)
    {
        Mount(path, spi, chipSelect, Cpu.Pin.GPIO_NONE);
    }

    public static void Mount(string path, SPI.SPI_module spi, Cpu.Pin chipSelect, Cpu.Pin cardDetect)
    {
        if (_sdCardPath != null)
            throw new NotSupportedException();

        MountNative(path, (uint) spi, (uint) chipSelect, (uint) cardDetect);
        _sdCardPath = path;
    }

    public static void Unmount(string path)
    {
        if (_sdCardPath != path)
            throw new ArgumentException();

        UnmountNative();
        _sdCardPath = null;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void MountNative(string path, uint spi, uint chipSelectPort, uint cardDetectPin);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void UnmountNative();
}