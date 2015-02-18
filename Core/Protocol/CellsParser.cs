using System;
using System.Collections.Generic;
using System.Globalization;

namespace Core.Protocol
{
    public static class CellsParser
    {
        public static IEnumerable<Cell> ParseResponse(byte[] response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            return ParseCell(response, 0);
        }

        private static IEnumerable<Cell> ParseCell(byte[] response, int startIndex)
        {
            var result = new List<Cell>();
            var currentIndex = startIndex;

            while (currentIndex < response.Length)
            {
                var command = response[currentIndex + 2];

                var payloadLen = ReadUInt16(response, currentIndex + 3);

                switch (command)
                {
                    case 0:
                        currentIndex = response.Length;
                        break;
                    case 7: 
                        var payload = new byte[payloadLen];
                        Array.Copy(response, currentIndex + 5, payload, 0, payloadLen);
                        result.Add(VersionsCell.Parse(payload));
                        currentIndex += 5 + payloadLen;
                        break;
                    case 8:  //fix length
                        var netInfoCell = NetInfoCell.Parse(response, currentIndex);
                        result.Add(netInfoCell);
                        currentIndex += netInfoCell.ToArray().Length;
                        break;
                    case 129:
                        var payload1 = new byte[payloadLen];
                        Array.Copy(response, currentIndex + 5, payload1, 0, payloadLen);
                        result.Add(CertsCell.Parse(payload1));
                        currentIndex += 5 + payloadLen;
                        break;
                    case 130:
                        var payload2 = new byte[payloadLen];
                        Array.Copy(response, currentIndex + 5, payload2, 0, payloadLen);
                        result.Add(ChallengeCell.Parse(payload2));
                        currentIndex += 5 + payloadLen;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(command.ToString(CultureInfo.InvariantCulture));
                }
                
                
            }

            return result;
        }

        private static short ReadUInt16(byte[] bytes, int index)
        {
            if (BitConverter.IsLittleEndian)
            {
                var first = bytes[index];
                var second = bytes[index + 1];
                var value = new byte[] {second, first};
                return BitConverter.ToInt16(value, 0);
            }
            else
            {
                return BitConverter.ToInt16(bytes, index);
            }
        }
    }
}
