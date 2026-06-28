using AutoMapper;
using OrderProcessing.Domain.DTOs;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.ImportService;

internal class ImportMappingProfile : Profile
{
    public ImportMappingProfile()
    {
        CreateMap<CustomerDto, Customer>()
            .ForMember(dest => dest.Orders, opt => opt.Ignore());

        CreateMap<OrderDto, Order>()
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore())
            .ForMember(dest => dest.DiscountRule, opt => opt.Ignore());

        CreateMap<OrderItemDto, OrderItem>();
    }
}