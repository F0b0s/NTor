using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Protocol
{
    public abstract class Command
    {
        public abstract byte[] ToArray();
    }
}
