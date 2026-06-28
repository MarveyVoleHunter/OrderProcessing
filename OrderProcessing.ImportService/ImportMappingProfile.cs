using AutoMapper;
using OrderProcessing.Domain.DTOs;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.ImportService;

public class ImportMappingProfile : Profile
{
    public ImportMappingProfile()
    {
        CreateMap<CustomerDto, Customer>()
            .ForMember(dest => dest.Orders, opt => opt.Ignore());

        CreateMap<OrderDto, Order>()
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore())
            .ForMember(dest => dest.AppliedDiscounts, opt => opt.Ignore());

        CreateMap<OrderItemDto, OrderItem>()
            .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(i => i.Discount));
    }
}