using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server.Networking.Classes;

namespace Server
{
    public partial class ServerForm : Form
    {
        private Server.Networking.Server _server;
    

        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {

           _server = new Networking.Server();
           _server.Start();

            _server.Connected += UpdateClients;
            
        }
        [STAThread]
        private void UpdateClients(Client client)
        {
    ListViewItem lvi = new ListViewItem();
            lvi.SubItems.Add(client.Guid.ToString());
            lvi.SubItems.Add(client.ConnectionDateTime.ToString());

           // lvClients.Items.Add(lvi);
            {
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lvClients.Items.Clear();
            foreach(KeyValuePair<Guid,Client> pair in Networking.Server.LstClients)
            {
                lvClients.Items.Add(pair.Key.ToString());

            }
        }
    }
}
