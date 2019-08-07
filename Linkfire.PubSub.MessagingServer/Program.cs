using static System.Console;

namespace Linkfire.PubSub.MessagingServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            WriteLine("Messaging Server Starts...");

            HostPublishSubscribeServices();
        }

        private static void HostPublishSubscribeServices()
        {
            var publisherService = new PublisherService();
            publisherService.StartPublisherService();

            var subscriberService = new SubscriberService();
            subscriberService.StartSubscriberService();
        }
    }
}
