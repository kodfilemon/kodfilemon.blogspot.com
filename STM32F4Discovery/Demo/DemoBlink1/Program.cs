using System.Threading;
using Common;
using Microsoft.SPOT.Hardware;

namespace DemoBlink1
{
    public class Program
    {
        private static readonly InterruptPort UserButton = new InterruptPort(Stm32F4Discovery.ButtonPins.User,
                                                                             false,
                                                                             Port.ResistorMode.PullDown,
                                                                             Port.InterruptMode.InterruptEdgeLow);

        private static readonly OutputPort DirectionLed = new OutputPort(Stm32F4Discovery.FreePins.PA15, false);

        public static void Main()
        {
            var leds = new[]
                           {
                               new OutputPort(Stm32F4Discovery.LedPins.Green, true),
                               new OutputPort(Stm32F4Discovery.LedPins.Orange, true),
                               new OutputPort(Stm32F4Discovery.LedPins.Red, true),
                               new OutputPort(Stm32F4Discovery.LedPins.Blue, true)
                           };

            var rotator = new LedRotator(leds);
            DirectionLed.Write(rotator.Right);

            UserButton.OnInterrupt += (u, data2, time) =>
                                          {
                                              rotator.ChangeDirection();
                                              DirectionLed.Write(rotator.Right);
                                              UserButton.ClearInterrupt();
                                          };

            Blink(leds, 6);
            rotator.Run();
        }

        private static void Blink(OutputPort[] leds, int blinkCnt)
        {
            //mrugamy diodami kilka razy (diody musza zgasnac)
            bool ledState = leds[0].Read();
            for (; blinkCnt > 0 || ledState; blinkCnt--)
            {
                ledState = !ledState;
                foreach (OutputPort led in leds)
                    led.Write(ledState);

                Thread.Sleep(1000);
            }
        }
    }
}
