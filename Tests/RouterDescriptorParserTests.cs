using System;
using System.Linq;
using System.Net;
using System.Text;
using Core.Documents;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class RouterDescriptorParserTests
    {
        private const string TestRouterDescription = @"router lexszero 37.146.198.71 9001 0 9030
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

        private const string ExpectedOnionKey = "-----BEGIN RSA PUBLIC KEY-----\nMIGJAoGBAJ6E2YmGPvAuvalADxX+hLcd5cCbmEzSnyVagOy45L5Mu7SNesUuzFa2\n/BmP45UmSb9n2rWmeVIBzTrlsDJC6cMe8qnKKUUlGH8xhZJHJZ07cq7A4C8yLNl+\nx6tTvkW9dBnKVYFE1zB8afnAVLNgX7OYqSNlCvX72KaIgkrjoqHjAgMBAAE=\n-----END RSA PUBLIC KEY-----";
        private const string ExpectedSigningKey = "-----BEGIN RSA PUBLIC KEY-----\nMIGJAoGBAM9aKm2NSH+ZO4bBIFqymdCCuZJUAoszPTjtHPnucSEtl4w+NWvN91Hq\n0Gxza2cHGyPtc993clQUKlreaocIl2RiBx2KFWfCYo0qjBHrLTREMwXgmUYI83TK\nSJxrPZWtmopZiBfdPVdU+WJ0kQL9D8deSC0hGuYkRpxRpFFzYAehAgMBAAE=\n-----END RSA PUBLIC KEY-----";
        private const string ExpectedFingerprint = "90EA 030D 227B 6081 F3DF FBBF 0C06 A1F9 617D 5D9E";
        private const string ExpectedNTorOnionKey = "LLcWxTyP0PUVDVSTwAaecjs8Cs/G67sBNO4m9NGDIiE=";

        private const string RouterDescription = @"router lexszero 37.146.198.71 9001 0 9030\nbla-bla";
        private const string SingningKeyDescription = "signing-key\n-----BEGIN RSA PUBLIC KEY-----\nMIGJAoGBAM9aKm2NSH+ZO4bBIFqymdCCuZJUAoszPTjtHPnucSEtl4w+NWvN91Hq\n0Gxza2cHGyPtc993clQUKlreaocIl2RiBx2KFWfCYo0qjBHrLTREMwXgmUYI83TK\nSJxrPZWtmopZiBfdPVdU+WJ0kQL9D8deSC0hGuYkRpxRpFFzYAehAgMBAAE=\n-----END RSA PUBLIC KEY-----";
        private const string OnionKeyDescription = "onion-key\n-----BEGIN RSA PUBLIC KEY-----\nMIGJAoGBAJ6E2YmGPvAuvalADxX+hLcd5cCbmEzSnyVagOy45L5Mu7SNesUuzFa2\n/BmP45UmSb9n2rWmeVIBzTrlsDJC6cMe8qnKKUUlGH8xhZJHJZ07cq7A4C8yLNl+\nx6tTvkW9dBnKVYFE1zB8afnAVLNgX7OYqSNlCvX72KaIgkrjoqHjAgMBAAE=\n-----END RSA PUBLIC KEY-----";
        private const string FingerprintDescription = "fingerprint 90EA 030D 227B 6081 F3DF FBBF 0C06 A1F9 617D 5D9E\n";
        private const string NTorOnionKeytDescription = "ntor-onion-key LLcWxTyP0PUVDVSTwAaecjs8Cs/G67sBNO4m9NGDIiE=\n";

        [Test]
        public void OnionKeyPatternTest() {
            string actualKey;

            if (RouterDescriptorParser.TryGetPublicKey(OnionKeyDescription, out actualKey))
            {
                Assert.AreEqual(ExpectedOnionKey, actualKey);
            } 
            else 
            {
                Assert.Fail();
            }
        }

        [Test]
        public void RouterDescritionTest()
        {
            NetworkEntity networkEntity;
            if (RouterDescriptorParser.GetNetowkEntity(RouterDescription, out networkEntity))
            {
                Assert.AreEqual("lexszero", networkEntity.Name);
                Assert.AreEqual("37.146.198.71", networkEntity.Address);
                Assert.AreEqual("9001", networkEntity.Port);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void SigningKeyTest()
        {
            string actualSigningKey;

            if(RouterDescriptorParser.TryGetSigningKey(SingningKeyDescription, out actualSigningKey))
            {
                Assert.AreEqual(ExpectedSigningKey, actualSigningKey);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void FingerprintTest()
        {
            string fingerprint;

            if (RouterDescriptorParser.TryGetFingerprint(FingerprintDescription, out fingerprint))
            {
                Assert.AreEqual(ExpectedFingerprint, fingerprint);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void NTorOnionKeyTest()
        {
            string ntorOnionKey;

            if (RouterDescriptorParser.TryGetNTorOnionKey(NTorOnionKeytDescription, out ntorOnionKey))
            {
                Assert.AreEqual(ExpectedNTorOnionKey, ntorOnionKey);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void RouterDescriptionParserTests()
        {

            var routerDescriptions = RouterDescriptorParser.GetDescriptors(TestRouterDescription.Replace(Environment.NewLine, "\n"));
            Assert.AreEqual(1, routerDescriptions.Count());

            var actual = routerDescriptions.First();

            Assert.AreEqual("lexszero", actual.NetworkEntity.Name);
            Assert.AreEqual("37.146.198.71", actual.NetworkEntity.Address);
            Assert.AreEqual("9001", actual.NetworkEntity.Port);
            Assert.AreEqual(ExpectedOnionKey, actual.OnionKey);
            Assert.AreEqual(ExpectedFingerprint, actual.FingerPrint); 
            Assert.AreEqual(ExpectedSigningKey, actual.SigningKey);
            Assert.AreEqual(ExpectedNTorOnionKey, actual.NTorOnionKey);
        }

        [Test, Explicit]
        public void ParseTest()
        {
            var url = "http://86.59.21.38:80/tor/server/all";
            var webClient = new WebClient();
            var result = webClient.DownloadData(url);
            var str = Encoding.ASCII.GetString(result);

            var routers = RouterDescriptorParser.GetDescriptors(str);
        }
    }
}
