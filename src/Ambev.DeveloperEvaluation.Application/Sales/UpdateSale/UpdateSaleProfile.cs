using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Mapping profile for updating a sale.
/// </summary>
public class UpdateSaleProfile : Profile
{
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleCommand, Sale>()
            .ForMember(dest => dest.SaleItems, opt => opt.MapFrom(src => src.Items));

        CreateMap<UpdateSaleItemCommand, SaleItem>();
    }
}
