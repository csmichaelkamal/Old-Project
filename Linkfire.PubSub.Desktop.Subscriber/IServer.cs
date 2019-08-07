using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Linkfire.PubSub.Desktop.Subscriber
{
    interface IServer
    {
        Task<Tuple<Socket, IPEndPoint>> InitServer(string serverIP, int serverPort);

        Task ReceiveDataFromServer();
    }
}
