using System;
using System.Collections.Generic;
using System.Text;
using Core.Utils;

namespace Core.Protocol
{
    public class NetInfoCell : Cell
    {
        public NetInfoCell(DateTime dateTime, string otherAddress, List<string> thisAddresses) : base(8)
        {
            DateTime = dateTime;
            OtherAddress = otherAddress;
            ThisAddresses = thisAddresses;
        }

        public DateTime DateTime { get; private set; }
        public string OtherAddress { get; private set; }

        public List<string> ThisAddresses { get; private set; }

        public static NetInfoCell Parse(byte[] payload, int startIndex)
        {
            var timestamp = ByteHelper.ReadInt32(payload[startIndex + 3], payload[startIndex + 4], payload[startIndex + 5], payload[startIndex + 6]);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var dateTime = epoch.AddSeconds(timestamp).ToLocalTime();
            var otherAddressLength = payload[startIndex + 8];
            var otherAddress = ParseAddress(payload, startIndex + 9, otherAddressLength);

            var thisAddressCount = payload[startIndex + 9 + otherAddressLength];
            var thisAddresses = new List<string>();
            for (int i = 0, addressIndex = startIndex + 10 + otherAddressLength; i < thisAddressCount; i++)
            {
                otherAddressLength = payload[addressIndex + 1];
                thisAddresses.Add(ParseAddress(payload, addressIndex + 2, otherAddressLength));
                addressIndex += otherAddressLength;
            }
            return new NetInfoCell(dateTime, otherAddress, thisAddresses);
        }

        private static string ParseAddress(byte[] payload, int startIndex, byte otherAddressLength)
        {
            var otherAddress = new StringBuilder();
            for (int i = 0; i < otherAddressLength; i++)
            {
                otherAddress.Append(payload[startIndex + i]).Append(".");
            }
            otherAddress.Remove(otherAddress.Length - 1, 1);
            return otherAddress.ToString();
        }

        protected override byte[] GetPayload()
        {
            return new byte[17];// TODO not implemented
        }
    }
}
