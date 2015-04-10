using System;
using System.Threading;
using Microsoft.SPOT;

namespace DemoLM15SGFNZ07Driver
{
    public class StretchImage : Test
    {
        public StretchImage(string comment) : base(comment) { }
        public override void Run()
        {
            try
            {
                using (var bmp = new Bitmap(Dimensions.Width, Dimensions.Height))
                {
                    var bitmaps = new[]
                                      {
                                          Resources.BitmapResources.out0,
                                          Resources.BitmapResources.out1,
                                          Resources.BitmapResources.out2
                                      };

                    for (int i = 0; i < bitmaps.Length; i++)
                    {
                        using (Bitmap src = Resources.GetBitmap(bitmaps[i]))
                        {
                            for (int s = 0; ; s += 3)
                            {
                                int w = s * Dimensions.Width / 200;
                                int h = s * Dimensions.Height / 200;

                                if (w > (Dimensions.Width) && h > (Dimensions.Height))
                                    break;

                                int x = (Dimensions.Width - w) / 2;
                                int y = (Dimensions.Height - h) / 2;

                                bmp.StretchImage(x, y, src, w, h, 256);
                                bmp.Flush();

                                
                            }
                        }

                        Thread.Sleep(1000);
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