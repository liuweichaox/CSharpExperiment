using MediatR;

namespace MediatRDemo.Service.Command
{
    public class NotyPingCommand : INotification
    {
        public string Message { get; set; }
    }
}