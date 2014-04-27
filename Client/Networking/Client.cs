using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Networking
{
    class Client
    {
        private Socket _clientSocket;
        public Client()
        {
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }

        public void Connect(IPEndPoint IP)
        {
            try
            {
                _clientSocket.BeginConnect(IP, new AsyncCallback(ConnectCallback), null);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void ConnectCallback(IAsyncResult AR)
        {
            _clientSocket.EndConnect(AR);

            //Initilize Connection

        }

        public void Send(byte[] data)
        {
            try
            {
                _clientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        void SendCallback(IAsyncResult AR)
        {
            _clientSocket.EndSend(AR);

        }

    }
}
