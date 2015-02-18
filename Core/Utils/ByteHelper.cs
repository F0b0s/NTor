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

        public static int ReadInt32(byte first, byte second, byte third, byte forth)
        {
            int value = 0;
            value = (short)((value | first) << 24);
            value = (short)((value | second) << 16);
            value = (short)((value | third) << 8);
            value |= forth;

            return value;
        }
    }
}
