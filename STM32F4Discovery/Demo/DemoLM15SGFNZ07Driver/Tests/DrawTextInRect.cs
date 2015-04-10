using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;

namespace DemoLM15SGFNZ07Driver
{
    public class DrawTextInRect : Test
    {
        public DrawTextInRect(string comment) : base(comment) { }
        public override void Run()
        {
            try
            {
                Font font = Resources.GetFont(Resources.FontResources.small);
                using (var bmp = new Bitmap(Dimensions.Width, Dimensions.Height))
                {
                    uint[] flags = { 
                                       Bitmap.DT_WordWrap | Bitmap.DT_AlignmentLeft,
                                       Bitmap.DT_WordWrap | Bitmap.DT_AlignmentCenter,
                                       Bitmap.DT_WordWrap | Bitmap.DT_AlignmentRight
                                   };


                    const string s = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";

                    for (int i = 0; i < flags.Length; i++)
                    {
                        bmp.DrawRectangle((Color) 0xFF0000, 0, 0, 0,
                                          Dimensions.Width, Dimensions.Height,
                                          0, 0, (Color) 0xFF0000, 0, 0, (Color) 0xFF0000, 0, 0,
                                          Bitmap.OpacityOpaque);

                        const Color color = (Color)0x0000FF;
                        bmp.DrawTextInRect(s, 0, 0, Dimensions.Width, Dimensions.Height,
                                           flags[i], color, font);

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