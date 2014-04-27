using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Networking
{
    class Receiver
    {
    private PacketReader pr;
        public Receiver( byte[] data)
        {
            pr = new PacketReader(data);
            }

        public void HandlePacket()
        {
            
            MainHeaders mainHeader = (MainHeaders)pr.ReadUshort();
            switch (mainHeader)
            {
                case MainHeaders.Initial:
                    InitialHeaders initialHeader = (InitialHeaders)pr.ReadUshort();
                    switch (initialHeader)
                    {
                        case InitialHeaders.Guid:
                            Guid guid = pr.ReadGuid();
                            Console.WriteLine(guid.ToString());
                            break;
                    }
                    break;
                case MainHeaders.Text:
                    break;
            }
        }
    }
}
