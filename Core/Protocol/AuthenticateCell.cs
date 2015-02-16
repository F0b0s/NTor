using System;
using System.Text;

namespace Core.Protocol
{
    public class AuthenticateCell : Cell
    {
        public AuthenticateCell(byte commandType) : base(commandType)
        {

        }

        protected override byte[] GetPayload()
        {
            var bytes = Encoding.UTF8.GetBytes("AUTH0001");

            throw new NotImplementedException();
        }
    }
}
