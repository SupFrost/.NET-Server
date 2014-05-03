using System;
using System.Net.Sockets;

namespace Server.Networking
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
        public string Country { get; set; }
        public ushort Group { get; set; }


        public Client(Socket socket)
        {
            Socket = socket;
           Buffer = new byte[Socket.ReceiveBufferSize];
        }

    }

}
