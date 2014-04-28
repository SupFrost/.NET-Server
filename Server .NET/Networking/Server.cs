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
    public class Server
    {

        private Socket _serverSocket;
       public static Dictionary<Guid, Client> LstClients;

        public void Start()
        {


            LstClients = new Dictionary<Guid, Client>();

            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 33533));
            _serverSocket.Listen(1);
            _serverSocket.BeginAccept(AcceptCallback, _serverSocket);

        }

        private void AcceptCallback(IAsyncResult AR)
        {
            Client client = null;
            try
            {
                Socket s = _serverSocket.EndAccept(AR);

                //Add to Client list
                client = new Client(s);
      client.ConnectionDateTime = DateTime.UtcNow;
                client.Guid = Guid.NewGuid();
                

                Console.WriteLine("Client connected : " + client.Guid);

                lock (LstClients)
                    LstClients.Add(client.Guid, client);

                
                _serverSocket.BeginAccept(AcceptCallback, _serverSocket);
                client.Socket.BeginReceive(client.Buffer, 0, sizeof (int), SocketFlags.None, ReceiveCallback, client);

                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (client.Socket != null)
                {
                    client.Socket.Close();
                    lock (LstClients)
                        LstClients.Remove(client.Guid);
                }
            }

        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            Client client = null;
            try
            {
                client = (Client) AR.AsyncState;

                //update Client
                LstClients[client.Guid].LastPacketReceived = DateTime.UtcNow;


                int PacketLength = BitConverter.ToInt32(client.Buffer, 0);
                client.Buffer = new byte[PacketLength];

                int received = 0;

                while (received < PacketLength)
                {
                    if (PacketLength < client.Socket.ReceiveBufferSize)
                    {
                        client.Socket.Receive(client.Buffer, received, PacketLength, SocketFlags.None);
                    }
                    else
                    {
                        client.Socket.Receive(client.Buffer, received, client.Socket.ReceiveBufferSize, SocketFlags.None);
                    }

                    received = client.Buffer.Length;
                }

                //Handling the packet!
                Receiver receiver = new Receiver(client, client.Buffer);
                receiver.HandlePacket();


                Console.WriteLine(client.LastPacketReceived);



                //Start receiving again!
                client.Socket.BeginReceive(client.Buffer, 0, client.Buffer.Length, SocketFlags.None, ReceiveCallback,client);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (client.Socket != null)
                {
                    client.Socket.Close();
                    lock (LstClients)
                        LstClients.Remove(client.Guid);
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
            Client client = (Client)AR.AsyncState;
            byte[] data = client.Data;

            client.Socket.Send(data, data.Length, SocketFlags.None);

        }


    }
}