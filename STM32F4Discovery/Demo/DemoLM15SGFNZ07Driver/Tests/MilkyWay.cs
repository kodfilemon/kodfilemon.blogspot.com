using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;

namespace DemoLM15SGFNZ07Driver
{
    internal class MilkyWay : Test
    {
        private readonly Star[] _stars;

        private class Star
        {
            private readonly Random _random;

            public Star(Random random)
            {
                _random = random;
                RandomPosition();
            }

            private void RandomPosition()
            {
                X = _random.Next(100) - 50;
                Y = _random.Next(100) - 50;
                Z = _random.Next(50) + 1;
            }

            public int X { get; private set; }
            public int Y { get; private set; }
            public int Z { get; private set; }

            public void Fly()
            {
                Z -= 1;
                if (Z <= 0)
                    RandomPosition();
            }
        }

        public MilkyWay(string comment) : base(comment)
        {
            var rnd = new Random();
            _stars = new Star[30];

            for (int i = 0; i < _stars.Length; i++)
                _stars[i] = new Star(rnd);
        }

        public override void Run()
        {
            int focus = 15;
            
            using (var bmp = new Bitmap(Dimensions.Width, Dimensions.Height))
            {
                DateTime barier = DateTime.Now.AddSeconds(5);
                while(DateTime.Now < barier)
                {
                    bmp.Clear();

                    foreach (var star in _stars)
                    {
                        int x = star.X*focus/star.Z + Dimensions.Width/2;
                        int y = Dimensions.Height/2 - star.Y*focus/star.Z;

                        if (x >= 0 && y >= 0 && x <= bmp.Width && y <= bmp.Height)
                        {
                            if (star.Z > 20)
                                bmp.SetPixel(x, y, Color.White);
                            else
                            {
                                bmp.SetPixel(x, y, Color.White);
                                bmp.SetPixel(x - 1, y, Color.White);
                                bmp.SetPixel(x + 1, y, Color.White);
                                bmp.SetPixel(x, y - 1, Color.White);
                                bmp.SetPixel(x, y + 1, Color.White);
                            }
                        }

                        star.Fly();
                    }

                    bmp.Flush();
                    Thread.Sleep(5);
                }
            }
        }
    }
}