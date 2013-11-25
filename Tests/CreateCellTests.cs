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
            var sample = routers.First(x => x.NetworkEntity.Port == "443");
            var crypthoProvider = new CrypthoProvider();

            var tcpClient = new TcpClient(sample.NetworkEntity.Address, 443);
            var tlsHandler = new TlsProtocolHandler(tcpClient.GetStream());
            tlsHandler.Connect(new MyTlsClient());

            var createCell = new CreateCell(sample, crypthoProvider);
            byte[] buffer = createCell.ToArray();
            tlsHandler.Stream.Write(buffer, 0, buffer.Length);

            var result = new byte[1024];
            tlsHandler.Stream.Read(result, 0, result.Length);
        }
    }
}
