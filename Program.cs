using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Channels {
    class Program {
        static async Task  Main(string[] args) {
            const int max = 1000;
            var channel = Channel.CreateBounded<int>(10);
            var count = 0;

            _ = Task.Run(async() => {
                await foreach (var i in channel.Reader.ReadAllAsync())  {
                    count++;
                }

            });


            var writer = Task.Run(async () => {
                for (var i = 0; i < max; i++) {
                    await channel.Writer.WriteAsync(i);
                }
                channel.Writer.Complete();
            });

            await writer;
            await channel.Reader.Completion;
      
           Console.WriteLine("Hello World! Total is {0}", count);


        }
    }
}
