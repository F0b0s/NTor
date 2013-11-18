using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Protocol
{
    public abstract class Cell
    {
        public abstract byte[] ToArray();
    }

    public class VersionCell : Cell
    {
        public override byte[] ToArray() {
            return new byte[]{0x00, 0x00, 0x07, 0x00, 0x02, 0x00, 0x02};
        }
    }

    public class NetInfoCell : Cell
    {
        public override byte[] ToArray() {
            return new byte[]{0x00, 0x00, 0x08,     0x00, 0x00, 0x00, 0x00   };
        }
    }
}
