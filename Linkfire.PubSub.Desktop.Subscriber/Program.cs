using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static System.Console;

namespace Linkfire.PubSub.Desktop.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Desktop Subscriber Server Starts...");

            var server = new Server();
            var subscriber = new Subscriber();

            InitServer();

            SubscribeToDesktopTopic();

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

        #region Methods

        static void InitServer(string serverIP = "127.0.0.1", int serverPort = 10002)
        {
            if (IPAddress.TryParse(serverIP, out IPAddress serverIPAddress))
            {
                _client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _remoteEndPoint = new IPEndPoint(serverIPAddress, serverPort);
            }
        }

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

        private static void SubscribeToDesktopTopic()
        {
            var command = Enums.Command.Subscribe.ToString();

            var message = $"{command},Desktop";

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

        #endregion
    }
}
