using AutoMapper;
using MediatRDemo.Entities;
using MediatRDemo.Models;

namespace MediatRDemo.Infrastructure.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCustomerModel, Customer>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<UpdateCustomerModel, Customer>();
        }
    }
}