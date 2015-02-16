using System;
using System.Collections.Generic;

namespace Core.Protocol
{
    public class VersionsCell : Cell
    {
        private readonly IEnumerable<ushort> _versions;

        public VersionsCell(IEnumerable<ushort> versions) : base(7)
        {
            if (versions == null) throw new ArgumentNullException("versions");

            _versions = versions;
        }

        protected override byte[] GetPayload()
        {
            var payload = new byte[0];
            foreach (var version in _versions)
            {
                var currentSize = payload.Length;
                Array.Resize(ref payload, currentSize + 2);

                Array.Copy(BitConverter.GetBytes(version), 0, payload, currentSize, 2);
            }

            return payload;
        }

        public IEnumerable<ushort> Versions
        {
            get { return _versions; }
        }

        public static VersionsCell Parse(byte[] array)
        {
            var versions = new List<ushort>();
            for (var i = 0; i < array.Length; i+=2)
            {
                versions.Add(BitConverter.ToUInt16(array, i));
            }

            return new VersionsCell(versions);
        }
    }
}
