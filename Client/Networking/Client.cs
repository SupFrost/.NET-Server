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
        private byte[] _buffer;
        public Client()
        {
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }

        public void Connect(IPEndPoint ip)
        {
            try
            {
                _clientSocket.BeginConnect(ip, ConnectCallback, null);
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void ConnectCallback(IAsyncResult AR)
        {
            _clientSocket.EndConnect(AR);
            _clientSocket.BeginReceive(_buffer, 0, sizeof(int), SocketFlags.None, ReceiveCallback, null);


            //Initilize Connection

        }

        void ReceiveCallback(IAsyncResult AR)
        {
            int length = BitConverter.ToInt32(_buffer, 0);
            int received = 0;
            

             while (received < length)
             {
                 if (length < _clientSocket.ReceiveBufferSize)
                 {
                     _clientSocket.Receive(_buffer, received, length, SocketFlags.None);
                 }
                 else{_clientSocket.Receive(_buffer, received, _clientSocket.ReceiveBufferSize, SocketFlags.None);}
                 }

            // Handle _buffer with class.
           
        }
          

        public void Send(byte[] data)
        {
            try
            {
                _clientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallback, null);
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

        public void 

    }
}
