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
                    case 7: 
                        var payload = new byte[payloadLen];
                        Array.Copy(response, currentIndex + 5, payload, 0, payloadLen);
                        result.Add(VersionsCell.Parse(payload));
                        break;
                    case 8:  //fix length
                        result.Add(new VersionsCell(new ushort[]{1}));
                        break;
                    case 129:
                        var payload1 = new byte[payloadLen];
                        Array.Copy(response, currentIndex + 5, payload1, 0, payloadLen);
                        result.Add(CertsCell.Parse(payload1));
                        //result.Add(ParseCertCell(response, currentIndex + 5, payloadLen));
                        break;
                    case 130:
                        result.Add(ParseChallengeCell(response, currentIndex + 5, payloadLen));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(command.ToString(CultureInfo.InvariantCulture));
                }
                currentIndex += 5 + payloadLen;
            }

            return result;
        }

        private static Cell ParseChallengeCell(byte[] certCellBytes, int index, int cellSize)
        {
            var challenge = new byte[32];
            Array.Copy(certCellBytes, index, challenge, 0, challenge.Length);
            var methodsCount = ReadUInt16(certCellBytes, index + challenge.Length);

            for (int i = 0; i < methodsCount; i++)
            {
                var method = ReadUInt16(certCellBytes, index + challenge.Length + 2 + 2 * i);
            }

            return new VersionsCell(new ushort[]{1});
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
