using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.Networking;
using Client.Networking.Packets;

namespace Client
{
    public partial class Client : Form
    {
        private ClientSide _client;
        private Sender _sender;
        public Client()
        {
            InitializeComponent();
        }

        private void Client_Load(object sender, EventArgs e)
        {
            _client = new ClientSide();
            if (!_client.Connect(new IPEndPoint(IPAddress.Loopback, 33533)))
                return;


            //Request GUID from server
            _sender = new Sender();
            _client.ClientSend(_sender.RequestGuid());

            //Enable ping timer :)
            tmrPing.Enabled = true;
            tmrPing.Start();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

            _sender = new Sender();

            _client.ClientSend(_sender.RequestGuid());
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

            if (!_client.Connect(new IPEndPoint(IPAddress.Loopback, 33533)))
                return;


            //Request GUID from server
            _sender = new Sender();
            _client.ClientSend(_sender.RequestGuid());

            //Enable ping timer :)
            tmrPing.Enabled = true;
            tmrPing.Start();

        }

        private void tmrPing_Tick(object sender, EventArgs e)
        {
            //Request new ping time!
            _sender = new Sender();
            _client.ClientSend(_sender.RequestPing());

            //Update the controls!
            if (Global.Ping == 0)
            { lblPing.Text = @"< 1 ms"; }
            else
            {
                lblPing.Text = Global.Ping.ToString(CultureInfo.InvariantCulture);
            }

            lblGUID.Text = Global.Guid.ToString();

        }
    }
}
