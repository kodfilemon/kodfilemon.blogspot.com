using Common;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.IO;

namespace DemoSDCard1
{
    public class Program
    {
        public static void Main()
        {
            var okLed = new OutputPort(Stm32F4Discovery.LedPins.Green, false);
            var errorLed = new OutputPort(Stm32F4Discovery.LedPins.Red, false);

            string[] availFs = VolumeInfo.GetFileSystems();
            if (availFs.Length == 0)
            {
                Debug.Print("No FS found");
                errorLed.Write(true);
                return;
            }

            foreach (string fs in availFs)
                Debug.Print("Available FS: " + fs);

            VolumeInfo[] volumes = VolumeInfo.GetVolumes();
            if(volumes.Length == 0)
            {
                Debug.Print("No volumes found");
                errorLed.Write(true);
                return;
            }

            foreach (VolumeInfo volume in volumes)
            {
                if (volume.TotalSize > 0)
                {
                    Debug.Print("Volume: " + volume.Name);
                    okLed.Write(true);
                }
                else
                {
                    Debug.Print("Invalid volume");
                    errorLed.Write(true);
                }
            }
        }
    }
}
