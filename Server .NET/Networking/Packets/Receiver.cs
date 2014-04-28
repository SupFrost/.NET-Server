using Server.Networking.Classes;

namespace Server.Networking.Packets
{
    public class Receiver
    {
        private readonly Client _client;
        private readonly PacketReader _pr;
        private Sender _sender;

        public Receiver(Client client, byte[] data)
        {

            _client = client;
            _pr = new PacketReader(data);

        }

        public void HandlePacket()
        {
            var iOHeader = (IoHeader)_pr.ReadUshort();
            switch (iOHeader)
            {
                case IoHeader.Request:
                    var standardHeader = (StandardHeader)_pr.ReadUshort();
                    switch (standardHeader)
                    {
                        case StandardHeader.Guid:

                            // Send Guid back!
                            _sender = new Sender(_client);
                            _sender.SendGuid(_client.Guid);
                            break;
                        case StandardHeader.Ping:

                            //send Ping back!
                            _sender = new Sender(_client);
                            _sender.SendPing();
                            break;
                    }
                    break;
                case IoHeader.Send:
                    break;
            }
        }
    }
}
