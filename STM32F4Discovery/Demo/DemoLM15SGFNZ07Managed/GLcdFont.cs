namespace DemoLM15SGFNZ07Managed
{
    public abstract class GLcdFont : ILm15Sgfnz07Font
    {
        protected abstract byte[][] Data { get; }

        public abstract int Width { get; }
        public abstract int Height { get; }

        public ushort[] GetImage(char value, ushort foreColor, ushort backColor)
        {
            byte[] data = Data[value - 32];

            var result = new ushort[Width * Height];
            for (int i = 0; i < Width; i++)
            {
                byte line = data[i];
                for (int j = 0, mask = 1; j < Height; j++, mask <<= 1)
                {
                    int index = Width * j + i;
                    result[index] = (line & mask) > 0 ? foreColor : backColor;
                }
            }

            return result;
        }
    }
}