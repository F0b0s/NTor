using System.IO;
using System.Linq;
using Core.Documents;
using Core.Protocol;
using NUnit.Framework;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

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

            var createCell = new CreateCell(sample, crypthoProvider);
            var arr = createCell.ToArray();
        }
    }
}
