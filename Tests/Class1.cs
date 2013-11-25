using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Core.Protocol;
using NUnit.Framework;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;

namespace Tests
{
    class MyTlsClient : DefaultTlsClient
    {
        public override TlsAuthentication GetAuthentication()
        {
            return new MyTlsAuthentication();
        }
    }

    // Need class to handle certificate auth
    class MyTlsAuthentication : TlsAuthentication
    {
        public TlsCredentials GetClientCredentials(CertificateRequest certificateRequest)
        {
            // return client certificate
            return null;
        }

        public void NotifyServerCertificate(Certificate serverCertificate)
        {
            // validate server certificate
        }
    }

    public class TlsClient : DefaultTlsClient
    {
        public override TlsAuthentication GetAuthentication() {
            return new LegacyTlsAuthentication(new AlwaysValidVerifyer());
        }
    }

    [TestFixture]
    public class Class1
    {
        [Test]
        public void Test()
        {
            var url = "http://86.59.21.38:80/tor/server/all";
            var webClient = new WebClient();
            var result = webClient.DownloadData(url);
            var str = Encoding.ASCII.GetString(result);
            File.WriteAllText("data.txt", str);
        }

        [Test]
        public void Check()
        {
            var data = @"router lexszero 37.146.198.71 9001 0 9030
platform Tor 0.2.4.16-rc on Linux
protocols Link 1 2 Circuit 1
published 2013-10-26 19:17:41
fingerprint 90EA 030D 227B 6081 F3DF FBBF 0C06 A1F9 617D 5D9E
uptime 10
bandwidth 524288 786432 0
extra-info-digest 5B2752FE6CE35BB7D0EF0A9CD2D80F1F2F528081
onion-key
-----BEGIN RSA PUBLIC KEY-----
MIGJAoGBAJ6E2YmGPvAuvalADxX+hLcd5cCbmEzSnyVagOy45L5Mu7SNesUuzFa2
/BmP45UmSb9n2rWmeVIBzTrlsDJC6cMe8qnKKUUlGH8xhZJHJZ07cq7A4C8yLNl+
x6tTvkW9dBnKVYFE1zB8afnAVLNgX7OYqSNlCvX72KaIgkrjoqHjAgMBAAE=
-----END RSA PUBLIC KEY-----
signing-key
-----BEGIN RSA PUBLIC KEY-----
MIGJAoGBAM9aKm2NSH+ZO4bBIFqymdCCuZJUAoszPTjtHPnucSEtl4w+NWvN91Hq
0Gxza2cHGyPtc993clQUKlreaocIl2RiBx2KFWfCYo0qjBHrLTREMwXgmUYI83TK
SJxrPZWtmopZiBfdPVdU+WJ0kQL9D8deSC0hGuYkRpxRpFFzYAehAgMBAAE=
-----END RSA PUBLIC KEY-----
hidden-service-dir
contact Tor Admin <tor@lexs.blasux.ru>
ntor-onion-key LLcWxTyP0PUVDVSTwAaecjs8Cs/G67sBNO4m9NGDIiE=
reject 0.0.0.0/8:*
reject 169.254.0.0/16:*
reject 127.0.0.0/8:*
reject 192.168.0.0/16:*
reject 10.0.0.0/8:*
reject 172.16.0.0/12:*
reject 37.146.198.71:*
reject *:80
reject *:443
reject *:8080
reject *:8000
reject *:6660-6667
accept *:*
router-signature
";

            //router-signature
            var signature = @"nY9ttBce4BmjaKyt5WKJKZM+RJ+MC4HnKBOHGR3BlvLyz7qkUhIlR7tNKdeGsjm7OcWJ2/pcO0gL96AXrTY3IJadAVvvSPRzk3NTRx1dNyXbvZkKQRujtYKxSn3n3sIi5ZhxRQE/e0mZ1MXRg6tCDAXzJMmlKGr+wb/FQJ3i0s0=";

            RsaKeyParameters keyPair;
            using (var reader = File.OpenText(@"test.pem")) // file containing RSA PKCS1 private key
                keyPair = (RsaKeyParameters)new PemReader(reader).ReadObject(); 


            var decryptEngine = new Pkcs1Encoding(new RsaEngine());
            decryptEngine.Init(false, keyPair);

            //var decrypted = Encoding.UTF8.GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));


            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                var rsaParams = new RSAParameters()
                                    {
                                        Exponent = keyPair.Exponent.ToByteArray(),
                                        Modulus = keyPair.Modulus.ToByteArray()
                                    };
                rsa.ImportParameters(rsaParams);
//                string hashString = @"nY9ttBce4BmjaKyt5WKJKZM+RJ+MC4HnKBOHGR3BlvLyz7qkUhIlR7tNKdeGsjm7
//OcWJ2/pcO0gL96AXrTY3IJadAVvvSPRzk3NTRx1dNyXbvZkKQRujtYKxSn3n3sIi
//5ZhxRQE/e0mZ1MXRg6tCDAXzJMmlKGr+wb/FQJ3i0s0=";
                byte[] hash = Convert.FromBase64String(signature);
                //byte[] hash = Encoding.UTF8.GetBytes(signature);
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                Array.Reverse(buffer);
                Console.WriteLine(rsa.VerifyData(buffer, CryptoConfig.MapNameToOID("SHA1"), hash));
            }
        }

        [Test]
        public void BI()
        {
            string value = @"00FFFFFFFFFFFFFFFFC90FDAA22168C234C4C6628B80DC1CD129024E08" + 
                "8A67CC74020BBEA63B139B22514A08798E3404DDEF9519B3CD3A431B" + 
                "302B0A6DF25F14374FE1356D6D51C245E485B576625E7EC6F44C42E9" + 
                "A637ED6B0BFF5CB6F406B7EDEE386BFB5A899FA5AE9F24117C4B1FE6" + 
                "49286651ECE65381FFFFFFFFFFFFFFFF";
            var bi = System.Numerics.BigInteger.Parse(value, NumberStyles.HexNumber);
            Assert.AreEqual(CreateCell.DH_modulus_p, bi);
        }
    }
}
