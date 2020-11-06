using DiceGame;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace WebService.Services.PlayDiceSrv
{
    public class PlayDiceService : DiceGameService.DiceGameServiceBase
    {

        public override async Task Start(IAsyncStreamReader<GameRequest> requestStream,
            IServerStreamWriter<GameResponse> responseStream, ServerCallContext context)
        {
            await foreach (var item in requestStream.ReadAllAsync())
            {
                //
                // client has played its dice
                //
                var serverResult = await RollDice();
                await responseStream.WriteAsync(new GameResponse { ServerResult = $"Server result: {serverResult}" });                
            }

        }

        public override async Task<DiceResult> RollDice(EmptyRequest request, ServerCallContext context)
        {
            var result = await RollDice();
            return new DiceResult { Result = result };
        }

        private Task<int> RollDice()
        {
            var task1 = Task.Run(async() =>
            {
                await Task.Delay(2000);
                var rnd = new Random(Math.Abs(Guid.NewGuid().GetHashCode()));
                return rnd.Next(1, 7);
            });
            return task1;
        }
    }
}
