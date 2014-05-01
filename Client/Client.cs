using System;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Windows.Forms;
using Client.Networking;
using Client.Networking.Packets;

namespace Client
{
    public partial class Client : Form
    {
        private ClientSide _client;
        private Sender _sender;

        public delegate void ClientDelegate(EventType type);
        public Client()
        {
            InitializeComponent();
        }

        private void Client_Load(object sender, EventArgs e)
        {
         
        }

        void _client_Disconnected(EventType type)
        {
            if (InvokeRequired)
            {
                Invoke(new ClientDelegate(_client_Disconnected), type);
            }
            else
            {
                   BackColor = Color.DarkRed;
            btnDisconnect.Enabled = false;
            btnConnect.Enabled = true;
                tmrPing.Stop();
            }
         
        }

        void _client_Connected(EventType type)
        {
            if (InvokeRequired)
            {
                Invoke(new ClientDelegate(_client_Connected), type);
            }
            else
            {
                BackColor = Color.ForestGreen;
                btnDisconnect.Enabled = true;
                btnConnect.Enabled = false;
                tmrPing.Start();
            }
          

        }

        private void btnSend_Click(object sender, EventArgs e)
        {

            _sender = new Sender();

            _client.ClientSend(_sender.RequestGuid());
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
           _client = new ClientSide();

            if (!_client.Connect(new IPEndPoint(IPAddress.Loopback, 33533)))
                return;

            _client.Connected += _client_Connected;
            _client.Disconnected += _client_Disconnected;

            //Request GUID from server
            _sender = new Sender();
            _client.ClientSend(_sender.RequestGuid());

            //Enable ping timer :)
            tmrPing.Enabled = true;
          
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

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            _client.Disconnect();
            _client.Dispose();
           }
    }
}
