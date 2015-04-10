namespace DemoLM15SGFNZ07Managed
{
    public interface ILm15Sgfnz07Font
    {
        int Width { get; }
        int Height { get; }
        ushort[] GetImage(char value, ushort foreColor, ushort backColor);
    }
}