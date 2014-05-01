﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using Client.Networking.Packets;

namespace Client.Networking
{
    class ClientSide
    {
        private  Socket _clientSocket;
        private readonly byte[] _buffer;
        public delegate void ClientEventHandler(EventType type);

        public event ClientEventHandler Connected;
        public event ClientEventHandler Disconnected;
        public event ClientEventHandler Error;

        public ClientSide()
        {
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _buffer = new byte[_clientSocket.ReceiveBufferSize];
        }

        public Boolean Connect(IPEndPoint ip)
        {
            try
            {
                if (Global.Connected)
                    return false;

                if( _clientSocket == null)
                    _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                _clientSocket.BeginConnect(ip, ConnectCallback, null);
                return true;
            }
            catch (Exception ex)
            {
                Global.Connected = false;
                Error.Invoke(EventType.Error);
                Disconnected.Invoke(EventType.Disconnected);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool Disconnect()
        {
            if (Global.Connected)
            {
                _clientSocket.Close();
                _clientSocket.Dispose();
                Disconnected.Invoke(EventType.Disconnected);
             }

            return true;
        }

        void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Global.Connected = true;
            _clientSocket.EndConnect(ar);
                Connected.Invoke(EventType.Connected);
            _clientSocket.BeginReceive(_buffer, 0, sizeof(int), SocketFlags.None, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                Global.Connected = false;
                Error.Invoke(EventType.Error);
                Disconnected(EventType.Disconnected);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void ReceiveCallback(IAsyncResult ar)
        {
            try
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

                Receiver receiver = new Receiver(this ,_buffer);
                receiver.HandlePacket();

                _clientSocket.BeginReceive(_buffer, 0, sizeof(int), SocketFlags.None, ReceiveCallback, null);
            }
            catch (SocketException socketException)
            {
                MessageBox.Show(socketException.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

        }
        
        public void ClientSend(byte[] data)
        {
            try
            {
                while (!Global.Connected)
                {
                    Application.DoEvents();
                }
                byte[] dataLength = BitConverter.GetBytes(data.Length);
                _clientSocket.BeginSend(dataLength, 0, dataLength.Length, SocketFlags.None, SendCallback, data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void SendCallback(IAsyncResult ar)
        {
            try
            {
                byte[] data = (byte[])ar.AsyncState;
                _clientSocket.Send(data, 0, data.Length, SocketFlags.None);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }



    }

    public enum EventType
    {
      Connected,
        Disconnected,
        Error
    }
}
