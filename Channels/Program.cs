// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;

var channel=Channel.CreateUnbounded<int>();
Task.Run(async () =>
{
  while (!channel.Reader.Completion.IsCompleted)
  {
    var msg =await channel.Reader.ReadAsync();
    Console.WriteLine("Channel receive "+msg);
  }
});
for (var i = 0; i < 100; i++)
{
  await channel.Writer.WriteAsync(i);
}
Console.ReadKey();
var channelWait=Channel.CreateBounded<int>(new BoundedChannelOptions(10)
{
  AllowSynchronousContinuations = true,
  Capacity = 10,
  FullMode = BoundedChannelFullMode.Wait
});
Task.Run(async () =>
{
  while (!channelWait.Reader.Completion.IsCompleted)
  {
    var msg =await channelWait.Reader.ReadAsync();
    Console.WriteLine("Channel Wait receive "+msg);
  }
});
for (var i = 0; i < 100; i++)
{
  await channelWait.Writer.WriteAsync(i);
}
Console.ReadKey();
var channelDropNewest=Channel.CreateBounded<int>(new BoundedChannelOptions(10)
{
  AllowSynchronousContinuations = true,
  Capacity = 10,
  FullMode = BoundedChannelFullMode.DropNewest
});
Task.Run(async () =>
{
  while (!channelDropNewest.Reader.Completion.IsCompleted)
  {
    var msg =await channelDropNewest.Reader.ReadAsync();
    Console.WriteLine("Channel DropNewest receive "+msg);
  }
});
for (var i = 0; i < 100; i++)
{
  await channelDropNewest.Writer.WriteAsync(i);
}

Console.ReadKey();
var channelDropOldest=Channel.CreateBounded<int>(new BoundedChannelOptions(10)
{
  AllowSynchronousContinuations = true,
  Capacity = 10,
  FullMode = BoundedChannelFullMode.DropOldest
});
Task.Run(async () =>
{
  while (!channelDropOldest.Reader.Completion.IsCompleted)
  {
    var msg =await channelDropOldest.Reader.ReadAsync();
    Console.WriteLine("Channel DropOldest receive "+msg);
  }
});
for (var i = 0; i < 100; i++)
{
  await channelDropOldest.Writer.WriteAsync(i);
}
Console.ReadKey();
var channelDropWrite=Channel.CreateBounded<int>(new BoundedChannelOptions(10)
{
  AllowSynchronousContinuations = true,
  Capacity = 10,
  FullMode = BoundedChannelFullMode.DropWrite
});
Task.Run(async () =>
{
  while (!channelDropWrite.Reader.Completion.IsCompleted)
  {
    var msg =await channelDropWrite.Reader.ReadAsync();
    Console.WriteLine("Channel DropWrite receive "+msg);
  }
});
for (var i = 0; i < 100; i++)
{
  await channelDropWrite.Writer.WriteAsync(i);
}
Console.ReadKey();