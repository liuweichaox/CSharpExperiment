using System;
using MediatR;
using MediatRDemo.Entities;

namespace MediatRDemo.Service.Query
{
    public class GetCustomerByIdQuery : IRequest<Customer>
    {
        public Guid Id { get; set; }
    }
}