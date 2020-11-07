using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ChannelSampleConsole.Apps
{
    public static class BoundedChannelSample
    {
        static Channel<Mail> channel = Channel.CreateBounded<Mail>(10);
        static object sync = new object();

        public static async Task Run()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Now Running Bounded Channel Sample...");
            Console.ResetColor();

            await Task.WhenAll(MailSender(), MailReceiver());
        }

        private static Task MailReceiver()
        {
            return Task.Run(async () =>
            {
                while (await channel.Reader.WaitToReadAsync())
                {
                    var mail = await channel.Reader.ReadAsync();
                    lock (sync)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Mail#{mail.Subject} is received");
                        Console.ResetColor();
                    }

                    await RandomWait();
                }
                Console.WriteLine("All mail received");
            });
        }

        private static async Task RandomWait()
        {
            await Task.Delay(new Random(Math.Abs(Guid.NewGuid().GetHashCode())).Next(100, 3000));
        }

        private static Task MailSender()
        {
            return Task.Run(async () =>
            {
                for (int i = 0; i <= 11; i++)
                {

                    await channel.Writer.WriteAsync(new Mail { Subject = $"{i}" });

                    lock (sync)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Mail#{i} has been sent");
                        Console.ResetColor();
                    }

                    await RandomWait();
                }

                Console.WriteLine("All mail sent");
                channel.Writer.Complete();
            });
        }
    }
}