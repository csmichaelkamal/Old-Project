using System.Threading.Tasks;

namespace Linkfire.PubSub.Publisher
{
    interface IPublisher
    {
        Task Publish<T>(T message);
    }
}
