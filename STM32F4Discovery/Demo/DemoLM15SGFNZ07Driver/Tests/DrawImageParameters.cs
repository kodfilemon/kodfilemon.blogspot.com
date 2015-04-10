using Microsoft.SPOT;

namespace DemoLM15SGFNZ07Driver
{
    public class DrawImageParameters : Test
    {
        public DrawImageParameters(string comment) : base(comment) { }
        public override void Run()
        {
            try
            {
                using (var bmp = new Bitmap(20, 20))
                    bmp.DrawImage(0, 0, bmp, 0, 0, bmp.Width, bmp.Height);
            }
            catch
            {
                Pass = true;
            }
        }
    }
}