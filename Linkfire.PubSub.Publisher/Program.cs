using System.Threading.Tasks;
using static System.Console;

namespace Linkfire.PubSub.Publisher
{
    class Program
    {
        public static void Main(string[] args)
        {
            // This project should be responsible of published messages
            var inProgress = true;
            do
            {
                WriteLine("Please enter message. Q to Quite");
                Write("> ");
                var input = ReadLine();
                if (input?.ToLower() != "q")
                {
                    // Publish any message/object user will enter

                    var publisher = new Publisher();

                    Task.Run(() => publisher.Publish<string>(input));
                }
                else
                {
                    inProgress = false;
                }
            } while (inProgress);
        }
    }
}
