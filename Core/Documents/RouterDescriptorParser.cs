using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Core.Documents
{
    public class RouterDescriptorParser
    {
        public static string OnionKeyPattern = @"onion-key[\s\n]+?(-----BEGIN RSA PUBLIC KEY-----[\w\W\n]+?-----END RSA PUBLIC KEY-----)";
        public static string SigningKeyPattern = @"signing-key[\s\n]+?(-----BEGIN RSA PUBLIC KEY-----[\w\W\n]+?-----END RSA PUBLIC KEY-----)";
        public static string NetworkEntityPattern = @"^router ([\w]+?) ([\S]+?) ([\d]+?) ([\d]+?) ([\d]+?)";
        public static string FingerprintPattern = @"fingerprint[\s\n]+?([\s\w]+?)\n";
        public static string NTorOnionKeyPattern = @"ntor-onion-key[\s\n]+?([\w\W]+?)\n";

        private static readonly Regex OnionKeyRegex = new Regex(OnionKeyPattern);
        private static readonly Regex SigningKeyRegex = new Regex(SigningKeyPattern);
        private static readonly Regex RouterDescriptionRegex = new Regex(NetworkEntityPattern);
        private static readonly Regex FingerPrintRegex = new Regex(FingerprintPattern);
        private static readonly Regex NTorOnionKeyRegex = new Regex(NTorOnionKeyPattern);
        
        public static IEnumerable<RouterDescriptor> GetDescriptors(string textData) {
            if (string.IsNullOrEmpty(textData)) {
                throw new ArgumentException("'textData' should be defined");
            }

            if (!textData.StartsWith("router")) {
                throw new ArgumentException("Bad document");
            }

            var routersDescriptions = new List<RouterDescriptor>();
            var routers = textData.Split(new[] {"router "}, StringSplitOptions.RemoveEmptyEntries);

            if(routers.Length == 0)
            {
                throw new InvalidOperationException("Bad document");
            }

            foreach (var router in routers)
            {
                RouterDescriptor routerDescriptor;
                if (ParseRouter("router " + router, out routerDescriptor))
                {
                    routersDescriptions.Add(routerDescriptor);
                }
            }

            return routersDescriptions;
        }

        public static bool ParseRouter(string routerDescriptionText, out RouterDescriptor routerDescriptor)
        {
            NetworkEntity networkEntity;
            string publicKey;
            string onionKey;
            string signingKey;
            string fingerprint;

            if (GetNetowkEntity(routerDescriptionText, out networkEntity) &&
                TryGetPublicKey(routerDescriptionText, out publicKey) &&
                TryGetNTorOnionKey(routerDescriptionText, out onionKey) &&
                TryGetSigningKey(routerDescriptionText, out signingKey) &&
                TryGetFingerprint(routerDescriptionText, out fingerprint))
            {
                routerDescriptor = new RouterDescriptor
                                       {
                                           NetworkEntity = networkEntity,
                                           OnionKey = publicKey,
                                           NTorOnionKey = onionKey,
                                           SigningKey = signingKey,
                                           FingerPrint = fingerprint
                                       };

                return true;
            }

            routerDescriptor = null;
            return false;
        }

        public static bool GetNetowkEntity(string routerDescription, out NetworkEntity networkEntity)
        {
            var matchCollection = RouterDescriptionRegex.Matches(routerDescription);

            if(matchCollection.Count == 1)
            {
                var match = matchCollection[0];
                if (match.Groups.Count == 6)
                {
                    //TODO: SOCKSPort DirPort doesn't used now
                    networkEntity = new NetworkEntity(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
                    return true;
                }
            }

            networkEntity = null;
            return false;
        }

        public static bool TryGetPublicKey(string descriptionText, out string publicKey) {
            var matchCollection = OnionKeyRegex.Matches(descriptionText);

            if(matchCollection.Count == 1)
            {
                var match = matchCollection[0];
                if(match.Groups.Count == 2)
                {
                    publicKey = match.Groups[1].Value;
                    return true;
                }
            }

            publicKey = null;
            return false;
        }

        public static bool TryGetSigningKey(string descriptionText, out string signingKey)
        {
            var matchCollection = SigningKeyRegex.Matches(descriptionText);

            if (matchCollection.Count == 1)
            {
                var match = matchCollection[0];
                if (match.Groups.Count == 2)
                {
                    signingKey = match.Groups[1].Value;
                    return true;
                }
            }

            signingKey = null;
            return false;
        }

        public static bool TryGetFingerprint(string descriptionText, out string fingerprint)
        {
            var matchCollection = FingerPrintRegex.Matches(descriptionText);

            if (matchCollection.Count == 1)
            {
                var match = matchCollection[0];
                if (match.Groups.Count == 2)
                {
                    fingerprint = match.Groups[1].Value;
                    return true;
                }
            }

            fingerprint = null;
            return false;
        }

        public static bool TryGetNTorOnionKey(string descriptionText, out string ntorOnionKey)
        {
            var matchCollection = NTorOnionKeyRegex.Matches(descriptionText);

            if (matchCollection.Count == 1)
            {
                var match = matchCollection[0];
                if (match.Groups.Count == 2)
                {
                    ntorOnionKey = match.Groups[1].Value;
                    return true;
                }
            }

            ntorOnionKey = null;
            return false;
        } 

    }
}
