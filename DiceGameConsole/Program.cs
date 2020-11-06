using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DiceGameConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lets play dice!");
            PlayDiceClient.PlayGame("Shahid Ali");
        }
    }
}
