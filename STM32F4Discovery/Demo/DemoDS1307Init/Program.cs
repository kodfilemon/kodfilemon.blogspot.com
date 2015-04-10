using System;
using System.Threading;
using Common;
using DemoDS1307;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace DemoDS1307Init
{
    public class Program
    {
        public static void Main()
        {
            //set current date and time + 1 or 2 minutes
            var newDateTime = new DateTime(2012, 09, 04, 21, 30, 45);

            Debug.Print("Wait for " + newDateTime);

            using (var userButton = new InterruptPort(Stm32F4Discovery.ButtonPins.User,
                                                      false, Port.ResistorMode.PullDown,
                                                      Port.InterruptMode.InterruptEdgeLow))
            {
                var ds1307 = new DS1307();
                byte[] storeData = Reflection.Serialize(newDateTime, typeof (DateTime));
                ds1307.WriteRam(storeData);

                //push userbutton when time comes
                userButton.OnInterrupt += (d1, d2, t) =>
                                              {
                                                  ds1307.SetDateTime(newDateTime);
                                                  Debug.Print("Initialized");
                                              };

                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
