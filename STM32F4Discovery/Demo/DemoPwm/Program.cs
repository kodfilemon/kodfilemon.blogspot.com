using System.Threading;
using Microsoft.SPOT.Hardware;

namespace DemoPwm
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

            const int minBright = 0;
            const int maxBright = 100;
            int bright = minBright;
            int step = 1;

            while (true)
            {
                pwm0.DutyCycle = ToDutyCycle(bright);
                pwm1.DutyCycle = ToDutyCycleExp(bright);
                pwm2.DutyCycle = ToDutyCycle10(bright);

                bright += step;
                if (bright > maxBright || bright < minBright)
                {
                    bright = bright > maxBright ? maxBright : minBright;
                    step = -step;

                    Thread.Sleep(1000);
                }

                Thread.Sleep(40);
            }
        }

        private static double ToDutyCycle(double brightness)
        {
            return brightness/100;
        }

        private static double ToDutyCycleExp(double brightness)
        {
            if (brightness < 1)
                return 0;

            if (brightness > 99)
                return 1;

            return (0.383*System.Math.Exp(0.0555*brightness) - 0.196)/100;
        }

        private static double ToDutyCycle10(double brightness)
        {
            if (brightness < 1)
                return 0;

            if (brightness > 99)
                return 1;

            return System.Math.Pow(10, (2.55*brightness - 1)/84.33 - 1)/100;
        }
    }
}
