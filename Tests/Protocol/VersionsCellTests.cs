using System;
using Core.Protocol;
using NUnit.Framework;

namespace Tests.Protocol
{
    [TestFixture]
    public class VersionsCellTests
    {
        [Test]
        public void Ctor_PassNull_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new VersionsCell(null));
        }

        [Test]
        public void Ctor_PassSomeVersions_ShouldReturnCorrectPayload()
        {
            var expected = new byte[] {0x00, 0x00, 0x07, 0x00, 0x04, 0x00, 0x03, 0x00, 0x04};
            var versions = new ushort[] {3, 4};
            var versionsCell = new VersionsCell(versions);

            var actual = versionsCell.ToArray();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Parse_PassVersionsCell_ShouldBeParsed()
        {
            var cellData = new byte[] { 0x00, 0x03, 0x00, 0x04 };
            var expected = new ushort[] { 3, 4 };
            var versionsCell = VersionsCell.Parse(cellData);

            Assert.AreEqual(expected, versionsCell.Versions);
        }
    }
}
