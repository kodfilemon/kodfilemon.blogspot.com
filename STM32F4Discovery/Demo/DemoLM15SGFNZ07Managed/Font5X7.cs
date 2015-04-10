namespace DemoLM15SGFNZ07Managed
{
    public class Font5X7 : GLcdFont
    {
        private readonly byte[][] _data = new[]
                                              {
                                                  new byte[] {0x00, 0x00, 0x00, 0x00, 0x00}, // sp
                                                  new byte[] {0x00, 0x00, 0x2f, 0x00, 0x00}, // !
                                                  new byte[] {0x00, 0x07, 0x00, 0x07, 0x00}, // "
                                                  new byte[] {0x14, 0x7f, 0x14, 0x7f, 0x14}, // #
                                                  new byte[] {0x24, 0x2a, 0x7f, 0x2a, 0x12}, // $
                                                  new byte[] {0x32, 0x34, 0x08, 0x16, 0x26}, // %
                                                  new byte[] {0x36, 0x49, 0x55, 0x22, 0x50}, // &
                                                  new byte[] {0x00, 0x05, 0x03, 0x00, 0x00}, // '
                                                  new byte[] {0x00, 0x1c, 0x22, 0x41, 0x00}, // (
                                                  new byte[] {0x00, 0x41, 0x22, 0x1c, 0x00}, // )
                                                  new byte[] {0x14, 0x08, 0x3E, 0x08, 0x14}, // *
                                                  new byte[] {0x08, 0x08, 0x3E, 0x08, 0x08}, // +
                                                  new byte[] {0x00, 0x00, 0x50, 0x30, 0x00}, // ,
                                                  new byte[] {0x10, 0x10, 0x10, 0x10, 0x10}, // -
                                                  new byte[] {0x00, 0x60, 0x60, 0x00, 0x00}, // .
                                                  new byte[] {0x20, 0x10, 0x08, 0x04, 0x02}, // /
                                                  new byte[] {0x3E, 0x51, 0x49, 0x45, 0x3E}, // 0
                                                  new byte[] {0x00, 0x42, 0x7F, 0x40, 0x00}, // 1
                                                  new byte[] {0x42, 0x61, 0x51, 0x49, 0x46}, // 2
                                                  new byte[] {0x21, 0x41, 0x45, 0x4B, 0x31}, // 3
                                                  new byte[] {0x18, 0x14, 0x12, 0x7F, 0x10}, // 4
                                                  new byte[] {0x27, 0x45, 0x45, 0x45, 0x39}, // 5
                                                  new byte[] {0x3C, 0x4A, 0x49, 0x49, 0x30}, // 6
                                                  new byte[] {0x01, 0x71, 0x09, 0x05, 0x03}, // 7
                                                  new byte[] {0x36, 0x49, 0x49, 0x49, 0x36}, // 8
                                                  new byte[] {0x06, 0x49, 0x49, 0x29, 0x1E}, // 9
                                                  new byte[] {0x00, 0x36, 0x36, 0x00, 0x00}, // :
                                                  new byte[] {0x00, 0x56, 0x36, 0x00, 0x00}, // ;
                                                  new byte[] {0x08, 0x14, 0x22, 0x41, 0x00}, // <
                                                  new byte[] {0x14, 0x14, 0x14, 0x14, 0x14}, // =
                                                  new byte[] {0x00, 0x41, 0x22, 0x14, 0x08}, // >
                                                  new byte[] {0x02, 0x01, 0x51, 0x09, 0x06}, // ?
                                                  new byte[] {0x32, 0x49, 0x59, 0x51, 0x3E}, // @
                                                  new byte[] {0x7E, 0x11, 0x11, 0x11, 0x7E}, // A
                                                  new byte[] {0x7F, 0x49, 0x49, 0x49, 0x36}, // B
                                                  new byte[] {0x3E, 0x41, 0x41, 0x41, 0x22}, // C
                                                  new byte[] {0x7F, 0x41, 0x41, 0x22, 0x1C}, // D
                                                  new byte[] {0x7F, 0x49, 0x49, 0x49, 0x41}, // E
                                                  new byte[] {0x7F, 0x09, 0x09, 0x09, 0x01}, // F
                                                  new byte[] {0x3E, 0x41, 0x49, 0x49, 0x7A}, // G
                                                  new byte[] {0x7F, 0x08, 0x08, 0x08, 0x7F}, // H
                                                  new byte[] {0x00, 0x41, 0x7F, 0x41, 0x00}, // I
                                                  new byte[] {0x20, 0x40, 0x41, 0x3F, 0x01}, // J
                                                  new byte[] {0x7F, 0x08, 0x14, 0x22, 0x41}, // K
                                                  new byte[] {0x7F, 0x40, 0x40, 0x40, 0x40}, // L
                                                  new byte[] {0x7F, 0x02, 0x0C, 0x02, 0x7F}, // M
                                                  new byte[] {0x7F, 0x04, 0x08, 0x10, 0x7F}, // N
                                                  new byte[] {0x3E, 0x41, 0x41, 0x41, 0x3E}, // O
                                                  new byte[] {0x7F, 0x09, 0x09, 0x09, 0x06}, // P
                                                  new byte[] {0x3E, 0x41, 0x51, 0x21, 0x5E}, // Q
                                                  new byte[] {0x7F, 0x09, 0x19, 0x29, 0x46}, // R
                                                  new byte[] {0x46, 0x49, 0x49, 0x49, 0x31}, // S
                                                  new byte[] {0x01, 0x01, 0x7F, 0x01, 0x01}, // T
                                                  new byte[] {0x3F, 0x40, 0x40, 0x40, 0x3F}, // U
                                                  new byte[] {0x1F, 0x20, 0x40, 0x20, 0x1F}, // V
                                                  new byte[] {0x3F, 0x40, 0x38, 0x40, 0x3F}, // W
                                                  new byte[] {0x63, 0x14, 0x08, 0x14, 0x63}, // X
                                                  new byte[] {0x07, 0x08, 0x70, 0x08, 0x07}, // Y
                                                  new byte[] {0x61, 0x51, 0x49, 0x45, 0x43}, // Z
                                                  new byte[] {0x00, 0x7F, 0x41, 0x41, 0x00}, // [
                                                  new byte[] {0x55, 0x2A, 0x55, 0x2A, 0x55}, // 55
                                                  new byte[] {0x00, 0x41, 0x41, 0x7F, 0x00}, // ]
                                                  new byte[] {0x04, 0x02, 0x01, 0x02, 0x04}, // ^
                                                  new byte[] {0x40, 0x40, 0x40, 0x40, 0x40}, // _
                                                  new byte[] {0x00, 0x01, 0x02, 0x04, 0x00}, // '
                                                  new byte[] {0x20, 0x54, 0x54, 0x54, 0x78}, // a
                                                  new byte[] {0x7F, 0x48, 0x44, 0x44, 0x38}, // b
                                                  new byte[] {0x38, 0x44, 0x44, 0x44, 0x20}, // c
                                                  new byte[] {0x38, 0x44, 0x44, 0x48, 0x7F}, // d
                                                  new byte[] {0x38, 0x54, 0x54, 0x54, 0x18}, // e
                                                  new byte[] {0x08, 0x7E, 0x09, 0x01, 0x02}, // f
                                                  new byte[] {0x0C, 0x52, 0x52, 0x52, 0x3E}, // g
                                                  new byte[] {0x7F, 0x08, 0x04, 0x04, 0x78}, // h
                                                  new byte[] {0x00, 0x44, 0x7D, 0x40, 0x00}, // i
                                                  new byte[] {0x20, 0x40, 0x44, 0x3D, 0x00}, // j
                                                  new byte[] {0x7F, 0x10, 0x28, 0x44, 0x00}, // k
                                                  new byte[] {0x00, 0x41, 0x7F, 0x40, 0x00}, // l
                                                  new byte[] {0x7C, 0x04, 0x18, 0x04, 0x78}, // m
                                                  new byte[] {0x7C, 0x08, 0x04, 0x04, 0x78}, // n
                                                  new byte[] {0x38, 0x44, 0x44, 0x44, 0x38}, // o
                                                  new byte[] {0x7C, 0x14, 0x14, 0x14, 0x08}, // p
                                                  new byte[] {0x08, 0x14, 0x14, 0x18, 0x7C}, // q
                                                  new byte[] {0x7C, 0x08, 0x04, 0x04, 0x08}, // r
                                                  new byte[] {0x48, 0x54, 0x54, 0x54, 0x20}, // s
                                                  new byte[] {0x04, 0x3F, 0x44, 0x40, 0x20}, // t
                                                  new byte[] {0x3C, 0x40, 0x40, 0x20, 0x7C}, // u
                                                  new byte[] {0x1C, 0x20, 0x40, 0x20, 0x1C}, // v
                                                  new byte[] {0x3C, 0x40, 0x30, 0x40, 0x3C}, // w
                                                  new byte[] {0x44, 0x28, 0x10, 0x28, 0x44}, // x
                                                  new byte[] {0x0C, 0x50, 0x50, 0x50, 0x3C}, // y
                                                  new byte[] {0x44, 0x64, 0x54, 0x4C, 0x44} // z
                                              };

        protected override byte[][] Data
        {
            get { return _data; }
        }

        public override int Width
        {
            get { return 5; }
        }

        public override int Height
        {
            get { return 7; }
        }
    }
}