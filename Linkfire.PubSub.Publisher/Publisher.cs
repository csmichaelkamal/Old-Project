using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Linkfire.PubSub.Publisher
{
    class Publisher : IPublisher
    {
        #region Members

        Socket _client;
        EndPoint _remoteEndPoint;

        string _command = Enums.Command.Publish.ToString();

        #endregion

        #region Ctors

        public Publisher()
        {
            var serverIP = "127.0.0.1";
            var serverIPAddress = IPAddress.Parse(serverIP);
            var serverPort = 10001;

            _client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _remoteEndPoint = new IPEndPoint(serverIPAddress, serverPort);
        }

        #endregion

        #region Methods

        public async Task Publish<T>(T inputMessage)
        {
            if (inputMessage != null)
            {
                var parsedInputMessage = inputMessage.ToString().Split(',');

                if (parsedInputMessage.Length > 1)
                {
                    var topicName = parsedInputMessage[0];
                    var eventData = parsedInputMessage[1];

                    var message = $"{_command},{topicName},{eventData}";

                    _client.SendTo(Encoding.ASCII.GetBytes(message), _remoteEndPoint);
                }
            }
        }

        #endregion
    }
}
