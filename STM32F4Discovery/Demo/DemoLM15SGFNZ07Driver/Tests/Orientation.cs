using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;

namespace DemoLM15SGFNZ07Driver
{
    public class Orientation : Test
    {
        public Orientation(string comment) : base(comment) { }

        public override void Run()
        {
            try
            {
                using (var bmp = new Bitmap(Dimensions.Width, Dimensions.Height))
                {
                    bmp.DrawLine(Colors.Red, 1,
                                 Dimensions.Width/2, Dimensions.Height/2,
                                 Dimensions.Width, Dimensions.Height/2);

                    bmp.DrawLine(Colors.Green, 1,
                                 Dimensions.Width/2, Dimensions.Height/2,
                                 Dimensions.Width/2, 0);

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