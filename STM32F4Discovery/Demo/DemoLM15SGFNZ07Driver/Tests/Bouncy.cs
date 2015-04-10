using System;
using System.Threading;
using Microsoft.SPOT;

namespace DemoLM15SGFNZ07Driver
{
    public class Bouncy : Test
    {
        public Bouncy(string comment) : base(comment) { }
        public override void Run()
        {
            try
            {
                using (var bmp = new Bitmap(Dimensions.Width, Dimensions.Height))
                {
                    //var resId = Resources.BitmapResources.smile;
                    var resId = Resources.BitmapResources.spotlogo110;
                    using (Bitmap src = Resources.GetBitmap(resId))
                    {
                        if(resId == Resources.BitmapResources.smile)
                            src.MakeTransparent(src.GetPixel(0,0));

                        var rand = new Random();

                        int xPos = rand.Next(Dimensions.Width - src.Width);
                        int yPos = rand.Next(Dimensions.Height - src.Height);

                        int xDir = 3;
                        int yDir = 4;

                        for (int animcount = 0; animcount < 100; animcount++)
                        {
                            bmp.Clear();
                            bmp.DrawImage(xPos, yPos, src, 0, 0, src.Width, src.Height);
                            bmp.Flush();

                            xPos = xPos + xDir;
                            yPos = yPos + yDir;

                            if (xPos + src.Width > bmp.Width || xPos < 0)
                            {
                                xDir = -xDir;
                                xPos += xDir;
                            }
                            if (yPos + src.Height > bmp.Height || yPos < 0)
                            {
                                yDir = -yDir;
                                yPos += yDir;
                            }

                            Thread.Sleep(10);
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