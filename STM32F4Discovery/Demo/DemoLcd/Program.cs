using System.Threading;
using Common;
using MicroLiquidCrystal;

namespace DemoLcd
{
    public class Program
    {
        public static void Main()
        {
            var lcdProvider = new GpioLcdTransferProvider(Stm32F4Discovery.Pins.PD1, Stm32F4Discovery.Pins.PD2,
                                                          Stm32F4Discovery.Pins.PD9, Stm32F4Discovery.Pins.PD11,
                                                          Stm32F4Discovery.Pins.PD10, Stm32F4Discovery.Pins.PD8);

            var lcd = new Lcd(lcdProvider);
            lcd.Begin(16, 2); //columns, rows

            //znaki specjalne
            //http://www.quinapalus.com/hd44780udg.html
            var customCharacters = new[]
                                       {
                                           new byte[] {0x00, 0x0a, 0x15, 0x11, 0x11, 0x0a, 0x04, 0x00}, //serce
                                           new byte[] {0x04, 0x02, 0x01, 0x1f, 0x01, 0x02, 0x04, 0x00} //strzalka
                                       };

            //ladowanie znakow specjalnych
            for (int i = 0; i < customCharacters.Length; i++)
                lcd.CreateChar(i, customCharacters[i]);

            lcd.Clear();
            lcd.Write("* Hello World! *");
            Thread.Sleep(3000);

//            lcd.Clear();
//            lcd.Encoding = Encoding.UTF8;
//            lcd.Write("ĄąĆćĘęŁłŃńÓóŚśŻż");
//            Thread.Sleep(3000);

            lcd.Clear();
            lcd.WriteByte(0); //pierwszy znak specjalny
            Thread.Sleep(2000);
            lcd.WriteByte(1); //drugi znak specjalny
            Thread.Sleep(3000);

            //nastepna linia
            lcd.SetCursorPosition(0, 1);
            lcd.Write("#     Bye...   #");
        }
    }
}
