using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Server.Networking;
using Server.Networking.Classes;

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
                Invoke(new ClientDelegate(_server_ClientDisconnected), new object[2] { client, type });
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
                Invoke(new ClientDelegate(_server_ClientConnected), new object[2] { client, type });
            }
            else
            {
                var lvi = new ListViewItem(new string[4]{client.Guid.ToString(),client.ConnectionDateTime.ToString(),client.LastPacketReceived.ToString(), client.LastPacketReceived.ToString()});
                lvConnections.Items.Add(lvi);
            }
          
        }
       
    }

    public enum Control
    {
        Add,
        Remove,
        Update
    }
}
