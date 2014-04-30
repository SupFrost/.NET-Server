using System;
using System.IO;

namespace Server.Networking.Packets
{
    public class Sender
    {
        private readonly Client _client;
        private PacketWriter _pw;
        private readonly  Server _server;
        public Sender(Server server, Client client)
        {
            _client = client;
            _server = server;
        }

        public void SendGuid(Guid guid)
        {
            _pw = new PacketWriter();

            _pw.Write((ushort)IoHeader.Send);
            _pw.Write((ushort)StandardHeader.Guid);
            _pw.Write(guid);

            byte[] data = _pw.GetBytes();

            using (var ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                _server.ServerSend(_client, ms.ToArray());
            }
        }
        public void SendPing()
        {
            _pw = new PacketWriter();

            _pw.Write((ushort) IoHeader.Send);
            _pw.Write((ushort) StandardHeader.Ping);

            byte[] data = _pw.GetBytes();

            using (var ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                _server.ServerSend(_client, ms.ToArray());
            }
        }
     
    }
}
