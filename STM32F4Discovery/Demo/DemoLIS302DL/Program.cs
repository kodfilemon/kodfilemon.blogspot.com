using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using Common;
using Microsoft.SPOT;

namespace DemoLIS302DL
{
    public class Program
    {
        public static void Main()
        {
            DateTime printTime = DateTime.Now;

            var serial = new SerialPort("COM2");
            serial.Open();

            var mems = new Lis302Dl(Stm32F4Discovery.SpiDevices.SPI1, Stm32F4Discovery.Pins.PE3);
            for (; ; )
            {
                sbyte x, y, z;
                mems.GetRaw(out x, out y, out z);

                double gx, gy, gz;
                mems.GetAcc(out gx, out gy, out gz);

                const double g = 9.80665;
                gx = gx * g;
                gy = gy * g;
                gz = gz * g;

                const double toDegree = 180 / System.Math.PI;
                double roll = System.Math.Atan2(gy, gz) * toDegree;
                double pitch = -System.Math.Atan2(gx, System.Math.Sqrt(gy * gy + gz * gz)) * toDegree;

                string accStr = gx.ToString("F3") + ";" + gy.ToString("F3") + ";" + gz.ToString("F3");
                if (DateTime.Now > printTime)
                {
                    Debug.Print("Raw:" + x + ";" + y + ";" + z);
                    Debug.Print("Acc:" + accStr);
                    Debug.Print("Pitch:" + pitch.ToString("F0") + " Roll:" + roll.ToString("F0"));
                    Debug.Print("");

                    printTime = DateTime.Now.AddSeconds(1);
                }

                string sendStr = accStr + ";"
                                 + pitch.ToString("F1") + ";"
                                 + roll.ToString("F1")
                                 + "\r\n";

                byte[] sendBuffer = Encoding.UTF8.GetBytes(sendStr);
                serial.Write(sendBuffer, 0, sendBuffer.Length);
                Thread.Sleep(50);
            }
        }
    }
}
