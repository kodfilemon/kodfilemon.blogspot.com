using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;

namespace DemoLM15SGFNZ07Driver
{
    public class Rotate : Test
    {
        public Rotate(string comment)
            : base(comment)
        {
        }

        public override void Run()
        {
            using (var bitmap = new Bitmap(Dimensions.Width, Dimensions.Height))
            {
                using (var img = Resources.GetBitmap(Resources.BitmapResources.compass2))
                {
                    int da = 3;
                    for (int angle = 0; angle <= 360 && angle >= 0; angle += da)
                    {
                        bitmap.DrawRectangle(Colors.White, 0, 0, 0,
                                             Dimensions.Width, Dimensions.Height,
                                             0, 0, Colors.White, 0, 0, Colors.White, 0, 0,
                                             Bitmap.OpacityOpaque);

                        int xdst = (Dimensions.Width - img.Width)/2,
                            ydst = (Dimensions.Height - img.Height)/2;
                        bitmap.RotateImage(angle, xdst, ydst, img, 0, 0, img.Width, img.Height, 0x00);
                        
                        bitmap.Flush();

                        if (angle > 180)
                            da = -da;
                    }
                }
            }
        }
    }
}