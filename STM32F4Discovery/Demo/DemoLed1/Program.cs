using System.Threading;
using Microsoft.SPOT.Hardware;

namespace DemoLed1
{
    public class Program
    {
        public static void Main()
        {
            const Cpu.Pin ledPin = (Cpu.Pin) 60;
            const int delay = 1000;

            using (var ledPort = new OutputPort(ledPin, false))
            {
                while (true)
                {
                    ledPort.Write(true);
                    Thread.Sleep(delay);
                    ledPort.Write(false);
                    Thread.Sleep(delay);
                }
            }
        // ReSharper disable FunctionNeverReturns
        }
        // ReSharper restore FunctionNeverReturns
    }
}
