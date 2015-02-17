namespace Core.Utils
{
    public static class ByteHelper
    {
        public static short ReadInt16(byte first, byte second)
        {
            short value = 0;
            value = (short)((value | first) << 8);
            value |= second;

            return value;
        }
    }
}
