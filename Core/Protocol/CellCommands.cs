namespace Core.Protocol
{
    public static class CellCommands
    {
        public static byte CELL_PADDING = 0;
        public static byte CELL_CREATE = 1;
        public static byte CELL_CREATED = 2;
        public static byte CELL_RELAY = 3;
        public static byte CELL_DESTROY = 4;
        public static byte CELL_CREATE_FAST = 5;
        public static byte CELL_CREATED_FAST = 6;

        public static int CELL_TOTAL_SIZE = 512;
        public static int CELL_CIRCID_SIZE = 2;
        public static int CELL_COMMAND_SIZE = 1;
        public static int CELL_PAYLOAD_SIZE = 509;
        public static int CELL_CIRCID_POS = 0;
        public static int CELL_COMMAND_POS = CELL_CIRCID_POS + CELL_CIRCID_SIZE;
        public static int CELL_PAYLOAD_POS = CELL_COMMAND_POS + CELL_COMMAND_SIZE;
    }
}