using System.IO.Ports;
using Common;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace DemoHardwareInfo.Helpers
{
    public static class SerialInfo
    {
        public static void Print()
        {
            HardwareProvider provider = HardwareProvider.HwProvider;
            var cnt = provider.GetSerialPortsCount();
            for (int i = 0; i < cnt; i++)
            {
                string comPort = Serial.COM1.Substring(0, 3) + (i + 1);
                Cpu.Pin rxPin, txPin, ctsPin, rtsPin;
                provider.GetSerialPins(comPort, out rxPin, out txPin, out ctsPin, out rtsPin);
                uint max, min;
                provider.GetBaudRateBoundary(i, out max, out min);

                Debug.Print(comPort + ": (rx, tx, cts, rts)=(" +
                            Stm32F4Discovery.GetPinName(rxPin) + ", " +
                            Stm32F4Discovery.GetPinName(txPin) + ", " +
                            Stm32F4Discovery.GetPinName(ctsPin) + ", " +
                            Stm32F4Discovery.GetPinName(rtsPin) + ")" +
                            " baud=" + min + "..." + max);
            }
        }
    }
}