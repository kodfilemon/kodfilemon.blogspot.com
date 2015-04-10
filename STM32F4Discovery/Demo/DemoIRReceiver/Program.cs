using System;
using System.Threading;
using Common;
using Microsoft.SPOT.Hardware;

namespace DemoIRReceiver
{
    public class Program
    {
        private const int DelayBetweenCommands = 500; //ms

        public static void Main()
        {
            var leds = new[]
                           {
                               new OutputPort(Stm32F4Discovery.LedPins.Green, true),
                               new OutputPort(Stm32F4Discovery.LedPins.Orange, true),
                               new OutputPort(Stm32F4Discovery.LedPins.Red, true),
                               new OutputPort(Stm32F4Discovery.LedPins.Blue, true)
                           };

            var receiver = new IRReceiver(Stm32F4Discovery.FreePins.PB5);

            DateTime nextCommand = DateTime.MinValue;
            receiver.Pulse += (width, state) =>
                                  {
                                      DateTime now = DateTime.Now;
                                      if (now < nextCommand)
                                          return;

                                      nextCommand = now.AddMilliseconds(DelayBetweenCommands);
                                      Toggle(leds);
                                  };

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Toggle(OutputPort[] leds)
        {
            bool state = !leds[0].Read();
            foreach (OutputPort led in leds)
                led.Write(state);
        }
    }
}
