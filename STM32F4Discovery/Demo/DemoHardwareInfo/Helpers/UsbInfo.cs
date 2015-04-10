using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware.UsbClient;

namespace DemoHardwareInfo.Helpers
{
    public static class UsbInfo
    {
        public static void Print()
        {
            UsbController[] controllers = UsbController.GetControllers();
            for (int i = 0; i < controllers.Length; i++)
            {
                Debug.Print("USB" + i + ": " + Convert(controllers[i].Status));
                Thread.Sleep(500);
            }
        }

        private static string Convert(UsbController.PortState status)
        {
            if (status == UsbController.PortState.Stopped)
                return "stopped";

            if (status == UsbController.PortState.Running)
                return "running";

            return status.ToString();
        }
    }
}