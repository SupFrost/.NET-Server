using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Client.Networking.Packets
{
    class Receiver
    {
        private PacketReader _pr;
        private ClientSide _client;

        public Receiver(ClientSide client, byte[] data)
        {
            _pr = new PacketReader(data);
            _client = client;
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
                case StandardHeader.Country:
                    {
                        if (ioHeader == IoHeader.Request)
                        {
                            if (Global.Country == null)
                            {
                                object obj = new object();

                                string countryName = new WebClient().DownloadString("http://api.hostip.info/country.php");

                                Global.Country = countryName;
                            }

                            var s = new Sender();
                            _client.ClientSend(s.SendCountry(Global.Country));
                        }


                        break;
                    }
            }
        }
    }
}
