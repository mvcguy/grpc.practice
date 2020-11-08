using ChannelSampleConsole.Apps;
using System;
using System.Threading.Tasks;

namespace ChannelSampleConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World! Channels demo");
            try
            {
                await UnboundedChannelSample.Run();
                await BoundedChannelSample.Run();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {e.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
