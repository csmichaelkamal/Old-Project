using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Linkfire.PubSub.Desktop.Subscriber
{
    class Subscriber : ISubscriber
    {
        public async Task SubscribeToTopic(string topicName, Tuple<Socket, IPEndPoint> output)
        {
            var command = Enums.Command.Subscribe.ToString();

            var message = $"{command},{topicName}";

            var client = output.Item1;
            var remoteEndPoint = output.Item2;

            client.SendTo(Encoding.ASCII.GetBytes(message), remoteEndPoint);
            //if (!isReceivingStarted)
            //{
            //    isReceivingStarted = true;

            //    data = new byte[1024];

            //    var thread1 = new Thread(new ThreadStart(ReceiveDataFromServer))
            //    {
            //        IsBackground = true
            //    };

            //    thread1.Start();
            //}

        }

        public async Task UnsubscribeFromTopic(string topicName)
        {
            throw new System.NotImplementedException();
        }
    }
}
