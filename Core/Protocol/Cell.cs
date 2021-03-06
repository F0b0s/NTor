﻿using System;

namespace Core.Protocol
{
    public abstract class Cell
    {
        private readonly byte _commandType;

        protected Cell(byte commandType)
        {
            _commandType = commandType;
        }

        protected abstract byte[] GetPayload();
        
        public byte[] ToArray()
        {
            var payload = GetPayload();

            if (payload.Length > CellCommands.CELL_PAYLOAD_SIZE)
            {
                var message = string.Format("Payload size exceeds, max value '{0}', actual '{1}'",
                    CellCommands.CELL_PAYLOAD_SIZE, payload.Length);
                throw new ArgumentOutOfRangeException(message);
            }

            var result = new byte[5 + payload.Length];

            result[0] = 0;
            result[1] = 0;
            result[2] = _commandType;
            var payloadSize = BitConverter.GetBytes((ushort) payload.Length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(payloadSize);
            }

            Array.Copy(payloadSize, 0, result, 3, payloadSize.Length);
            Array.Copy(payload, 0, result, 5, payload.Length);
            
            return result;
        }
    }
}
