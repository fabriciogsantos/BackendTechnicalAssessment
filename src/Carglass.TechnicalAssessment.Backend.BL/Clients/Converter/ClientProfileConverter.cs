using AutoMapper;
using Carglass.TechnicalAssessment.Backend.Dtos.Clients;
using Carglass.TechnicalAssessment.Backend.Entities;

namespace Carglass.TechnicalAssessment.Backend.BL.Clients.Converter;

public class ClientProfileConverter : Profile
{
    public ClientProfileConverter()
    {
        CreateMap<Client, ClientDto>().ReverseMap();
    }
}
