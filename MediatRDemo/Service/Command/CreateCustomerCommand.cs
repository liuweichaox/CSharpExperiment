using MediatR;
using MediatRDemo.Entities;

namespace MediatRDemo.Service.Command
{
    public class CreateCustomerCommand : IRequest<Customer>
    {
        public Customer Customer { get; set; }
    }
}