namespace Common
{
    public static class ByteExtension
    {
        // ReSharper disable InconsistentNaming
        public static byte FromBCD(this byte @this) // ReSharper restore InconsistentNaming
        {
            var result = (byte) (((@this & 0xf0) >> 4)*10 + (@this & 0x0f));
            return result;
        }

        // ReSharper disable InconsistentNaming
        public static byte ToBCD(this byte @this) // ReSharper restore InconsistentNaming
        {
            return (byte) (@this/10 << 4 | @this%10);
        }
    }
}
