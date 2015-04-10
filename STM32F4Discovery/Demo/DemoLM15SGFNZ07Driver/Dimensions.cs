using Microsoft.SPOT.Presentation;

namespace DemoLM15SGFNZ07Driver
{
    public class Dimensions
    {
        public static int Width;
        public static int Height;

        static Dimensions()
        {

            Width = SystemMetrics.ScreenWidth;
            Height = SystemMetrics.ScreenHeight;
        }
    }
}