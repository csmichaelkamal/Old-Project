using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Linkfire.PubSub.MessagingServer
{
    public class SubscriberService
    {
        public void StartSubscriberService()
        {
            var th = new Thread(new ThreadStart(HostSubscriberService))
            {
                // IsBackground = true
            };
            th.Start();
        }

        private void HostSubscriberService()
        {
            var ipV4 = IPAddress.Parse("127.0.0.1");
            var localEP = new IPEndPoint(ipV4, 10002);

            var server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(localEP);

            StartListening(server);
        }

        private static void StartListening(Socket server)
        {
            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            var recv = 0;
            var data = new byte[1024];

            while (true)
            {
                recv = 0;
                data = new byte[1024];
                recv = server.ReceiveFrom(data, ref remoteEP);

                Console.WriteLine($"Subscriber Subscribe {recv}");

                var messageSendFromClient = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine($"Subscriber Subscribe {messageSendFromClient}");

                var messageParts = messageSendFromClient.Split(",".ToCharArray());

                if (!string.IsNullOrEmpty(messageParts[0]))
                {
                    switch (messageParts[0])
                    {
                        case "Subscribe":

                            if (!string.IsNullOrEmpty(messageParts[1]))
                            {
                                Filter.AddSubscriber(messageParts[1], remoteEP);
                            }
                            break;
                        case "Unsubscribe":

                            if (!string.IsNullOrEmpty(messageParts[1]))
                            {
                                Filter.RemoveSubscriber(messageParts[1], remoteEP);
                            }
                            break;
                    }
                }
            }
        }
    }
}
