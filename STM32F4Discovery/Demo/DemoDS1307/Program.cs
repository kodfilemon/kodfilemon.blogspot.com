using System;
using System.Threading;
using Common;
using MicroLiquidCrystal;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace DemoDS1307
{
    public class Program
    {
        private const int Columns = 16;
        private const int ShowUptimeInterval = 10; //seconds

        private static readonly DS1307 Ds1307 = new DS1307();

        public static void Main()
        {
            DateTime dt = Ds1307.GetDateTime();
            Utility.SetLocalTime(dt);

            var lcdProvider = new GpioLcdTransferProvider(Stm32F4Discovery.Pins.PD1,
                                                          Stm32F4Discovery.Pins.PD2,
                                                          Stm32F4Discovery.Pins.PD9,
                                                          Stm32F4Discovery.Pins.PD11,
                                                          Stm32F4Discovery.Pins.PD10,
                                                          Stm32F4Discovery.Pins.PD8);

            var lcd = new Lcd(lcdProvider);
            lcd.Begin(Columns, 2);

            var userButton = new InterruptPort(Stm32F4Discovery.ButtonPins.User,
                                               false, Port.ResistorMode.PullDown,
                                               Port.InterruptMode.InterruptEdgeLow);

            DateTime showUptimeMode = DateTime.MinValue;
            userButton.OnInterrupt += (d1, d2, t) => showUptimeMode = DateTime.Now.AddSeconds(ShowUptimeInterval);
            
            for (;;)
            {
                var now = DateTime.Now;

                string line1, line2;

                if(showUptimeMode > now)
                {
                    TimeSpan uptime = GetUptime();
                    string uptimeStr = uptime.ToString();
                    int endIndex = uptimeStr.LastIndexOf('.');
                    if(endIndex > Columns)
                        endIndex = Columns;

                    line1 = "Uptime:   ";
                    line2 = uptimeStr.Substring(0, endIndex);
                }
                else
                {
                    line1 = now.ToString("yyyy-MM-dd");
                    line2 = now.ToString("HH:mm:ss        ");
                }

                lcd.SetCursorPosition(0, 0);
                lcd.Write(line1);
                lcd.SetCursorPosition(0, 1);
                lcd.Write(line2);

                Thread.Sleep(100);
            }
        }

        private static TimeSpan GetUptime()
        {
            TimeSpan result = TimeSpan.MinValue;

            byte[] store = Ds1307.ReadRam();
            if (store.Length > 0)
            {
                var setTime = (DateTime) Reflection.Deserialize(store, typeof (DateTime));
                result = DateTime.Now - setTime;
            }

            return result;
        }
    }
}
