using System;

namespace Core.Protocol
{
    public abstract class Cell
    {
        private readonly byte commandType;

        protected Cell(byte commandType)
        {
            this.commandType = commandType;
        }

        protected abstract byte[] GetPayload();
        
        public byte[] ToArray()
        {
            var payload = GetPayload();

            if(payload.Length > CellCommands.CELL_PAYLOAD_SIZE)
            {
                var message = string.Format("Payload size exceeds, max value '{0}', actual '{1}'",
                                            CellCommands.CELL_PAYLOAD_SIZE, payload.Length);
                throw new ArgumentOutOfRangeException(message);
            }

            var result = new byte[CellCommands.CELL_TOTAL_SIZE];

            result[1] = 7; // TODO: CircId should be choosed correctly
            result[CellCommands.CELL_COMMAND_POS] = commandType;
            Array.Copy(payload, 0, result, CellCommands.CELL_PAYLOAD_POS, payload.Length);
            
            return result;
        }
    }
}
