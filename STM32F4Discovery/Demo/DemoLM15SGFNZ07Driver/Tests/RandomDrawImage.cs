using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;

namespace DemoLM15SGFNZ07Driver
{
    public class RandomDrawImage : Test
    {
        public RandomDrawImage(string comment) : base(comment) { }
        public override void Run()
        {
            try
            {
                using (var scratch = new Bitmap(64, 32))
                {
                    for (int x = 0; x < scratch.Width; x++)
                    {
                        for (int y = 0; y < scratch.Height; y++)
                        {
                            Color color = ColorUtility.ColorFromRGB((byte) (x*255/scratch.Width),
                                                                    (byte) (y*255/(scratch.Height)),
                                                                    (byte) (255 - x*255/scratch.Width));
                            scratch.SetPixel(x, y, color);
                        }
                    }

                    using (var bmp = new Bitmap(Dimensions.Width, Dimensions.Height))
                    {
                        var rand = new Random();

                        for (int i = 0; i < 100; i++)
                        {
                            bmp.DrawImage(rand.Next(Dimensions.Width),
                                          rand.Next(Dimensions.Height),
                                          scratch, 0, 0,
                                          scratch.Width, scratch.Height);
                            bmp.Flush();
                        }
                    }
                }
                Pass = true;
            }
            catch (Exception e)
            {
                UnexpectedException(e);
            }
        }
    }
}