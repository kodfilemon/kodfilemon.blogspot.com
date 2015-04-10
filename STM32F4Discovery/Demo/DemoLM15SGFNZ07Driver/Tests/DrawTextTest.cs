using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;

namespace DemoLM15SGFNZ07Driver
{
    public class DrawTextTest : Test
    {
        public DrawTextTest(string comment) : base(comment) { }
        public override void Run()
        {
            try
            {
                using (var bmp = new Bitmap(Dimensions.Width, Dimensions.Height))
                {
                    const string text = "Hello World";
                    int textWidth, textHeight;

                    Font font = Resources.GetFont(Resources.FontResources.ninabd18ppem);
                    font.ComputeExtent(text, out textWidth, out textHeight);
                    int textX = (Dimensions.Width - textWidth)/2;

                    var rand = new Random();

                    for (int x = 0; x < 3; x++)
                    {
                        int baseColor = rand.Next(0xffff);
                        for (int i = 0; i < Dimensions.Height; i++)
                        {
                            bmp.Clear();
                            bmp.DrawText(text, font, (Color) (((255 - i) << 16) | baseColor), textX, i);
                            bmp.Flush();
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