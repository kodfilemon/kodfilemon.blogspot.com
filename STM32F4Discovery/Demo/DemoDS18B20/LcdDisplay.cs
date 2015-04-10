using System;
using Common;
using MicroLiquidCrystal;

namespace DemoDS18B20
{
    public class LcdDisplay : IDisplay
    {
        private readonly Lcd _lcd;
        private const byte Rows = 2;
        private const byte Columns = 16;

        public LcdDisplay()
        {
            var lcdProvider = new GpioLcdTransferProvider(Stm32F4Discovery.Pins.PD1, Stm32F4Discovery.Pins.PD2,
                                                          Stm32F4Discovery.Pins.PD9, Stm32F4Discovery.Pins.PD11,
                                                          Stm32F4Discovery.Pins.PD10, Stm32F4Discovery.Pins.PD8);

            _lcd = new Lcd(lcdProvider);

            _lcd.Begin(Columns, Rows);
            _lcd.Write("Wait...");

            //http://www.quinapalus.com/hd44780udg.html
            _lcd.CreateChar(0, new byte[] {0x8, 0x14, 0x8, 0x3, 0x4, 0x4, 0x3, 0x0});
            _lcd.Backlight = false;
        }

        public void ShowTemperature(float temperature)
        {
            _lcd.SetCursorPosition(0, 0);
            const string txt = "Temperatura:";
            int padsCnt = Columns - txt.Length;
            _lcd.Write(txt + new string(' ', padsCnt));

            _lcd.SetCursorPosition(0, 1);

            string tempStr = temperature.ToString("F2");
            padsCnt = Columns - tempStr.Length;
            _lcd.Write(tempStr);
            _lcd.WriteByte(0); //znak specjalny 

            if (padsCnt > 0)
            {
                var pads = new String(' ', padsCnt);
                _lcd.Write(pads);
            }
        }

        public void ShowError(string message)
        {
            if (message == null) 
                throw new ArgumentNullException("message");

            bool split = message.Length > Columns;

            string line1 = split ? message.Substring(0, Columns) : message;
            _lcd.Clear();
            _lcd.Write(line1);

            if(split)
            {
                string line2 = message.Substring(Columns, message.Length - Columns);
                _lcd.SetCursorPosition(0, 1);
                _lcd.Write(line2);
            }
        }
    }
}