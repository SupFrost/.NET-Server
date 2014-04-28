﻿using System;
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
        public TimeSpan Ping { get; set; }
        public byte[] Buffer { get; set; }


        public Client(Socket socket)
        {
            Socket = socket;
           Buffer = new byte[Socket.ReceiveBufferSize];
        }

    }
}
