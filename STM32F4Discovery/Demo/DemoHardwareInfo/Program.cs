using System;
using DemoHardwareInfo.Helpers;
using Microsoft.SPOT;

namespace DemoHardwareInfo
{
    public class Program
    {
        public static void Main()
        {
            CommonInfo.Print();
            Debug.Print(String.Empty);

            I2CInfo.Print();
            Debug.Print(String.Empty);

            PwmInfo.Print();
            Debug.Print(String.Empty);

            AnalogOutputInfo.Print();
            Debug.Print(String.Empty);

            AnalogInfo.Print();
            Debug.Print(String.Empty);

            SpiInfo.Print();
            Debug.Print(String.Empty);

            SerialInfo.Print();
            Debug.Print(String.Empty);

            UsbInfo.Print();
        }
    }
}
