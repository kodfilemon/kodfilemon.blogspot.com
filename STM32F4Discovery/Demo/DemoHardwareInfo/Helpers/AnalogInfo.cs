using Common;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace DemoHardwareInfo.Helpers
{
    public static class AnalogInfo
    {
        public static void Print()
        {
            HardwareProvider provider = HardwareProvider.HwProvider;
            var cnt = provider.GetAnalogChannelsCount();
            for (int i = 0; i < cnt; i++)
            {
                var channel = (Cpu.AnalogChannel) i;
                Cpu.Pin pin = provider.GetAnalogPinForChannel(channel);
                int[] precisions = provider.GetAvailablePrecisionInBitsForChannel(channel);

                Debug.Print("AnalogChannel" + channel +
                            ": pin=" + Stm32F4Discovery.GetPinName(pin) +
                            " precisions=" + precisions.Join(", "));
            }
        }
    }
}