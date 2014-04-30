using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Server.Networking.Classes;
using Server.Networking.Packets;

namespace Server.Networking
{
    public class Server
    {

        private Socket _serverSocket;
        public static Dictionary<Guid, Client> LstClients;

        public delegate void ClientEventHandler(Client client, ClientEventType type);
        public event ClientEventHandler ClientConnected;
        public event ClientEventHandler ClientDisconnected;

        public void Start()
        {
            LstClients = new Dictionary<Guid, Client>();

            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 33533));
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(AcceptCallback, _serverSocket);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            Socket s = _serverSocket.EndAccept(ar);
            var client = new Client(s);
            try
            {
                

                //Add to Client list
                
                client.ConnectionDateTime = DateTime.UtcNow;
                client.Guid = Guid.NewGuid();


                Console.WriteLine(@"Client connected : {0}", client.Guid);

                lock (LstClients)
                    LstClients.Add(client.Guid, client);
                ClientConnected.Invoke(client, ClientEventType.Connected);


                _serverSocket.BeginAccept(AcceptCallback, _serverSocket);
                client.Socket.BeginReceive(client.Buffer, 0, sizeof (int), SocketFlags.None, ReceiveCallback, client);

            }
            catch (Exception)
            {
                if (client.Socket != null)
                {
                    client.Socket.Close();
                    lock (LstClients)
                        LstClients.Remove(client.Guid);
                    ClientDisconnected.Invoke(client, ClientEventType.Disconnected);
                }
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var client = (Client)ar.AsyncState;

            //update Client
            LstClients[client.Guid].LastPacketReceived = DateTime.UtcNow;
            if (client.Buffer.Length == 0)
            {
                return;
            }
            try
            {
                int packetLength = BitConverter.ToInt32(client.Buffer, 0);
                client.Buffer = new byte[packetLength];

                int received = 0;

                while (received < packetLength)
                {
                    if (packetLength < client.Socket.ReceiveBufferSize)
                    {
                        client.Socket.Receive(client.Buffer, received, packetLength, SocketFlags.None);
                    }
                    else
                    {
                        client.Socket.Receive(client.Buffer, received, client.Socket.ReceiveBufferSize, SocketFlags.None);
                    }

                    received = client.Buffer.Length;
                }

                //Handling the packet!
                var receiver = new Receiver(this, client, client.Buffer);
                receiver.HandlePacket();


                Console.WriteLine(client.LastPacketReceived);
                client.Socket.BeginReceive(client.Buffer, 0, client.Buffer.Length, SocketFlags.None, ReceiveCallback,
                    client);
            }
            catch (SocketException)
            {
                if (client.Socket != null)
                {
                    client.Socket.Close();
                    Console.WriteLine(@"Client disconnected: {0}", client.Guid);
                    lock (LstClients)
                        LstClients.Remove(client.Guid);
                    ClientDisconnected.Invoke(client, ClientEventType.Disconnected);
                }

            }
            catch (ObjectDisposedException)
            {
                if (client.Socket != null)
                {
                    client.Socket.Close();
                    Console.WriteLine(@"Client disconnected: {0}", client.Guid);
                    lock (LstClients)
                        LstClients.Remove(client.Guid);
                    ClientDisconnected.Invoke(client, ClientEventType.Disconnected);
                }
            }


        }

        public void ServerSend(Client client, byte[] data)
        {
            try
            {
            byte[] dataLength = BitConverter.GetBytes(data.Length);
            client.Data = data;

            client.Socket.BeginSend(dataLength, 0, dataLength.Length, SocketFlags.None, SendCallback, client);
            }
            catch (SocketException)
            {
                if (client.Socket != null)
                {
                    client.Socket.Close();
                    Console.WriteLine(@"Client disconnected: {0}", client.Guid);
                    lock (LstClients)
                        LstClients.Remove(client.Guid);
                    ClientDisconnected.Invoke(client, ClientEventType.Disconnected);
                }

            }



        }

        private static void SendCallback(IAsyncResult ar)
        {
            Client client = (Client)ar.AsyncState;
            byte[] data = client.Data;

            client.Socket.Send(data, data.Length, SocketFlags.None);


        }


    }

    public enum ClientEventType
    {
        Connected,
        Disconnected
    }
}