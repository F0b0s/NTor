using System;
using Core.Protocol;
using NUnit.Framework;

namespace Tests.Protocol
{
    [TestFixture]
    public class NetInfoClassTests
    {
        [Test]
        public void Parse_PassPayload_ShouldParseIt()
        {
            var payload = new byte[] { 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x01, 0x04, 0x04, 0x01, 0x02, 0x03, 0x04, 0x01, 0x04, 0x04, 0x01, 0x02, 0x03, 0x04 };
            var cell = NetInfoCell.Parse(payload, 0);
        }
    }
}
