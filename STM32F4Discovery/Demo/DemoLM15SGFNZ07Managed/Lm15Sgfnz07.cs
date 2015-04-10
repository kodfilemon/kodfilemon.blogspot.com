using System;
using System.IO;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace DemoLM15SGFNZ07Managed
{
    public class Lm15Sgfnz07
    {
        public const int Width = 101;
        public const int Height = 80;
        public readonly Font5X7 DefaultFont = new Font5X7();

        private readonly SPI _spi;
        private readonly OutputPort _reset;
        private readonly OutputPort _rs;

        public Lm15Sgfnz07(SPI.SPI_module spi, Cpu.Pin cs, Cpu.Pin reset, Cpu.Pin rs)
        {
            var spiCfg = new SPI.Configuration(cs, false, 0, 0, true, true, 5000, spi);
            _spi = new SPI(spiCfg);
            _reset = new OutputPort(reset, true);
            _rs = new OutputPort(rs, false);

            Initialize();
        }

        private void SendCommand(params byte[] values)
        {
            _rs.Write(true);
            _spi.Write(values);
        }

        private void SendData(params ushort[] values)
        {
            _rs.Write(false);
            _spi.Write(values);
        }

        private void Initialize()
        {
            _reset.Write(false);
            Thread.Sleep(10);
            _reset.Write(true);

            SendCommand(0xF4, 0x90, 0xB3, 0xA0, 0xD0,
                        0xF0, 0xE2, 0xD4, 0x70, 0x66, 0xB2, 0xBA, 0xA1, 0xA3, 0xAB, 0x94, 0x95,
                        0x95, 0x95, 0xF5, 0x90, 0xF1, 0x00, 0x10, 0x22, 0x30, 0x45, 0x50, 0x68,
                        0x70, 0x8A, 0x90, 0xAC, 0xB0, 0xCE, 0xD0, 0xF2, 0x0F, 0x10, 0x20, 0x30,
                        0x43, 0x50, 0x66, 0x70, 0x89, 0x90, 0xAB, 0xB0, 0xCD, 0xD0, 0xF3, 0x0E,
                        0x10, 0x2F, 0x30, 0x40, 0x50, 0x64, 0x70, 0x87, 0x90, 0xAA, 0xB0, 0xCB,
                        0xD0, 0xF4, 0x0D, 0x10, 0x2E, 0x30, 0x4F, 0x50, 0xF5, 0x91, 0xF1, 0x01,
                        0x11, 0x22, 0x31, 0x43, 0x51, 0x64, 0x71, 0x86, 0x91, 0xA8, 0xB1, 0xCB,
                        0xD1, 0xF2, 0x0F, 0x11, 0x21, 0x31, 0x42, 0x51, 0x63, 0x71, 0x85, 0x91,
                        0xA6, 0xB1, 0xC8, 0xD1, 0xF3, 0x0B, 0x11, 0x2F, 0x31, 0x41, 0x51, 0x62,
                        0x71, 0x83, 0x91, 0xA4, 0xB1, 0xC6, 0xD1, 0xF4, 0x08, 0x11, 0x2B, 0x31,
                        0x4F, 0x51, 0x80, 0x94, 0xF5, 0xA2, 0xF4, 0x60, 0xF0, 0x40, 0x50, 0xC0,
                        0xF4, 0x70);

            Thread.Sleep(10);

            SendCommand(0xF0, 0x81,
                        0xF4, 0xB3, 0xA0,
                        0xF0, 0x06, 0x10, 0x20, 0x30,
                        0xF5, 0x0F, 0x1C, 0x2F, 0x34);
        }

        public void Contrast(byte contrast)
        {
            SendCommand(0xF4,
                        (byte) (0xB0 | (contrast >> 4)),
                        (byte) (0xA0 | (contrast & 0x0F)));
        }

        private void ViewPort(int x1 = 0, int y1 = 0, int x2 = Width - 1, int y2 = Height - 1)
        {
            x1 <<= 1;
            x1 += 6;
            x2 <<= 1;
            x2 += 7;

            SendCommand(0xf0,
                        (byte) (0x00 | (x1 & 0x0f)),
                        (byte) (0x10 | (x1 >> 4)),
                        (byte) (0x20 | (y1 & 0x0f)),
                        (byte) (0x30 | (y1 >> 4)),
                        0xf5,
                        (byte) (0x00 | (x2 & 0x0f)),
                        (byte) (0x10 | (x2 >> 4)),
                        (byte) (0x20 | (y2 & 0x0f)),
                        (byte) (0x30 | (y2 >> 4)));
        }

        public void SetPixel(ushort color, int x, int y)
        {
            ViewPort(x, y, x, y);
            SendData(color);
        }

        public void DrawImage(ushort[] image, int x, int y, int width, int height)
        {
            ViewPort(x, y, x + width - 1, y + height - 1);
            SendData(image);
        }

        public void FillRectangle(ushort color, int x, int y, int width, int height)
        {
            var buffer = new ushort[width*height];
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = color;

            DrawImage(buffer, x, y, width, height);
        }

        public void Clear(ushort color)
        {
            FillRectangle(color, 0, 0, Width, Height);
        }

        public void Line(ushort color, int x1, int x2, int y1, int y2)
        {
            short stepx, stepy, fraction;

            var dy = (short)(y2 - y1);
            var dx = (short)(x2 - x1);

            if (dy < 0)
            {
                dy = (short)(-dy);
                stepy = -1;
            }
            else
                stepy = 1;

            if (dx < 0)
            {
                dx = (short)(-dx);
                stepx = -1;
            }
            else
                stepx = 1;

            dx <<= 1;
            dy <<= 1;

            SetPixel(color, x1, y1);

            if (dx > dy)
            {
                fraction = (short)(dy - (dx >> 1));
                while (x1 != x2)
                {
                    if (fraction >= 0)
                    {
                        y1 = (byte)(y1 + stepy);
                        fraction -= dx;
                    }
                    x1 = (byte)(x1 + stepx);
                    fraction += dy;

                    SetPixel(color, x1, y1);
                }
            }
            else
            {
                fraction = (short)(dx - (dy >> 1));
                while (y1 != y2)
                {
                    if (fraction >= 0)
                    {
                        x1 = (byte)(x1 + stepx);
                        fraction -= dy;
                    }
                    y1 = (byte)(y1 + stepy);
                    fraction += dx;

                    SetPixel(color, x1, y1);
                }
            }
        }

        public void Rectangle(ushort color, int x, int y, int width, int height, int thickness = 1)
        {
            var x2 = (byte)(x + width);
            var y2 = (byte)(y + height);

            for (int i = 1; i <= thickness; i++)
            {
                Line(color, x, x, y, y2);
                Line(color, x2, x2, y, y2);
                Line(color, x, x2, y, y);
                Line(color, x, x2, y2, y2);

                x += 1;
                y += 1;
                x2 -= 1;
                y2 -= 1;
            }
        }

        private void Char(char value, ILm15Sgfnz07Font font, int x, int y, ushort foreColor, ushort backColor)
        {
            ushort[] img = font.GetImage(value, foreColor, backColor);
            DrawImage(img, x, y, font.Width, font.Height);
        }

        public void Text(string text, int x, int y, ushort foreColor, ushort backColor, ILm15Sgfnz07Font font = null)
        {
            ILm15Sgfnz07Font destFont = font ?? DefaultFont;
            for (int i = 0; i < text.Length; i++)
            {
                char chr = text[i];
                if (chr != '\n' && chr != '\r')
                    Char(chr, destFont, x, y, foreColor, backColor);

                x = (byte)(x + destFont.Width + 1);
                if (x > Width || chr == '\n')
                {
                    y = (byte)(y + destFont.Height + 1);
                    x = 0;
                }
            }
        }

        public int MeasureTextWidth(string text, ILm15Sgfnz07Font font = null)
        {
            ILm15Sgfnz07Font destFont = font ?? DefaultFont;
            int result = text.Length * (destFont.Width + 1);
            return result;
        }

        public static ushort[] PpmToImage(byte[] bytes, out int width, out int height, out int depth)
        {
            ushort[] result;

            using (Stream stream = new MemoryStream(bytes))
            {
                using (var reader = new BinaryReader(stream))
                {
                    if (reader.ReadChar() != 'P' || reader.ReadChar() != '6')
                        throw new InvalidOperationException("Invalid PPM");

                    reader.Read();

                    var nextChar = reader.Peek();
                    if (nextChar == '#')
                    {
                        while (nextChar != '\n')
                            nextChar = reader.Read();

                        nextChar = reader.Peek();
                        if (IsWhitespace(nextChar))
                            reader.Read();
                    }

                    width = ReadValue(reader);
                    height = ReadValue(reader);
                    depth = ReadValue(reader);

                    result = new ushort[width * height];

                    switch (depth)
                    {
                        case 15:
                            for (int i = 0; i < result.Length; i++)
                                result[i] = (ushort)(reader.Read() << 8
                                                     | reader.Read() << 4
                                                     | reader.Read());
                            break;

                        case 255:
                            for (int i = 0; i < result.Length; i++)
                                result[i] = (ushort)((reader.Read() & 0xF0) << 4
                                                     | (reader.Read() & 0xF0)
                                                     | (reader.Read() & 0xF0) >> 4);
                            break;

                        default:
                            throw new InvalidOperationException("Invalid ppm depth " + depth);
                    }
                }
            }

            return result;
        }

        private static bool IsWhitespace(int value)
        {
            return value == '\n' || value == 'r' || value == ' ' || value == '\t';
        }

        private static int ReadValue(BinaryReader reader)
        {
            string value = String.Empty;
            while (!IsWhitespace(reader.Peek()))
                value += reader.ReadChar();

            reader.Read();
            return Int32.Parse(value);
        }
    }
}
