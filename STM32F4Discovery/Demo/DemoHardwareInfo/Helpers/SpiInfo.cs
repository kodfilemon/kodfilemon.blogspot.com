using Common;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace DemoHardwareInfo.Helpers
{
    public static class SpiInfo
    {
        public static void Print()
        {
            HardwareProvider provider = HardwareProvider.HwProvider;
            var cnt = provider.GetSpiPortsCount();
            for (int i = 0; i < cnt; i++)
            {
                var module = (SPI.SPI_module) i;
                Cpu.Pin msk, miso, mosi;
                provider.GetSpiPins(module, out msk, out miso, out mosi);
                Debug.Print("SPI_module" + (i + 1) +
                            ": (msk, miso, mosi)=(" +
                            Stm32F4Discovery.GetPinName(msk) + ", " +
                            Stm32F4Discovery.GetPinName(miso) + ", " +
                            Stm32F4Discovery.GetPinName(mosi) + ")");
            }
        }
    }
}