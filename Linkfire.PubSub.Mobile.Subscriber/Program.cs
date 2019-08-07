using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static System.Console;

namespace Linkfire.PubSub.Mobile.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Mobile Subscriber Server Starts...");

            InitServer();

            SubscribeToMobileTopic();

            ReceiveDataFromServer();

            ReadLine();
        }

        #region Members

        private static Socket _client;

        private static EndPoint _remoteEndPoint;

        private static byte[] data;
        private static int recv;
        private static bool isReceivingStarted = false;

        public static List<string> Events { get; set; } = new List<string>();

        #endregion

        #region Ctors

        public static void InitServer()
        {
            var serverIP = "127.0.0.1";
            var serverIPAddress = IPAddress.Parse(serverIP);
            var serverPort = 10002;

            _client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _remoteEndPoint = new IPEndPoint(serverIPAddress, serverPort);
        }

        #endregion

        static void ReceiveDataFromServer()
        {
            var publisherEndPoint = _client.LocalEndPoint;

            while (true)
            {
                recv = _client.ReceiveFrom(data, ref publisherEndPoint);

                var message = $"{Encoding.ASCII.GetString(data, 0, recv)},{publisherEndPoint.ToString()}";

                DisplayMessage(message);
            }
        }

        public static void DisplayMessage(string message)
        {
            var messageParts = message.Split(",".ToCharArray());

            for (int counter = 0; counter < messageParts.Length; counter++)
            {
                WriteLine(messageParts[counter]);
            }
        }

        private static void SubscribeToMobileTopic()
        {
            var command = Enums.Command.Subscribe.ToString();

            var message = $"{command},Mobile";

            _client.SendTo(Encoding.ASCII.GetBytes(message), _remoteEndPoint);

            if (!isReceivingStarted)
            {
                isReceivingStarted = true;

                data = new byte[1024];

                var thread1 = new Thread(new ThreadStart(ReceiveDataFromServer))
                {
                    IsBackground = true
                };

                thread1.Start();
            }
        }
    }
}
