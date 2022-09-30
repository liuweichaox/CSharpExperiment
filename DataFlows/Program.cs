// See https://aka.ms/new-console-template for more information

using System.Threading.Tasks.Dataflow;

var actionBlock = new ActionBlock<int>((msg) => { Console.WriteLine("ActionBlock receive:" + msg); });
for (int i = 0; i < 100; i++)
{
    await actionBlock.SendAsync(i);
}

actionBlock.Complete();

var bufferBlock = new BufferBlock<int>();
Task.Run(async () =>
{
    while (!bufferBlock.Completion.IsCompleted)
    {
        var msg = await bufferBlock.ReceiveAsync();
        Console.WriteLine("BufferBlock receive:" + msg);
    }
});

for (int i = 0; i < 100; i++)
{
    await bufferBlock.SendAsync(i);
}
bufferBlock.Complete();

var transformBlock = new TransformBlock<int, string>((input) =>
{
    var output = input.ToString();
    return "output "+output;
}); 

Task.Run(async () =>
{
    while (true)
    {
        var msg = await transformBlock.ReceiveAsync();
        Console.WriteLine("TransformBlock receive:" + msg);
    }
});

for (int i = 0; i < 100; i++)
{
    await transformBlock.SendAsync(i);
}


Console.ReadKey();