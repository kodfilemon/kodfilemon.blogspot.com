using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;

namespace DemoLM15SGFNZ07Driver
{
    public class GreenBlueX : Test
    {
        public GreenBlueX(string comment) : base(comment) { }
        public override void Run()
        {
            try
            {
                using (var bmp = new Bitmap(Dimensions.Width, Dimensions.Height))
                {
                    int size = System.Math.Min(Dimensions.Width, Dimensions.Height);

                    for (int i = 0; i < size; ++i)
                        bmp.SetPixel(i, i, (Color)((255 - i) << 16));

                    for (int i = 0; i < size; i += 2)
                        bmp.SetPixel(size - i, i, (Color)(i << 8));

                    bmp.Flush();
                }

                Thread.Sleep(3000);
                Pass = true;
            }
            catch (Exception e)
            {
                UnexpectedException(e);
            }
        }
    }
}