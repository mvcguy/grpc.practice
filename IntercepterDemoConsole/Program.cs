using IntercepterDemoConsole.App;
using System;
using System.Threading.Tasks;

namespace IntercepterDemoConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            await InterceptorDemoClient.Run();

            Console.WriteLine("Press any key to exist");

            Console.ReadKey();
        }
    }
}
