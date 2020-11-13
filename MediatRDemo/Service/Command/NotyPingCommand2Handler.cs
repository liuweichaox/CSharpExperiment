using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace MediatRDemo.Service.Command
{
    public class NotyPingCommand2Handler : INotificationHandler<NotyPingCommand>
    {
        public Task Handle(NotyPingCommand notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("Noty2Handler Doing..." + notification.Message);
            return Task.CompletedTask;
        }
    }
}