// See https://aka.ms/new-console-template for more information

using System.Threading.Tasks.Dataflow;

var actionBlock = new ActionBlock<int>((msg) => { Console.WriteLine("ActionBlock receive:" + msg); });
for (int i = 0; i < 100; i++)
{
    await actionBlock.SendAsync(i);
}


var bufferBlock = new BufferBlock<int>();
for (int i = 0; i < 100; i++)
{
    await bufferBlock.SendAsync(i);
}

Task.Run(async () =>
{
    while (true)
    {
        var msg = await bufferBlock.ReceiveAsync();
        Console.WriteLine("BufferBlock receive:" + msg);
    }
});

var transformBlock = new TransformBlock<int, string>((input) =>
{
    var output = input.ToString();
    return "output "+output;
}); 

for (int i = 0; i < 100; i++)
{
    await transformBlock.SendAsync(i);
}

Task.Run(async () =>
{
    while (true)
    {
        var msg = await transformBlock.ReceiveAsync();
        Console.WriteLine("TransformBlock receive:" + msg);
    }
});
Console.ReadKey();