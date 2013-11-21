using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Core.Documents
{
    public class RouterDescriptorParser
    {
        //public static string OnionKeyPattern = @"onion key[\n]*?\n([.]*?-----END RSA PUBLIC KEY-----\n)";
        public static string OnionKeyPattern = @"onion key";

        private static Regex OnionKeyRegex = new Regex(OnionKeyPattern);

        public RouterDescriptorParser() {
            
        }

        public IEnumerable<RouterDescriptor> GetDescriptors(string textData) {
            if (string.IsNullOrEmpty(textData)) {
                throw new ArgumentException("'textData' should be defined");
            }

            if (!textData.StartsWith("router")) {
                throw new ArgumentException("Bad document");
            }

            var routersList = new List<RouterDescriptor>();
            var splittedByrouter = textData.Split(new[] {"router"}, StringSplitOptions.RemoveEmptyEntries);

            if (splittedByrouter.Length == 0) {
                throw new InvalidOperationException("No router descriptions has been found");
            }

            var routersDescriptions = new List<RouterDescriptor>();

            foreach (var routerDescription in splittedByrouter) 
            {
                RouterDescriptor routerDescriptor;

            }


            return routersList;
        }

        public static bool TryGetPublicKey(string descriptionText, out string publicKey) {
            var match = OnionKeyRegex.Match(descriptionText);

            if (match.Success) {
                publicKey = match.Groups[0].Value;
                return true;
            }

            publicKey = null;
            return false;
        } 
    }
}
