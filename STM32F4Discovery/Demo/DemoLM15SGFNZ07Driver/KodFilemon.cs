using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;

namespace DemoLM15SGFNZ07Driver
{
    public class KodFilemon : Test
    {
        public KodFilemon(string comment)
            : base(comment)
        {
        }

        public override void Run()
        {
            Font font = Resources.GetFont(Resources.FontResources.ninabd18ppem);
            using (var bitmap = new Bitmap(Dimensions.Width, Dimensions.Height))
            {
                for (int i = 0; i < 4; i++)
                {
                    bitmap.Clear();
                    bitmap.Flush();
                    Thread.Sleep(1000);

                    bitmap.DrawTextInRect("kodFilemon\n.blogspot\n.com", 0, 10,
                                          Dimensions.Width, Dimensions.Height-10,
                                          Bitmap.DT_AlignmentCenter, Colors.White, font);
                    bitmap.Flush();
                    Thread.Sleep(1000);
                }
            }
        }
    }
}