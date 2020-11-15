using System;
using System.Threading.Tasks;

namespace GrpcAuthDemoConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            try
            {
                var token = await GetTokenApp.GetJwt("me.shahidali@hotmail.com");
                await GrpcAuthDemo.Run(token);
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }
    }


}
