using System;
using System.Threading;
using Microsoft.SPOT;

namespace DemoLM15SGFNZ07Driver
{
    public class JpegArray : Test
    {
        public JpegArray(string comment) : base(comment) { }

        public override void Run()
        {
            using (var bitmap = new Bitmap(Dimensions.Width, Dimensions.Height))
            {
                try
                {
                    bitmap.Clear();

                    using (Bitmap bmp = Resources.GetBitmap(Resources.BitmapResources.Jpeg01Normal))
                        bitmap.DrawImage(0, 0, bmp, 0, 0, bmp.Width, bmp.Height);

                    bitmap.Flush();
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
}