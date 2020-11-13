using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatRDemo.Entities;
using MediatRDemo.Repository;

namespace MediatRDemo.Service.Command
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Customer>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            return await _customerRepository.AddAsync(request.Customer);
        }
    }
}