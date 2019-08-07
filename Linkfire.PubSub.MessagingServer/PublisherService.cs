using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Linkfire.PubSub.MessagingServer
{
    class PublisherService
    {
        public void StartPublisherService()
        {
            var th = new Thread(new ThreadStart(HostPublisherService))
            {
                // IsBackground = true
            };
            th.Start();
        }

        private void HostPublisherService()
        {
            var ipV4 = IPAddress.Parse("127.0.0.1");
            var localEP = new IPEndPoint(ipV4, 10001);
            var server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(localEP);
            StartListening(server);
        }

        private static IPAddress ReturnMachineIP()
        {
            var hostName = Dns.GetHostName();
            var ipEntry = Dns.GetHostEntry(hostName);
            var ipAddresses = ipEntry.AddressList;
            IPAddress ipV4 = null;

            foreach (var ipAddress in ipAddresses)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipV4 = ipAddress;
                    break;
                }
            }
            if (ipV4 == null)
            {
                Console.WriteLine("You have no IP of Version 4.Server can not run witout it");
                // Application.Exit();
            }
            return ipV4;
        }

        private static void StartListening(Socket server)
        {
            var data = new byte[1024];
            var recv = 0;
            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                try
                {
                    recv = 0;
                    data = new byte[1024];
                    recv = server.ReceiveFrom(data, ref remoteEP);
                    Console.WriteLine($"Publisher Publish {recv}");
                    var messageSendFromClient = Encoding.ASCII.GetString(data, 0, recv);
                    var messageParts = messageSendFromClient.Split(",".ToCharArray());
                    var command = messageParts[0];
                    var topicName = messageParts[1];

                    if (!string.IsNullOrEmpty(command))
                    {
                        if (messageParts[0] == "Publish")
                        {
                            if (!string.IsNullOrEmpty(topicName))
                            {
                                var eventParts = new List<string>(messageParts);
                                eventParts.RemoveRange(0, 1);
                                var message = MakeCommaSeparatedString(eventParts);
                                var subscriberListForThisTopic = Filter.GetSubscribers(topicName);
                                var workerThreadParameters = new WorkerThreadParameters
                                {
                                    Server = server,
                                    Message = message,
                                    SubscriberListForThisTopic = subscriberListForThisTopic
                                };

                                ThreadPool.QueueUserWorkItem(new WaitCallback(Publish), workerThreadParameters);
                            }
                        }
                    }
                }
                catch
                { }
            }
        }

        public static void Publish(object stateInfo)
        {
            var workerThreadParameters = (WorkerThreadParameters)stateInfo;
            var server = workerThreadParameters.Server;
            var message = workerThreadParameters.Message;
            var subscriberListForThisTopic = workerThreadParameters.SubscriberListForThisTopic;
            var messagelength = message.Length;

            if (subscriberListForThisTopic != null)
            {
                foreach (EndPoint endPoint in subscriberListForThisTopic)
                {
                    server.SendTo(Encoding.ASCII.GetBytes(message), messagelength, SocketFlags.None, endPoint);
                }
            }
        }

        private static string MakeCommaSeparatedString(List<string> eventParts)
        {
            var message = string.Empty;

            foreach (var eventPart in eventParts)
            {
                message = $"{message},{eventPart}";
            }

            if (message.Length != 0)
            {
                message = message.Remove(message.Length - 1, 1);
            }

            return message;
        }
    }

    class WorkerThreadParameters
    {
        public Socket Server { get; set; }

        public string Message { get; set; }

        public List<EndPoint> SubscriberListForThisTopic { get; set; }
    }
}
