using System;
using System.IO;

namespace Client.Networking
{
    public class Sender
    {
        public  byte[] RequestGuid()
        {
            PacketWriter pw = new PacketWriter();

            pw.Write((ushort)MainHeaders.Initial);
            pw.Write((ushort)InitialHeaders.Guid);
           
            byte[] data = pw.GetBytes();
            int length = data.Length;

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] lengthBytes = BitConverter.GetBytes(length);
                ms.Write(lengthBytes, 0, lengthBytes.Length);
                ms.Write(data, 0, data.Length);

                return ms.ToArray();
            }


        }
    }
}
