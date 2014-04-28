using System;
using System.IO;

namespace Client.Networking
{
    public class Sender
    {
        private PacketWriter _pw;

        public Sender()
        {
            _pw = new PacketWriter();
        }

        public  byte[] RequestGuid()
        {
            _pw.Write((ushort)IoHeader.Request);
            _pw.Write((ushort)StandardHeader.Guid);

            byte[] data = _pw.GetBytes();

            using (var ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                return ms.ToArray();
            }
           }
        public byte[] RequestPing()
        {
            _pw = new PacketWriter();

            _pw.Write((ushort)IoHeader.Request);
            _pw.Write((ushort)StandardHeader.Ping);

            byte[] data = _pw.GetBytes();

            using (var ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);

                Global.PingTime = DateTime.UtcNow;
                return ms.ToArray();
            }

            
        }
    }
}
