using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using NUnit.Framework;
using OpenSSL.SSL;
using OpenSSL.X509;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using SslStream = OpenSSL.SSL.SslStream;

namespace Tests
{
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
            File.WriteAllText("X:\\data.txt", str);
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
        public void ConnectTest()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var tcpClient = new TcpClient("178.24.64.33", 443);
            
//            using(var stream = tcpClient.GetStream())
//            {
//                var clientMessage = Encoding.ASCII.GetBytes("Client hello");
//                stream.Write(clientMessage, 0, clientMessage.Length);
//                var bufer = new byte[1000];
//                var r = stream.Read(bufer, 0, bufer.Length);
//            }

//            return;

            Console.WriteLine("Client connected.");
            var sslStream = new SslStream(
                tcpClient.GetStream(),
                false,
                new RemoteCertificateValidationHandler(ValidateServerCertificate),
                null
                );

//            sslStream.SslProtocol = 
            try
            {
                sslStream.AuthenticateAsClient("default");
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
                Console.WriteLine("Authentication failed - closing the connection.");
                tcpClient.Close();
                return;
            }
        }

        // The following method is invoked by the RemoteCertificateValidationDelegate. 
        public static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              int some, VerifyResult verifyResult)
        {
//            if (sslPolicyErrors == SslPolicyErrors.None)
//                return true;

            //Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers. 
            return false;
        }
    }
}
