using AutoMapper;
using Carglass.TechnicalAssessment.Backend.Dtos.Products;
using Carglass.TechnicalAssessment.Backend.Entities;

namespace Carglass.TechnicalAssessment.Backend.BL.Products.Converter;

public class ProductProfileConverter : Profile
{
    public ProductProfileConverter()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}
