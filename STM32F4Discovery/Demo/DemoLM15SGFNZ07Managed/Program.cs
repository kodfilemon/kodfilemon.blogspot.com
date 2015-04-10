using System;
using System.Threading;
using Common;
using Microsoft.SPOT.Hardware;

namespace DemoLM15SGFNZ07Managed
{
    public class Program
    {
        public static void Main()
        {
            const Cpu.Pin csPin = Stm32F4Discovery.FreePins.PE11;
            const Cpu.Pin resetPin = Stm32F4Discovery.FreePins.PE10;
            const Cpu.Pin rsPin = Stm32F4Discovery.FreePins.PE9;

            const ushort black = 0x0000;
            const ushort white = 0x0FFF;
            const ushort red = 0x0F00;
            const ushort blue = 0x000F;
            const ushort green = 0x00F0;
            const ushort yellow = 0x0FF0;
            var colors = new[] { red, blue, green, yellow, black };

            var lcd = new Lm15Sgfnz07(SPI.SPI_module.SPI2, csPin, resetPin, rsPin);
            lcd.Contrast(43);
            lcd.Clear(white);

            lcd.FillRectangle(red, 10, 10, Lm15Sgfnz07.Width - (2 * 10), 20);
            const string text = ".NET MF STM32F4";
            int textWidth = lcd.MeasureTextWidth(text);
            lcd.Text(text, (Lm15Sgfnz07.Width - textWidth)/2, 35, black, white);

            lcd.Rectangle(blue, 5, 5, Lm15Sgfnz07.Width - (2 * 5), Lm15Sgfnz07.Height - (2 * 5));

            lcd.Line(0x888, 0, Lm15Sgfnz07.Width, 0, Lm15Sgfnz07.Height);
            lcd.Line(0x888, Lm15Sgfnz07.Width, 0, 0, Lm15Sgfnz07.Height);

            byte[] resSmile = Resources.GetBytes(Resources.BinaryResources.smile);

            int smileWidth, smileHeight, smileDepth;
            ushort[] imgSmile = Lm15Sgfnz07.PpmToImage(resSmile, out smileWidth, out smileHeight, out smileDepth);
            lcd.DrawImage(imgSmile, (Lm15Sgfnz07.Width - smileWidth) / 2, 50, smileWidth, smileHeight);

            Thread.Sleep(5000);

            lcd.Clear(white);
            
            byte x = 0;
            byte y = 0;
            var w = Lm15Sgfnz07.Width / (colors.Length + 1);
            var xx = 0;

            lcd.Clear(white);
            foreach (var color in colors)
            {
                lcd.FillRectangle(color, (int)Math.Round(xx), y, (int)Math.Round(w), 40);
                xx += w;
            }

            xx = 0;
            var colors2 = new ushort[]
                              {
                                  0x0F0F,
                                  0x0FF,
                                  0x0888,
                                  0x08F8,
                                  0x0F93,
                                  0x03F0
                              };

            foreach (var color in colors2)
            {
                lcd.FillRectangle(color, (int)Math.Round(xx), 40, (int)Math.Round(w), 40);
                xx += w;
            }

            Thread.Sleep(5000);

            lcd.Clear(white);
            x = 10;
            y = 10;
            foreach (var color in colors)
            {
                lcd.Text(".NET MF", x, y, color, white);
                x += 10;
                y += 10;
            }
            Thread.Sleep(5000);

            lcd.Clear(white);
            x = 10;
            y = 10;
            foreach (var color in colors)
            {
                lcd.Rectangle(color, x, y, 50, 15);
                x += 5;
                y += 7;
            }
            Thread.Sleep(5000);

            lcd.Clear(white);
            x = 10;
            y = 10;
            foreach (var color in colors)
            {
                lcd.FillRectangle(color, x, y, 50, 15);
                x += 5;
                y += 5;
            }
            Thread.Sleep(5000);

            lcd.Clear(white);
            lcd.Line(red, 0, 100, 0, 79);
            lcd.Line(blue, 100, 0, 0, 79);
            Thread.Sleep(5000);

            int width, height, depth;

            var resList = new[]
                              {
                                  Resources.BinaryResources.stm32f4disc,
                                  Resources.BinaryResources.rose,
                                  Resources.BinaryResources.dotnet
                              };
            foreach (Resources.BinaryResources resource in resList)
            {
                var resContent = Resources.GetBytes(resource);
                ushort[] resImg = Lm15Sgfnz07.PpmToImage(resContent, out width, out height, out depth);
                lcd.DrawImage(resImg, (Lm15Sgfnz07.Width - width)/2, (Lm15Sgfnz07.Width - width)/2, width, height);
                Thread.Sleep(5000);
            }

            var res = Resources.GetBytes(Resources.BinaryResources.smile);
            ushort[] spriteImg = Lm15Sgfnz07.PpmToImage(res, out width, out height, out depth);
            lcd.Clear(blue);

            var sprites = new[]
                              {
                                  new Sprite(lcd, spriteImg, width, height)
                                      {X = 50, Y = 10, Dx = -1, Dy = 1},
                                  new Sprite(lcd, spriteImg, width, height)
                                      {X = 5, Y = 37, Dx = 1, Dy = -1},
                                  new Sprite(lcd, spriteImg, width, height)
                                      {X = 67, Y = 50, Dx = -1, Dy = -1}
                              };

            lcd.Contrast(50);
            const string format = "hh:mm:ss";
            int posX = ((Lm15Sgfnz07.Width - lcd.MeasureTextWidth(DateTime.Now.ToString(format))) / 2);
            while (true)
            {
                string uptime = PowerState.Uptime.ToString();
                var msindex = uptime.LastIndexOf(".");
                lcd.Text("Uptime:", posX, 20, white, blue);
                lcd.Text(uptime.Substring(0, msindex), posX, 30, white, blue);

                foreach (Sprite sprite in sprites)
                    sprite.Update();

                Thread.Sleep(5);
            }
        }
    }
}
