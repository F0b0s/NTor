using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Core.Infrastructure;

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

    public class CreateCell : Cell
    {
        private static string DH_moduls_base64 = @"//////////+BU+bsUWYoSeYfS3wRJJ+upZ+JWvtrOO7ttwb0tlz/C2vtN6bpQkz0xn5eYna1heRFwlFtbTXhTzcUX/JtCiswG0M6zbMZle/dBDSOeQhKUSKbEzumvgsCdMxnighOAinRHNyAi2LGxDTCaCGi2g/J//////////8A";
        public static BigInteger DH_modulus_p = new BigInteger(Convert.FromBase64String(DH_moduls_base64));
        private byte[] symmKey;

        private readonly ISymmetricalProvider symmetricalProvider;

        public CreateCell(ISymmetricalProvider symmetricalProvider)
        {
            this.symmetricalProvider = symmetricalProvider;
            symmKey = symmetricalProvider.GenerateKey(16);
        }

        public override byte[] ToArray()
        {
            byte[] data = new byte[144];
            var symmetricKey = symmetricalProvider.GenerateKey(16);

            var dh_g = new BigInteger(2);
            var dh_private =
                new BigInteger(symmetricalProvider.GenerateKey(Convert.FromBase64String(DH_moduls_base64).Length - 1));
            var dh_x = BigInteger.ModPow(dh_g, dh_private, DH_modulus_p);
            var dh_x_bytes = dh_x.ToByteArray();

            Array.Copy(symmetricKey, 0, data, 0, 16);
            Array.Copy(dh_x_bytes, 0, data, 16, 128);

            var part1ToEncrypth = new byte[128];
            Array.Copy(data, 0, part1ToEncrypth, 0, 128);

            var part1 = symmetricalProvider.AssymEncrypt(part1ToEncrypth);

            var part2ToEncrypt = new byte[16];
            Array.Copy(data, 128, part2ToEncrypt, 0, 16);
            var part2 = symmetricalProvider.SymmEncrypt(part2ToEncrypt, symmetricKey, new byte[16]);

            var bytes = new byte[part1.Length + part2.Length];

            Array.Copy(part1, 0, bytes, 0, part1.Length);
            Array.Copy(part2, 0, bytes, part1.Length, part2.Length);

            return bytes;
        }
    }
}
