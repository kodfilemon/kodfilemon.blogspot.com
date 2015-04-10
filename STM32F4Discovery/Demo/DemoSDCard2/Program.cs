using System;
using System.IO;
using System.Threading;
using Common;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace DemoSDCard2
{
    public class Program
    {
        private const string SdRoot = @"\SD";
        private static IButtonEventsRepository _repository;
        private static InterruptPort _userButton;

        public static void Main()
        {
//            RemovableMedia.Insert += (o, e) => Debug.Print("Insert: " + e.Volume.Name);
//            RemovableMedia.Eject += (o, e) => Debug.Print("Eject: " + e.Volume.Name);


            string targetFile = Path.Combine(SdRoot, "test.txt");
            _repository = new SdCardRepository(targetFile);

            //File.Delete(targetFile);

            PrintContent();

            _userButton = new InterruptPort(Stm32F4Discovery.ButtonPins.User,
                                            true,
                                            Port.ResistorMode.PullDown,
                                            Port.InterruptMode.InterruptEdgeBoth);

            _userButton.OnInterrupt += UserButton_OnInterrupt;
            Thread.Sleep(Timeout.Infinite);
        }

        static void UserButton_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            var newEvent = new ButtonEvent(time, data2 == 1);
            _repository.Add(newEvent);

            PrintContent();
        }

        private static void PrintContent()
        {
            ButtonEvent[] records = _repository.GetAll();
            foreach (ButtonEvent record in records)
                Debug.Print(record.EventTime + " " + record.State);
        }
    }
}
