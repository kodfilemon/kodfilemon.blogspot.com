using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;

namespace DemoLM15SGFNZ07Driver
{
    internal class DrawText : Test
    {
        public DrawText(string comment) : base(comment)
        {
        }

        public override void Run()
        {
            try
            {
                using (var bmp = new Bitmap(Dimensions.Width, Dimensions.Height))
                {
                    var fonts = new[]
                                    {
                                        new FontItem {Name = "Arial 10", FontId = Resources.FontResources.arial10},
                                        new FontItem {Name = "Small", FontId = Resources.FontResources.small},
                                        new FontItem {Name = "Courier 11", FontId = Resources.FontResources.courier11}
                                    };

                    const string s = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";

                    foreach (var font in fonts)
                    {
                        bmp.Clear();

                        Font fnt = Resources.GetFont(font.FontId);

                        bmp.DrawRectangle((Color)0xFF0000, 0, 0, 0,
                                          Dimensions.Width, Dimensions.Height,
                                          0, 0, (Color)0xFF0000, 0, 0, (Color)0xFF0000, 0, 0,
                                          Bitmap.OpacityOpaque);

                        const Color color = (Color)0x0000FF;
                        bmp.DrawTextInRect(font.Name + " " + s, 0, 0, Dimensions.Width, Dimensions.Height,
                                           Bitmap.DT_WordWrap | Bitmap.DT_AlignmentLeft, color, fnt);

                        bmp.Flush();

                        Thread.Sleep(5000);
                    }
                }
                Pass = true;
            }
            catch (Exception e)
            {
                UnexpectedException(e);
            }
        }

        internal class FontItem
        {
            public String Name { get; set; }
            public Resources.FontResources FontId { get; set; }
        }
    }
}