using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private Networking.ClientSide _s;
        public Client()
        {
            InitializeComponent();
        }

        private void Client_Load(object sender, EventArgs e)
        {
            _s = new Networking.ClientSide();
            _s.Connect(new IPEndPoint(IPAddress.Loopback, 33533));
        }
  
        private void btnSend_Click(object sender, EventArgs e)
        {
            _s.ClientSend(Sender.SendGuid(new Guid()));
   
        }
    }
}
