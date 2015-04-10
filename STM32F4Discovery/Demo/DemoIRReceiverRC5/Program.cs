using System;
using System.Threading;
using Common;
using Microsoft.SPOT;

namespace DemoIRReceiverRC5
{
    public class Program
    {
        private static DateTime _nextCommand = DateTime.MinValue;

        public static void Main()
        {
            using(var receiver = new IRReceiver(Stm32F4Discovery.FreePins.PB5))
            {
                var detector = new RC5Decoder(receiver);
                detector.Frame += (s, f) =>
                                      {
                                          DateTime now = DateTime.Now;
                                          if (now < _nextCommand)
                                              return;

                                          Debug.Print("Addr:" + f.Address +
                                                      " Cmd:" + f.Command +
                                                      " Toggle: " + f.Toggle);

                                          _nextCommand = now.AddMilliseconds(500);
                                      };

                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
