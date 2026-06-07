using LiteNetLib;
using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace TServer
{
    public class NetworkServer : INetEventListener
    {
        private NetManager _netManager;
        private Dictionary<int, NetPeer> connectionsDic;

        #region public methods
        public void Start()
        {
            _netManager = new NetManager(this)
            {
                DisconnectTimeout = 100000
            };
            _netManager.Start(9050);
            Console.WriteLine("Server started on port 9050");
        }
        public void PollEvents()
        {
            _netManager.PollEvents();
        }
        #endregion
        #region INetEventListener implementation
        public void OnConnectionRequest(ConnectionRequest request)
        {
            Console.WriteLine("Connection request from " + request.RemoteEndPoint);
            request.Accept();
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            var data = Encoding.UTF8.GetString(reader.RawData);
            Console.WriteLine($"Received data from {peer.Address}:{peer.Port}: {data}");
        }

        public void OnPeerConnected(NetPeer peer)
        {
            Console.WriteLine($"Client Peer connected:{peer.Address}:{peer.Port} Id:{peer.Id}");
            connectionsDic.Add(peer.Id, peer);
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Console.WriteLine($"Client Peer disConnected:{peer.Address}:{peer.Port} Id:{peer.Id}");
            connectionsDic.Remove(peer.Id);
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            throw new NotImplementedException();
        }
        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            throw new NotImplementedException();
        }
        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
