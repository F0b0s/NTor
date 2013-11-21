using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.Documents;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class RouterDescriptorParserTests
    {
        private const string ExpectedOnionKey = 
@"-----BEGIN RSA PUBLIC KEY-----
MIGJAoGBAJ6E2YmGPvAuvalADxX+hLcd5cCbmEzSnyVagOy45L5Mu7SNesUuzFa2
/BmP45UmSb9n2rWmeVIBzTrlsDJC6cMe8qnKKUUlGH8xhZJHJZ07cq7A4C8yLNl+
x6tTvkW9dBnKVYFE1zB8afnAVLNgX7OYqSNlCvX72KaIgkrjoqHjAgMBAAE=
-----END RSA PUBLIC KEY-----";

        [Test]
        public void OnionKeyPatternTest() {
            string description = 
@"onion-key
-----BEGIN RSA PUBLIC KEY-----
MIGJAoGBAJ6E2YmGPvAuvalADxX+hLcd5cCbmEzSnyVagOy45L5Mu7SNesUuzFa2
/BmP45UmSb9n2rWmeVIBzTrlsDJC6cMe8qnKKUUlGH8xhZJHJZ07cq7A4C8yLNl+
x6tTvkW9dBnKVYFE1zB8afnAVLNgX7OYqSNlCvX72KaIgkrjoqHjAgMBAAE=
-----END RSA PUBLIC KEY-----";

            var regex = new Regex(RouterDescriptorParser.OnionKeyPattern);
            string actualKey = null;

            if (RouterDescriptorParser.TryGetPublicKey(description, out actualKey)) {
                Assert.AreEqual(ExpectedOnionKey, actualKey);
            } else {
                Assert.Fail();
            }
        }
    }
}
