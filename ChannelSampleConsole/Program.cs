using ChannelSampleConsole.Apps;
using System;
using System.Threading.Tasks;

namespace ChannelSampleConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            await BoundedChannelSample.Run();

        }


    }
}
