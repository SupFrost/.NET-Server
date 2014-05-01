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
        private ListView.ListViewItemCollection lvic;
        public ServerForm()
        {
            InitializeComponent();
            lvic = new ListView.ListViewItemCollection(lvConnections);
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            _server = new Networking.Server();
            _server.Start();

            _server.ClientConnected += _server_ClientConnected;
            _server.ClientDisconnected += _server_ClientDisconnected;
            _server.ClientUpdated += _server_ClientUpdated;

        }

        void _server_ClientUpdated(Client client, ClientEventType type)
        {
            if (InvokeRequired)
            {
                Invoke(new ClientDelegate(_server_ClientUpdated), new object[] {client, type});
            }
            else
            {
                switch (type)
                {
                    case ClientEventType.UpdatedCountry:
                        lvic[0].SubItems[3].Text = client.Country;
                        break;
                }
            }

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
                lvic.Add(client.Guid.ToString(), client.Guid.ToString()).SubItems.AddRange(new[] { client.ConnectionDateTime.ToString(), client.LastPacketReceived.ToString(),"" });


                //lvConnections.Items.Add(client.Guid.ToString(), client.Guid.ToString());
                //var lvi = lvConnections.Items[client.Guid.ToString()].SubItems.Add(client.ConnectionDateTime.ToString());
                //var lvi2 = lvConnections.Items[client.Guid.ToString()].SubItems.Add(client.LastPacketReceived.ToString());
            }
        }
    }
}
