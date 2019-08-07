using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Linkfire.PubSub.Desktop.Subscriber
{
    interface ISubscriber
    {
        Task SubscribeToTopic(string topicName, Tuple<Socket, IPEndPoint> output);

        Task UnsubscribeFromTopic(string topicName);
    }
}
