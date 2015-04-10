using Common;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace DemoHardwareInfo.Helpers
{
    public static class AnalogOutputInfo
    {
        public static void Print()
        {
            HardwareProvider provider = HardwareProvider.HwProvider;
            int cnt = provider.GetAnalogOutputChannelsCount();
            for (int i = 0; i < cnt; i++)
            {
                var channel = (Cpu.AnalogOutputChannel) i;
                Cpu.Pin pin = provider.GetAnalogOutputPinForChannel(channel);
                int[] precisions = provider.GetAvailableAnalogOutputPrecisionInBitsForChannel(channel);

                Debug.Print("AnalogOutputChannel" + channel +
                            ": pin=" + Stm32F4Discovery.GetPinName(pin) +
                            " precisions=" + precisions.Join(", "));
            }
        }
    }
}