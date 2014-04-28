using System;

namespace Client.Networking.Packets
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
                            Global.Guid = _pr.ReadGuid();
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
