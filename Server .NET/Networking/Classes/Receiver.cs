using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Networking.Classes
{
    public class Receiver
    {
        private Client _client;
        private PacketReader pr;
        
        public Receiver(Client client, byte[] data)
         {

             _client = client;
        pr = new PacketReader(data);

         }

        public void HandlePacket()
        {


            MainHeaders mainHeader = (MainHeaders) pr.ReadInt16();
            switch (mainHeader)
            {
                case MainHeaders.Initial:
                    InitialHeaders initialHeader = (InitialHeaders) pr.ReadInt16();
                    switch (initialHeader)
                    {
                            case InitialHeaders.Guid:

                            // Send Guid back!
                            Sender s = new Sender(_client);
                            s.SendGuid(_client.Guid);

                           
                            break;
                    }
                    break;
                case MainHeaders.Text:
                    break;
            }
        }

    }
}
