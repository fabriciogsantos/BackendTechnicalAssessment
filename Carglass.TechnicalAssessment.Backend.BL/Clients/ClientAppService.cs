using AutoMapper;
using Carglass.TechnicalAssessment.Backend.DL.Repositories;
using Carglass.TechnicalAssessment.Backend.Dtos;
using Carglass.TechnicalAssessment.Backend.Entities;
using FluentValidation;

namespace Carglass.TechnicalAssessment.Backend.BL;

internal class ClientAppService : IClientAppService
{
    private readonly ClientIMRepository theData;
    private readonly IMapper magicalClassChanger;
    private readonly IValidator<ClientDto> allIsCorrectHere;

    // TODO Implement
    public ClientAppService()
    { }

    public IEnumerable<ClientDto> GetAll()
    {
        var moneySpenders = theData.GetAll();
        return magicalClassChanger.Map<IEnumerable<ClientDto>>(moneySpenders);
    }

    public ClientDto GetById(params object[] keyValues)
    {
        var theOne = theData.GetById(keyValues);
        return magicalClassChanger.Map<ClientDto>(theOne);
    }

    public void Create(ClientDto newMoney)
    {
        if (null != theData.GetById(newMoney.Id))
        {
            throw new Exception("Ya existe un cliente con este Id");
        }

        // TODO Implement
        throw new NotImplementedException();
    }

    public void Update(ClientDto aBitOfMakeup)
    {
        if (null == theData.GetById(aBitOfMakeup.Id))
        {
            throw new Exception("No existe ningún cliente con este Id");
        }

        ValidateDto(aBitOfMakeup);

        var entity = magicalClassChanger.Map<Client>(aBitOfMakeup);
        theData.Update(entity);
    }

    public void Delete(ClientDto byebyee)
    {
        // TODO Implement
        throw new NotImplementedException();
    }

    private void ValidateDto(ClientDto item)
    {
        var validationResult = allIsCorrectHere.Validate(item);
        if (validationResult.Errors.Any())
        {
            string toShowErrors = string.Join("; ", validationResult.Errors.Select(s => s.ErrorMessage));
            throw new Exception($"El cliente especificado no cumple los requisitos de validación. Errores: '{toShowErrors}'");
        }
    }
}