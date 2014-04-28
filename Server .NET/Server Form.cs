using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Server.Networking.Classes;

namespace Server
{
    public partial class ServerForm : Form
    {
        private Networking.Server _server;


        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            _server = new Networking.Server();
            _server.Start();

            timer1.Enabled = true;
            timer1.Interval = 1000;
            //timer1.Start();

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
               

            }
            finally
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
 lvClients.Items.Clear();
                foreach (KeyValuePair<Guid, Client> pair in Networking.Server.LstClients)
                {
                    lvClients.Items.Add(pair.Key.ToString());

                }
        }
    }
}
