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
    class ClientSide
    {
        private Socket _clientSocket;
        private  byte[] _buffer;

        public ClientSide()
        {
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _buffer = new byte[_clientSocket.ReceiveBufferSize];

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
            _clientSocket.BeginReceive(_buffer, 0,sizeof(int), SocketFlags.None, ReceiveCallback, null);


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
                else { _clientSocket.Receive(_buffer, received, _clientSocket.ReceiveBufferSize, SocketFlags.None); }

                received = _buffer.Length;
            }

            Receiver receiver = new Receiver(_buffer);
            receiver.HandlePacket();

            _clientSocket.BeginReceive(_buffer, 0, sizeof(int), SocketFlags.None, ReceiveCallback, null);
        }


        public void ClientSend(byte[] data)
        {
            try
            {
                byte[] dataLength = BitConverter.GetBytes(data.Length);
                _clientSocket.BeginSend(dataLength, 0, dataLength.Length, SocketFlags.None, SendCallback, data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        void SendCallback(IAsyncResult AR)
        {
            try
            {
                byte[] data = (byte[])AR.AsyncState;
                _clientSocket.Send(data, 0, data.Length, SocketFlags.None);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }



    }
}
