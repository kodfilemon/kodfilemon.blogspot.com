using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;

namespace DemoLM15SGFNZ07Driver
{
    public class RandomSetPixel : Test
    {
        public RandomSetPixel(string comment) : base(comment) { }
        public override void Run()
        {
            try
            {
                using (var bmp = new Bitmap(Dimensions.Width, Dimensions.Height))
                {
                    var rand = new Random();
                    for (int i = 0; i < 100; i++)
                    {
                        bmp.SetPixel(rand.Next(Dimensions.Width),
                                     rand.Next(Dimensions.Height),
                                     (Color) rand.Next(0xFFFFFF));
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