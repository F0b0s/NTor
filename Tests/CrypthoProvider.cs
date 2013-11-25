using System.IO;
using System.Security.Cryptography;
using Core.Infrastructure;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace Tests
{
    public class CrypthoProvider : ISymmetricalProvider
    {
        readonly RandomNumberGenerator randomNumberGenerator;

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

        public byte[] AssymEncrypt(byte[] data, string pemPublicKey, int @from, int len)
        {
            var pemReader = new PemReader(new StringReader(pemPublicKey));
            var parameters = (RsaKeyParameters)pemReader.ReadObject();

            var rsa = new RsaEngine();
            var oaep = new OaepEncoding(rsa);
            oaep.Init(true, parameters);

            return oaep.ProcessBlock(data, @from, len);
        }

        public int GetBlockSizeForKey(string pemPublicKey)
        {
            var pemReader = new PemReader(new StringReader(pemPublicKey));
            var parameters = (RsaKeyParameters)pemReader.ReadObject();

            var rsa = new RsaEngine();
            var oaep = new OaepEncoding(rsa);
            oaep.Init(true, parameters);

            return oaep.GetInputBlockSize();
        }

        public byte[] AssymEncrypt(byte[] data, byte[] modulus, byte[] exponent)
        {
            var rsaParams = new RSAParameters();
            rsaParams.Modulus = modulus;
            rsaParams.Exponent = exponent;

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

        public byte[] GetExponent(string key)
        {
            using (var textReader = new StringReader(key))
            {
                var keyPair = (RsaKeyParameters)new PemReader(textReader).ReadObject();
                return keyPair.Exponent.ToByteArray();
            }
        }

        public byte[] GetModulus(string key)
        {
            using (var textReader = new StringReader(key))
            {
                var keyPair = (RsaKeyParameters)new PemReader(textReader).ReadObject();
                return keyPair.Modulus.ToByteArray();
            }
        }
    }
}
