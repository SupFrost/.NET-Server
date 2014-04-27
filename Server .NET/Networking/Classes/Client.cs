using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Networking.Classes
{
    public class Client
    {
        public Socket Socket { get; set; }
        public DateTime ConnectionDateTime { get; set; }
        public Guid Guid { get; set; }
        public DateTime LastPacketReceived { get; set; }
        public byte[] Data { get; set; }


        public Client()
        {

        }





    }
}
