using System.Threading;
using Microsoft.SPOT.Hardware;

namespace DemoServoSG90
{
    public class Program
    {
        public static void Main()
        {
            const uint period = 20000; //20ms in us
            const uint minDuration = 530; //0.53ms in us
            const uint maxDuration = 2350; //2.35ms in us

            var servo = new PWM(Cpu.PWMChannel.PWM_0, period, minDuration,
                                PWM.ScaleFactor.Microseconds, false);
            servo.Start();

            uint step = 30;
            for (uint angle = 0;; angle += step)
            {
                if (angle > 180)
                {
                    step = (uint) -step;
                    continue;
                }

                servo.Duration = Map(angle, 0, 180, minDuration, maxDuration);
                Thread.Sleep(2000);
            }
        }

        private static uint Map(uint x, uint minX, uint maxX, uint outMin, uint outMax)
        {
            return (x - minX)*(outMax - outMin)/(maxX - minX) + outMin;
        }
    }
}
