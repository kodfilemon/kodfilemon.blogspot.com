using Common;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace DemoHardwareInfo.Helpers
{
    public static class I2CInfo
    {
        public static void Print()
        {
            Cpu.Pin scl, sda;
            HardwareProvider.HwProvider.GetI2CPins(out scl, out sda);
            Debug.Print("I2C pins: scl=" + Stm32F4Discovery.GetPinName(scl) +
                        " sda=" + Stm32F4Discovery.GetPinName(sda));
        }
    }
}