﻿using ChatServer;
using ChatServer.Net.IO;
using System;
using System.Net;
using System.Net.Sockets;

namespace ChatrSever
{
    class Program {

        static List<Client> _users;
        static TcpListener _listener;
        static void Main(string[] args) {

            _users= new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"),7891);
            _listener.Start();

            while (true) {
                var client = new Client(_listener.AcceptTcpClient());
                _users.Add(client);

                BroadcastConnection();
            } 
        }
        static void BroadcastConnection() {
            foreach (var user in _users) {
                foreach (var item in _users) {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1);
                    broadcastPacket.WriteMessage(item.Username);
                    broadcastPacket.WriteMessage(item.UID.ToString());

                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            
            }
        }
    }
}