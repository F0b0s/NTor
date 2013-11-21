using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Documents
{
    public class NetworkEntity
    {
        public NetworkEntity(string name, string address, string port)
        {
            Name = name;
            Address = address;
            Port = port;
        }
        
        public string Name { get; set; }
        public string Address { get; set; }
        public string Port { get; set; }
    }

    public class RouterDescriptor
    {
        public NetworkEntity NetworkEntity { get; set; }

        public string FingerPrint { get; set; }
        public string OnionKey { get; set; }
        public string NTorOnionKey { get; set; }
        public string SigningKey { get; set; }




    }
}
