using System.IO;
using System.Security.Cryptography;
using Core.Infrastructure;

namespace Tests
{
    public class CrypthoProvider : ISymmetricalProvider
    {
        RandomNumberGenerator randomNumberGenerator;

        public CrypthoProvider()
        {
            randomNumberGenerator = new RNGCryptoServiceProvider();
        }

        public byte[] GenerateKey(int len)
        {
            var bytes = new byte[len];
            randomNumberGenerator.GetBytes(bytes);
            return bytes;
        }

        public byte[] AssymEncrypt(byte[] data)
        {
            var rsaParams = new RSAParameters();
            rsaParams.Modulus = null;
            rsaParams.Exponent = null;

            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParams);

            return rsa.Encrypt(data, true);
        }

        public byte[] SymmEncrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(data, 0, data.Length);
                        csEncrypt.FlushFinalBlock();
                        return msEncrypt.ToArray();
                    }
                }
            }
        }
    }
}
