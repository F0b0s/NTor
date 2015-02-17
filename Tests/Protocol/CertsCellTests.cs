using System;
using Core.Protocol;
using NUnit.Framework;

namespace Tests.Protocol
{
    [TestFixture]
    public class CertsCellTests
    {
        [Test]
        public void Ctor_PassNull_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new CertsCell(null));
        }

        [Test]
        public void Ctor_PassCertifcates_ShouldCreateCell()
        {
            var expected = new byte[] {0x00, 0x00, 0x81, 0x00, 0x06, 0x01, 0x01, 0x00, 0x02, 0x01, 0x02};
            var cert = new Certificate
                       {
                           CertType = 1,
                           CertBytes = new byte[] {0x01, 0x02}
                       };
            Cell cell = new CertsCell(new[] {cert});

            var actual = cell.ToArray();

            Assert.AreEqual(expected, actual);
        }
    }
}
