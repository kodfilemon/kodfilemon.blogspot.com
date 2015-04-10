using System;
using System.Threading;
using Microsoft.SPOT;

namespace DemoLM15SGFNZ07Driver
{
    public class SlideShow : Test
    {
        public SlideShow(string comment) : base(comment) { }
        public override void Run()
        {
            try
            {
                using (var bmp = new Bitmap(Dimensions.Width, Dimensions.Height))
                {
                    var bitmaps = new[]
                                      {
                                          Resources.BitmapResources.cat2,
                                          Resources.BitmapResources.cat1,
                                          Resources.BitmapResources.cat3
                                      };

                    for (int i = 0; i < bitmaps.Length; i++)
                    {
                        using (Bitmap src = Resources.GetBitmap(bitmaps[i]))
                            bmp.DrawImage(0, 0, src, 0, 0, src.Width, src.Height);

                        bmp.Flush();

                        Thread.Sleep(2000);
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