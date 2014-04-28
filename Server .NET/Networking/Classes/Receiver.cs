﻿using System;
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
        private PacketReader _pr;
        private Sender _sender;

        public Receiver(Client client, byte[] data)
        {

            _client = client;
            _pr = new PacketReader(data);

        }

        public void HandlePacket()
        {
            IoHeader iOHeader = (IoHeader)_pr.ReadUshort();
            switch (iOHeader)
            {
                case IoHeader.Request:
                    StandardHeader standardHeader = (StandardHeader)_pr.ReadUshort();
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
