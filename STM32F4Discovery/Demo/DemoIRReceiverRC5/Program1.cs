using System;
using System.Threading;
using Common;
using Microsoft.SPOT;

namespace DemoIRReceiverRC5
{
    public class Program
    {
        private const int MinWidth = 640; //us
        private const int MaxWidth = 1140; //us

        private static DateTime _nextCommand = DateTime.MinValue;
        private static int _okImpulses;

        public static void Main()
        {
            using(var receiver = new IRReceiver(Stm32F4Discovery.FreePins.PB5))
            {
                receiver.Pulse += ConsumePulse;
                Thread.Sleep(Timeout.Infinite);
            }
        }

        private static void ConsumePulse(TimeSpan width, bool state)
        {
            DateTime now = DateTime.Now;
            if (now < _nextCommand)
                return;

            long usWidth = width.TotalMicroseconds();
            //czekamy na 1
            if (state)
            {
                if (usWidth > MinWidth && usWidth < MaxWidth)
                    _okImpulses++;
            }
            else
            {
                if (_okImpulses == 1)
                {
                    if (usWidth > MinWidth && usWidth < MaxWidth)
                        Debug.Print("RC5!");

                    _okImpulses = 0;
                    _nextCommand = now.AddMilliseconds(500);
                }
            }
        }
    }
}
