using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace MediatRDemo.Service.Command
{
    public class NotyPingCommand1Handler : INotificationHandler<NotyPingCommand>
    {
        public Task Handle(NotyPingCommand notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("Noty1Handler Doing..." + notification.Message);
            return Task.CompletedTask;
        }
    }
}