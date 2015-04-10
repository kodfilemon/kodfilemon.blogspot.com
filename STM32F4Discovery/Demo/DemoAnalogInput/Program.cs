using System;
using System.Net;
using System.Text;
using Microsoft.SPOT.Hardware;
using Math = System.Math;

namespace DemoAnalogInput
{
    public class Program
    {
        public static void Main()
        {
            var pwm0 = new PWM(Cpu.PWMChannel.PWM_0, 300, 0, false);
            pwm0.Start();

            var pwm1 = new PWM(Cpu.PWMChannel.PWM_1, 300, 0, false);
            pwm1.Start();

            var pwm2 = new PWM(Cpu.PWMChannel.PWM_2, 300, 0, false);
            pwm2.Start();

            var pwm3 = new PWM(Cpu.PWMChannel.PWM_3, 300, 0, false);
            pwm3.Start();

            using (var analogInput = new AnalogInput(Cpu.AnalogChannel.ANALOG_0))
            {
                analogInput.Scale = 100;
                analogInput.Offset = 0;

                double prevVal = Double.MinValue;
                for (;;)
                {
                    double currentVal = analogInput.Read();
                    //int raw = analogInput.ReadRaw();
                    //Debug.Print("Sample: " + val + " (" + raw + ")");

                    if (Math.Abs(currentVal - prevVal) >= 1)
                    {
                        pwm0.DutyCycle =
                            pwm1.DutyCycle =
                            pwm2.DutyCycle =
                            pwm3.DutyCycle = ToDutyCycle(currentVal);

                        //int volume = ToVolume(currentVal);
                        //SendXbmcVolume(volume);

                        prevVal = currentVal;
                    }
                }
            }
        }

        private static double ToDutyCycle(double brightness)
        {
            if (brightness < 1)
                return 0;

            if (brightness > 99)
                return 1;

            return Math.Pow(10, (2.55 * brightness - 1) / 84.33 - 1) / 100;
        }

        private static int ToVolume(double volume)
        {
            double loud = 50 * Math.Log10(volume);

            if (loud < 0)
                return 0;

            if (loud > 100)
                return 100;

            return (int)Math.Round(loud);
        }

        private static void SendXbmcVolume(int volume)
        {
            const string xbmcHost = "10.15.16.2";
            const string xbmcPort = "8081";
            const string xbmcUser = "xbmc";
            const string xbmcPassword = "xbmc";

            string json = "{\"jsonrpc\": \"2.0\", " +
                          "\"method\": \"Application.SetVolume\", " +
                          "\"params\": {\"volume\": " + volume + "}, " +
                          "\"id\": 1}";

            const string xbmcRpc = @"http://" + xbmcHost + ":" + xbmcPort + "/jsonrpc";

            using (var request = (HttpWebRequest)WebRequest.Create(xbmcRpc))
            {
                byte[] content = Encoding.UTF8.GetBytes(json);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.ContentLength = content.Length;
                request.KeepAlive = false;
                request.Credentials = new NetworkCredential(xbmcUser,
                                                            xbmcPassword,
                                                            AuthenticationType.Basic);

                using (var stream = request.GetRequestStream())
                    stream.Write(content, 0, content.Length);
            }
        }
    }
}