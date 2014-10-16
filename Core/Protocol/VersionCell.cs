namespace Core.Protocol
{
    public class VersionCell : Cell
    {
        public VersionCell() : base(1)
        {
        }

        protected override byte[] GetPayload()
        {
            return new byte[2]{0x00, 0x03};
        }
    }
}
