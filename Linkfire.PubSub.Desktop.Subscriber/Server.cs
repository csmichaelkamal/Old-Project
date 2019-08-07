using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Linkfire.PubSub.Desktop.Subscriber
{
    class Server : IServer
    {
        public async Task<Tuple<Socket, IPEndPoint>> InitServer(string serverIP = "127.0.0.1", int serverPort = 10002)
        {
            if (IPAddress.TryParse(serverIP, out IPAddress serverIPAddress))
            {
                var client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                var remoteEndPoint = new IPEndPoint(serverIPAddress, serverPort);

                return new Tuple<Socket, IPEndPoint>(client, remoteEndPoint);
            }

            return new Tuple<Socket, IPEndPoint>(null, null);
        }

        public Task ReceiveDataFromServer()
        {
            throw new NotImplementedException();
        }
    }
}
