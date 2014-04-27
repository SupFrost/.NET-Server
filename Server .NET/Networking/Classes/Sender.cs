using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Networking;

namespace Server.Networking.Classes
{
    public class Sender
    {
        private Client _client;
        public Sender(Client client)
        {
            _client = client;
        }

        public void SendGuid(Guid guid)
        {
            PacketWriter pw = new PacketWriter();

            pw.Write((ushort)MainHeaders.Initial);
            pw.Write((ushort)InitialHeaders.Guid);
            pw.Write(guid);

            byte[] data = pw.GetBytes();
            int length = data.Length;

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);

                Server.ServerSend(_client, ms.ToArray());
            }


        }
    }
}
