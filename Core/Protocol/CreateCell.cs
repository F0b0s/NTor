using System;
using System.Numerics;
using Core.Documents;
using Core.Infrastructure;

namespace Core.Protocol
{
    public class CreateCell : Cell
    {
        private readonly RouterDescriptor routerDescriptor;
        private static string DH_moduls_base64 = @"//////////+BU+bsUWYoSeYfS3wRJJ+upZ+JWvtrOO7ttwb0tlz/C2vtN6bpQkz0xn5eYna1heRFwlFtbTXhTzcUX/JtCiswG0M6zbMZle/dBDSOeQhKUSKbEzumvgsCdMxnighOAinRHNyAi2LGxDTCaCGi2g/J//////////8A";
        public static BigInteger DH_modulus_p = new BigInteger(Convert.FromBase64String(DH_moduls_base64));

        private readonly ISymmetricalProvider symmetricalProvider;

        public CreateCell(RouterDescriptor routerDescriptor, ISymmetricalProvider symmetricalProvider)
        {
            this.routerDescriptor = routerDescriptor;
            this.symmetricalProvider = symmetricalProvider;
        }

        public override byte[] ToArray()
        {
            byte[] data = new byte[144];
            var symmetricKey = symmetricalProvider.GenerateKey(16);

            var dh_g = new BigInteger(2);
            var dh_private = new BigInteger(symmetricalProvider.GenerateKey(Convert.FromBase64String(DH_moduls_base64).Length - 1));

            if(dh_private.Sign == -1)
            {
                dh_private = BigInteger.Negate(dh_private);
            }

            var dh_x = BigInteger.ModPow(dh_g, dh_private, DH_modulus_p);
            var dh_x_bytes = dh_x.ToByteArray();

            Array.Copy(symmetricKey, 0, data, 0, 16);
            Array.Copy(dh_x_bytes, 0, data, 16, 128);

            var part1Len = symmetricalProvider.GetBlockSizeForKey(routerDescriptor.OnionKey);
            var part1 = symmetricalProvider.AssymEncrypt(data, routerDescriptor.OnionKey, 0, part1Len);

            var part2ToEncrypt = new byte[data.Length - part1Len];
            Array.Copy(data, part1Len, part2ToEncrypt, 0, data.Length - part1Len);
            var part2 = symmetricalProvider.SymmEncrypt(part2ToEncrypt, symmetricKey, new byte[16]);

            var bytes = new byte[part1.Length + part2.Length];

            Array.Copy(part1, 0, bytes, 0, part1.Length);
            Array.Copy(part2, 0, bytes, part1.Length, part2.Length);

            return bytes;
        }
    }
}