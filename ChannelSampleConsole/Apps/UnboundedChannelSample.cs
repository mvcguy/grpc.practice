using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ChannelSampleConsole.Apps
{
    public static class UnboundedChannelSample
    {

        static Channel<Mail> channel = Channel.CreateUnbounded<Mail>();

        public static async Task Run()
        {
            //
            // A unbounded channel is something which is thread safe and supports multiple writers and readers
            //

            //
            // lets create a few writer that can write some mails to the channel
            //

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Running Unbounded Channel Sample...");
            Console.ResetColor();
           
            var writerTask = Task.Run(async () =>
            {
                try
                {
                    var mailerList = new List<Task>();
                    for (int i = 0; i < 5; i++)
                    {
                        var mailer = SendMail();
                        mailerList.Add(mailer);
                    }

                    await Task.WhenAll(mailerList);

                    if (channel.Writer.TryComplete())
                        Console.WriteLine("No more mails to forward! Hurrayyyy");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error ocurred. Error: {0}", ex.Message);
                    channel.Writer.TryComplete(ex);
                }


            });

            //
            // lets create some readers that can read from the channel whenever a mail is arrived
            //
            var readers = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                var readerTask = Task.Run(async () =>
                {
                    var taskID = Guid.NewGuid().GetHashCode();
                    while (await channel.Reader.WaitToReadAsync())
                    {
                        if (channel.Reader.TryRead(out var mail))
                        {
                            Console.WriteLine($"Task-{taskID}: A new mail has arrived Code: {mail.GetHashCode()}");
                        }
                    }
                });

                readers.Add(readerTask);
            }

            await Task.WhenAll(readers);

        }

        private static async Task SendMail()
        {
            for (int i = 0; i < 3; i++)
            {
                //
                // send a mail every few seconds
                //
                var mail = new Mail();
                Console.WriteLine("Mail {0} is sent", mail.GetHashCode());
                await channel.Writer.WriteAsync(mail);
                await Task.Delay(new Random(Math.Abs(Guid.NewGuid().GetHashCode())).Next(500, 1000));
            }
        }
    }
}
