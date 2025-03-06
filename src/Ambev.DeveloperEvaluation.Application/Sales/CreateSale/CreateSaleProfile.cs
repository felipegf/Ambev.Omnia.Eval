using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Profile responsible for mapping CreateSaleCommand to Sale entity.
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleProfile"/> class.
    /// Configures mappings between CreateSaleCommand and Sale entities.
    /// </summary>
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleCommand, Sale>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // ID is generated
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Handled internally
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) // Handled internally
            .ForMember(dest => dest.SaleItems, opt => opt.MapFrom(src => src.Items));

        CreateMap<CreateSaleItemCommand, SaleItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // ID is generated
            .ForMember(dest => dest.SaleId, opt => opt.Ignore()) // Set in repository
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()); // Calculated
    }
}
