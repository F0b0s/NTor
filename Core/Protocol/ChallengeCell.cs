using System;
using System.Collections.Generic;
using Core.Utils;

namespace Core.Protocol
{
    public class ChallengeCell : Cell
    {
        public ChallengeCell(int[] methods, byte[] challenge) : base(130)
        {
            Methods = methods;
            Challenge = challenge;
        }

        public byte[] Challenge { get; private set; }

        public int[] Methods { get; private set; }

        public static ChallengeCell Parse(byte[] cellBytes)
        {
            var challenge = new byte[32];
            Array.Copy(cellBytes, challenge, challenge.Length);
            var methodsCount = ByteHelper.ReadInt16(cellBytes[32], cellBytes[33]);
            var methods = new List<int>(methodsCount);
            
            for (int i = 0, index = 34; i < methodsCount; i++, index += 2)
            {
                var method = ByteHelper.ReadInt16(cellBytes[index], cellBytes[index + 1]);
                methods.Add(method);
            }

            return new ChallengeCell(methods.ToArray(), challenge);
        }

        protected override byte[] GetPayload()
        {
            throw new System.NotImplementedException();
        }
    }
}
