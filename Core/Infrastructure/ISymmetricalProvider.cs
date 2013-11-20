using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Infrastructure
{
    public interface ISymmetricalProvider
    {
        byte[] GenerateKey(int len);
        byte[] AssymEncrypt(byte[] data);
        byte[] SymmEncrypt(byte[] data, byte[] key, byte[] iv);
    }
}
