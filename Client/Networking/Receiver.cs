using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Networking
{
    class Receiver
    {
    private PacketReader _pr;
        
        public Receiver( byte[] data)
        {
            _pr = new PacketReader(data);
            }

        public void HandlePacket()
        {

            IoHeader ioHeader = (IoHeader)_pr.ReadUshort();
            StandardHeader standardHeader = (StandardHeader)_pr.ReadUshort();
                    switch (standardHeader)
                    {
                        case StandardHeader.Guid:
                            Guid guid = _pr.ReadGuid();
                            Console.WriteLine(guid.ToString());
                            break;
                        case StandardHeader.Ping:
                        {
                            if (ioHeader == IoHeader.Send)
                            {
                                //Subtracts the PingTime from the current time to get the Ping in ms.
                                Global.Ping = (ushort)DateTime.UtcNow.Subtract(Global.PingTime).Milliseconds;
                            }
                            break;
                        }
                    }
        }
    }
}
