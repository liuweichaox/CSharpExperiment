using MediatR;
using MediatRDemo.Entities;

namespace MediatRDemo.Service.Command
{
    public class UpdateCustomerCommand : IRequest<Customer>
    {
        public Customer Customer { get; set; }
    }
}