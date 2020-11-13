using System;
using System.Threading;
using System.Threading.Tasks;
using MediatRDemo.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediatRDemo.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository( ) : base()
        {
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            // return await CustomerContext.Customer.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return new Customer();
        }
    }
}