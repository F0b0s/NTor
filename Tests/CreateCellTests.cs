using System;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Core.Protocol;
using NUnit.Framework;
using Org.BouncyCastle.Crypto.Tls;

namespace Tests
{
    [TestFixture]
    public class CreateCellTests
    {
        [Test, Ignore]
        public void GenerateKey()
        {
            var rsaProvider = RSA.Create();
            var key = rsaProvider.ExportParameters(true);
            rsaProvider.ToXmlString(true);
        }

        [Test, Ignore]
        public void Test()
        {
            var routers = RouterDescriptotLoader.Load();
            foreach(var sample in routers.Where(x => x.NetworkEntity.Port == "443").Skip(1))
            {
                try
                {
                    var tcpClient = new TcpClient(sample.NetworkEntity.Address, 443)
                                    {
                                        ReceiveTimeout = 1000 * 40
                                    };
                    var tlsHandler = new TlsProtocolHandler(tcpClient.GetStream());
                    var tlsClient = new MyTlsClient();
                    tlsHandler.Connect(tlsClient);

                    var createCell = new VersionsCell(new ushort[]{3});
                    var buffer = createCell.ToArray();
                    tlsHandler.Stream.Write(buffer, 0, buffer.Length);

                    var buff = new byte[1024];
                    var result = new byte[0];
                    int bytesRead;

                    do
                    {
                        bytesRead = tlsHandler.Stream.Read(buff, 0, buff.Length);
                        Array.Resize(ref  result, result.Length + bytesRead);
                        Array.Copy(buff, 0, result, result.Length - bytesRead, bytesRead);
                    } while (bytesRead == buff.Length);

                    var res = CellsParser.ParseResponse(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    continue;
                }
            }
        }

        [Test, Ignore]
        public void Test1()
        {
            const string response =
                "AAAHAAQAAwAEAACBA4sCAQHEMIIBwDCCASmgAwIBAgIIOo2CiggyYdAwDQYJKoZIhvcNAQEFBQAwIDEeMBwGA1UEAwwVd3d3Lmp1dm51bzY2N25zdHQuY29tMB4XDTE1MDEwNTAwMDAwMFoXDTE1MDUzMTAwMDAwMFowJTEjMCEGA1UEAwwad3d3Lm43b2dsenFnd21tZnB1Nm41eS5uZXQwgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBAKPhxBks7bMDAS+HAn3J20vWJ+NCfI4Ua1CzqmiSD+VfOgTNJ/YOqxnHk62DifcwuxpoO47nXQsFN7SBbMFy825unso9LvmKiiYm9AGq2J+/WizSEDFmTLohaqln0JQ32xDP4RZANloo21/0wgjUXpCjYbWAm+n9ETPCiZTqYREdAgMBAAEwDQYJKoZIhvcNAQEFBQADgYEAI30/1rVygUKiVrHmRjvUzgOfan+sypCprn1smeWCtUDzYGJXGN9lPFYCCeyfzSk8nM3R/sT/9md/hctjn6jVoa6W9C2nC9rqL+dpIUrJ7ImPvrMYEbwZ2mrjV4lyUe4G0/Vqgew4wKj8TcOPXOSVb9PY4n2tRn9XkHuwJK/qEy8CAcAwggG8MIIBJaADAgECAgkA6c87TMvac9kwDQYJKoZIhvcNAQEFBQAwIDEeMBwGA1UEAwwVd3d3Lmp1dm51bzY2N25zdHQuY29tMB4XDTE0MDgyMzAwMDAwMFoXDTE1MDgyMzAwMDAwMFowIDEeMBwGA1UEAwwVd3d3Lmp1dm51bzY2N25zdHQuY29tMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDjuhSooLKVVle130vjl4plDWySzUkaxOyNm7vJAo6y9sF+2MaPCUEFtRcRcFFzMf9EGIIpW7RYhPo81Ey+t6qi43PhpBRmnK3/MbWQZHpeehaFmRiOPtB+sgiF6Ub4t9FY22cYyif1t1n7LUQ87Ci1vJMLAqbOUF5rCjqQefs5swIDAQABMA0GCSqGSIb3DQEBBQUAA4GBACtRNfzDSoP5QY1FcjzZ7/HOStuRTc0r4OcBWA0yZ3lslClyNlAIk/UTnnr9XiyrlUKkJEeFFSNJSDvLrck/E1JJjPdrgk1fWjRCvrr/JIQMueQfRsL188gSeH6m7kY0/UJJ3clu7M0/ioOi35PN+6ff2WaD0cdMm/2QXYUP3nqyAACCACS+/imKwlWgn3kJTwNJZy+dlRzhbkhRvFgFpPuAXlu3fwABAAEAAAhU4kziBARt/AirAQQEWwnrjQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==";
            var responseBytes = Convert.FromBase64String(response);

            var result = CellsParser.ParseResponse(responseBytes);
            var certcell = (CertsCell)result.FirstOrDefault(x => x.GetType() == typeof(CertsCell));

            var cert = certcell.Certificates.First();
            var x509 = new X509Certificate2(cert.CertBytes);

            var challengeCell = (ChallengeCell)result.FirstOrDefault(x => x.GetType() == typeof(ChallengeCell));

            var str = Encoding.ASCII.GetString(challengeCell.Challenge);
        }
    }
}
