using System;
using System.Globalization;
using System.Windows.Forms;
using Server.Networking;

namespace Server
{
    public partial class ServerForm : Form
    {
        public delegate void ClientDelegate(Client client, ClientEventType type);
        private Networking.Server _server;
        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            _server = new Networking.Server();
            _server.Start();

            _server.ClientConnected += _server_ClientConnected;
            _server.ClientDisconnected += _server_ClientDisconnected;
        }

        void _server_ClientDisconnected(Client client, ClientEventType type)
        {
            if (InvokeRequired)
            {
                Invoke(new ClientDelegate(_server_ClientDisconnected), new object[] { client, type });
            }
            else
            {
                lvConnections.Items.Remove(lvConnections.FindItemWithText(client.Guid.ToString()));
            }
        }

        void _server_ClientConnected(Client client, ClientEventType type)
        {
            if (InvokeRequired)
            {
                Invoke(new ClientDelegate(_server_ClientConnected), new object[] { client, type });
            }
            else
            {
                var lvi = new ListViewItem(new[]{client.Guid.ToString(),client.ConnectionDateTime.ToString(CultureInfo.InvariantCulture),client.LastPacketReceived.ToString(CultureInfo.InvariantCulture), client.LastPacketReceived.ToString(CultureInfo.InvariantCulture)});
                lvConnections.Items.Add(lvi);
            }
        }
    }
}
