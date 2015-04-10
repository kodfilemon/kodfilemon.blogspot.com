using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;

namespace DemoLM15SGFNZ07Driver
{
    public class RandomDrawRectangle : Test
    {
        public RandomDrawRectangle(string comment) : base(comment) { }
        public override void Run()
        {
            try
            {
                using (var bmp = new Bitmap(Dimensions.Width, Dimensions.Height))
                {
                    var rand = new Random();

                    for (int i = 0; i < 100; i++)
                    {
                        var fillColor = (Color)rand.Next(0xFFFFFF);
                        bmp.DrawRectangle((Color) rand.Next(0xFFFFFF), rand.Next(1),
                                          rand.Next(Dimensions.Width), rand.Next(Dimensions.Height),
                                          rand.Next(Dimensions.Width), rand.Next(Dimensions.Height),
                                          0, 0, fillColor, 0, 0, fillColor, 0, 0, (ushort) rand.Next(256));
                        bmp.Flush();
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