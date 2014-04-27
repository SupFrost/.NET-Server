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

namespace Client
{
    public partial class Client : Form
    {
        private Networking.Client _s;
        public Client()
        {
            InitializeComponent();
        }

        private void Client_Load(object sender, EventArgs e)
        {
            _s = new Networking.Client();
            _s.Connect(new IPEndPoint(IPAddress.Loopback, 33533));
        }
  
        private void btnSend_Click(object sender, EventArgs e)
        {
     _s.Send(new byte[1]);
        }
    }
}
