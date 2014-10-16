using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Core.Protocol;
using NUnit.Framework;
using Org.BouncyCastle.Crypto.Tls;

namespace Tests
{
    [TestFixture]
    public class CreateCellTests
    {
        [Test]
        public void Test()
        {
            var routers = RouterDescriptotLoader.Load();
            foreach(var sample in routers.Where(x => x.NetworkEntity.Port == "443"))
            {
                try
                {
                    var tcpClient = new TcpClient(sample.NetworkEntity.Address, 443);
                    var tlsHandler = new TlsProtocolHandler(tcpClient.GetStream());
                    var tlsClient = new MyTlsClient();

                    tlsHandler.Connect(tlsClient);


                    var createCell = new VersionCell();
                    byte[] buffer = createCell.ToArray();
                    tlsHandler.Stream.Write(buffer, 0, buffer.Length);

                    var buff = new byte[1024];
                    var result = new byte[0];
                    int bytesRead = 0;
                    
                    while ((bytesRead = tlsHandler.Stream.Read(buff, 0, buff.Length)) != 0)
                    {
                        Array.Resize(ref  result, result.Length + bytesRead);
                        Array.Copy(buff, 0, result, result.Length - bytesRead, bytesRead);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    continue;
                }
            }
        }
    }
}
