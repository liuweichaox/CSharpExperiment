using System;
using System.Threading;
using System.Threading.Tasks;
using MediatRDemo.Entities;

namespace MediatRDemo.Repository
{
    public interface ICustomerRepository: IRepository<Customer>
    {
        Task<Customer> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}