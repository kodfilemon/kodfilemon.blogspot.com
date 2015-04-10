using System;

namespace DemoLM15SGFNZ07Driver
{
    public class Program
    {
        public static void Main()
        {
            var suite = new TestSuite();
            
again:
            suite.RunTest(new KodFilemon("Verify blink text"));

            suite.RunTest(new GifArray("Verify use of gif array results in display of eiffle tower"));
            suite.RunTest(new JpegArray("Verify use of jpeg array results in display of waterfall"));
            suite.RunTest(new Orientation("Verify red is right, and green is up"));
            //suite.RunTest(new GreenBlueX("Verify green / blue X"));
            suite.RunTest(new DrawTextTest("Verify Hello World appears in Text"));
            suite.RunTest(new RandomSetPixel("Verify Random SetPixel() calls"));
            suite.RunTest(new RandomDrawLine("Verify Random DrawLine() calls with thickness = 1"));
            suite.RunTest(new RandomDrawCircle("Verify Random DrawEllipse() calls: basic circles, thinkness = 1, no fill"));
            suite.RunTest(new RandomDrawRectangle("Verify Random DrawRectangle() calls: basic rectangles, thinkness = 1"));
            suite.RunTest(new RandomDrawImage("Verify Random DrawImage() calls: gradient image, constant size, entire image. dark blue in upper left"));
            suite.RunTest(new DrawImageParameters("Verify drawimage parameter correctly throws exception on different src/dst parameters"));
            suite.RunTest(new StretchImage("Verify images stretched equidistant across screen"));
            suite.RunTest(new SlideShow("Verify 3 bmps blit ed over one another"));
            suite.RunTest(new Bouncy("Verify bmp bounced within screen"));
            suite.RunTest(new Rotate("Rotate rose"));
            suite.RunTest(new MilkyWay("Star flight animation"));
            suite.RunTest(new DrawTextInRect("Verify left, center and right justified text"));
            suite.RunTest(new DrawText("Verify text type"));

            GC.WaitForPendingFinalizers();

            suite.Finished();

            goto again;
        }
    }
}