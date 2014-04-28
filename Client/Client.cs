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

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            _sender = new Sender();

            _client.ClientSend(_sender.RequestGuid());

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            _client = new ClientSide();
            _client.Connect(new IPEndPoint(IPAddress.Loopback, 33533));


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
            lblPing.Text = Global.Ping.ToString(CultureInfo.InvariantCulture);
            lblGUID.Text = Global.Guid.ToString();

        }
    }
}
