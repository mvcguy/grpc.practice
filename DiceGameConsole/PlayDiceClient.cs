using DiceGame;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace DiceGameConsole
{
    public static class PlayDiceClient
    {
        static readonly GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
        static readonly DiceGameService.DiceGameServiceClient client = new DiceGameService.DiceGameServiceClient(channel);
        static bool QuitSignal = false;
        static readonly object Sync = new object();

        public static void PlayGame(string name)
        {
            Task.WaitAll(StartGame(), WatchGame());
        }

        private static async Task MyPlay(IClientStreamWriter<GameRequest> request)
        {
            //
            // client play
            //
            var myDice = await client.RollDiceAsync(new EmptyRequest());
            var clientResult = $"Client Result: {myDice.Result}";

            Console.WriteLine(clientResult);

            await request.WriteAsync(new GameRequest { ClientResult = clientResult });
        }

        private static Task WatchGame()
        {
            return Task.Run(() =>
            {
                Console.ReadKey();
                QuitGame();
            });
        }

        private static Task StartGame()
        {
            return Task.Run(async () =>
            {
                using var duplexStream = client.Start();

                var request = duplexStream.RequestStream;
                var response = duplexStream.ResponseStream;

                await MyPlay(request);

                //
                // server play
                //
                await foreach (var item in response.ReadAllAsync())
                {

                    if (QuitSignal)
                    {
                        await request.WriteAsync(new GameRequest { EndGame = true });
                        break;
                    }

                    Console.WriteLine(item.ServerResult);
                    Console.WriteLine("**********************************");

                    await MyPlay(request);

                }

                await request.CompleteAsync();
            });
        }

        private static void QuitGame()
        {
            Console.WriteLine("Game is being ended, please wait ...");
            lock (Sync)
            {
                QuitSignal = true;
            }

        }
    }
}
