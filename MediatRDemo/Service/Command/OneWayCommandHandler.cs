using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace MediatRDemo.Service.Command
{
    public class OneWayCommandHandler:AsyncRequestHandler<OneWayCommand>
    {
        protected override Task Handle(OneWayCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}