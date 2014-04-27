using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using Server.Networking.Classes;

namespace Server.Networking
{
     public static class Server
    {
        private static Socket _serverSocket;
        private static byte[] _buffer;
        private static Dictionary<Guid, Client> _lstClients;

        public static void Start()
        {

            _lstClients = new Dictionary<Guid, Client>();

            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 33533));
            _serverSocket.Listen(1);
            _serverSocket.BeginAccept(AcceptCallback, _serverSocket);

        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            Client client = new Client();
            try
            {
                Socket s = _serverSocket.EndAccept(AR);

                //Add to Client list
                client = new Client();
                client.Socket = s;
                client.ConnectionDateTime = DateTime.UtcNow;
                client.Guid = Guid.NewGuid();

                Console.WriteLine("Client connected : " + client.Guid);

                lock (_lstClients)
                    _lstClients.Add(client.Guid, client);


                _buffer = new byte[client.Socket.ReceiveBufferSize];
                client.Socket.BeginReceive(_buffer, 0, sizeof(int), SocketFlags.None,ReceiveCallback, client);

                _serverSocket.BeginAccept(AcceptCallback, _serverSocket);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (client.Socket != null)
                {
                    client.Socket.Close();
                    lock (_lstClients)
                        _lstClients.Remove(client.Guid);
                }
            }

        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Client client = new Client();
            try
            {
                client = (Client)AR.AsyncState;

                //update Client
                _lstClients[client.Guid].LastPacketReceived = DateTime.UtcNow;


                int PacketLength = BitConverter.ToInt32(_buffer,0);
                _buffer = new byte[PacketLength];

                int received = 0;

                while (received < PacketLength)
                {
                    if (PacketLength < client.Socket.ReceiveBufferSize)
                    {
                        client.Socket.Receive(_buffer, received, PacketLength, SocketFlags.None);
                    }
                    else { client.Socket.Receive(_buffer, received, client.Socket.ReceiveBufferSize, SocketFlags.None); }

                    received = _buffer.Length;
                }

                //Handling the packet!
                Receiver receiver = new Receiver(client, _buffer);
                receiver.HandlePacket();
               

               Console.WriteLine(client.LastPacketReceived);



                //Start receiving again!
                client.Socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None,ReceiveCallback, client);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (client.Socket != null)
                {
                    client.Socket.Close();
                    lock (_lstClients)
                        _lstClients.Remove(client.Guid);
                }
            }
        }

         public static void ServerSend(Client client, byte[] data)
        {
            try
            {
                byte[] dataLength = BitConverter.GetBytes(data.Length);
                client.Data = data;

                client.Socket.BeginSend(dataLength, 0, dataLength.Length, SocketFlags.None, SendCallback, client);
            }
            catch (SocketException socketException)
            {
                MessageBox.Show(socketException.ErrorCode.ToString());
            }

       

        }

        private static void SendCallback(IAsyncResult AR)
        {
            Client client = (Client) AR.AsyncState;
            byte[] data = client.Data;

            client.Socket.Send(data, data.Length, SocketFlags.None);

        }


    }
}