using System;
using Microsoft.SPOT;
using System.Threading;
using Common;
using Microsoft.SPOT.Hardware;

namespace DemoHCSR04
{
    public class Program
    {
        public static void Main()
        {
            var sensor = new HCSR04(Stm32F4Discovery.FreePins.PC2, Stm32F4Discovery.FreePins.PC1);

            //crash detect: red led on or off
            var crashLed = new OutputPort(Stm32F4Discovery.LedPins.Red, false);
            var scanPeriod = new TimeSpan(0, 0, 0, 0, 100);//100ms
            var detector = new CollisionDetector(sensor, scanPeriod) { Barier = 10 }; //10cm
            detector.StateChanged += crashLed.Write;

            //pwm frequency display (green led linear blink): far->lo freq, near->hi freq
            var distancePwm = new PWM(Cpu.PWMChannel.PWM_0, 1, 0, false);
            distancePwm.Start();
            
            //print distance every 1s and change pwm freq
            DateTime nextPrint = DateTime.Now;
            for(;;)
            {
                TimeSpan pulse = sensor.Ping();

                if (DateTime.Now > nextPrint)
                {
                    float cm = HCSR04.ToCentimeters(pulse);
                    string cmStr = cm.Equals(Single.MaxValue) ? "?" : cm.ToString("F1");

                    float inch = HCSR04.ToInches(pulse);
                    string inStr = inch.Equals(Single.MaxValue) ? "?" : inch.ToString("F1");

                    Debug.Print("Distance: " + cmStr + " cm = " + inStr + " in");
                    nextPrint = DateTime.Now.AddSeconds(1);
                }

                distancePwm.Frequency = ToFrequency(pulse);
                distancePwm.DutyCycle = 0.5;

                Thread.Sleep(200);
            }
        }

        public static float ToFrequency(TimeSpan pulse)
        {
            //0-30 cm --> 12-1 Hz

            const int barier = 30;//cm

            float cm = HCSR04.ToCentimeters(pulse);
            if (cm > barier)
                return 1;

            const int maxFreq = 12;//Hz

            float result = (cm * (1 - maxFreq)) / barier + maxFreq;
            return result;
        }
    }
}
