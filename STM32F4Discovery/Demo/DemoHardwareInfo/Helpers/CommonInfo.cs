using Common;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace DemoHardwareInfo.Helpers
{
    public static class CommonInfo
    {
        public static void Print()
        {
            Debug.Print("SystemClock: " + Cpu.SystemClock);
            Debug.Print("SlowClock: " + Cpu.SlowClock);
            Debug.Print("GlitchFilterTime: " + Cpu.GlitchFilterTime.TotalMiliseconds() + " ms");
            Debug.Print("PowerLevel: " + PowerState.CurrentPowerLevel);
            Debug.Print("Uptime: " + PowerState.Uptime);
            
            Debug.Print("OEMString: " + SystemInfo.OEMString);
            Debug.Print("Version: " + SystemInfo.Version);

            Debug.Print("Total pins:" +HardwareProvider.HwProvider.GetPinsCount());
        }
    }
}