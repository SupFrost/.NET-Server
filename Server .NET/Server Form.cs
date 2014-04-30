using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Server.Networking.Classes;

namespace Server
{
    public partial class ServerForm : Form
    {
        private Timer tmrTimer;
        private Networking.Server _server;
        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            _server = new Networking.Server();
            _server.Start();

            tmrTimer = new Timer();
            tmrTimer.Interval = 1000;
            tmrTimer.Tick += UpdateListview;
            tmrTimer.Start();
        }

        private void UpdateListview(object sender, EventArgs e)
        {

            lvConnections.Items.Clear();

            foreach (KeyValuePair<Guid, Client> pair in Networking.Server.LstClients)
            {
                Client client = pair.Value;
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
