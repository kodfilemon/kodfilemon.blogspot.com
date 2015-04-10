using Common;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace DemoHardwareInfo.Helpers
{
    public static class PwmInfo
    {
        public static void Print()
        {
            HardwareProvider provider = HardwareProvider.HwProvider;

            int cnt = provider.GetPWMChannelsCount();
            for (int i = 0; i < cnt; i++)
            {
                var channel = (Cpu.PWMChannel) i;
                Cpu.Pin pin = provider.GetPwmPinForChannel(channel);
                Debug.Print("PWMChannel" + channel + ": pin=" + Stm32F4Discovery.GetPinName(pin));
            }
        }
    }
}