namespace DemoLM15SGFNZ07Managed
{
    internal class Sprite
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Dx { get; set; }
        public int Dy { get; set; }

        private readonly int _imageWidth;
        private readonly int _imageHeight;
        private readonly ushort[] _image;
        private readonly Lm15Sgfnz07 _lcd;

        public Sprite(Lm15Sgfnz07 lcd, ushort[] image, int imageWidth, int imageHeight)
        {
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;
            _image = image;
            _lcd = lcd;
            Dx = 1;
            Dy = 1;
        }

        public void Update()
        {
            int newx = X + Dx;
            if (newx < 0 || newx + _imageWidth > Lm15Sgfnz07.Width)
                Dx = -Dx;
            else
                X = newx;

            int newy = Y + Dy;
            if (newy < 0 || newy + _imageHeight > Lm15Sgfnz07.Height)
                Dy = -Dy;
            else
                Y = newy;

            _lcd.DrawImage(_image, X, Y, _imageWidth, _imageHeight);
        }
    }
}