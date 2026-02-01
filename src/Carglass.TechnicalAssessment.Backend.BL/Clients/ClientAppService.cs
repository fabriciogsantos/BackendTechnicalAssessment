using AutoMapper;
using FluentValidation;
using Carglass.TechnicalAssessment.Backend.BL.Interfaces;
using Carglass.TechnicalAssessment.Backend.Core.Dtos;
using Carglass.TechnicalAssessment.Backend.Core.Extensions;
using Carglass.TechnicalAssessment.Backend.Core.Resources;
using Carglass.TechnicalAssessment.Backend.DL.Interfaces.Repositories;
using Carglass.TechnicalAssessment.Backend.Dtos.Clients;
using Carglass.TechnicalAssessment.Backend.Entities;
using Microsoft.Extensions.Logging;

namespace Carglass.TechnicalAssessment.Backend.BL.Clients;

public class ClientAppService : IClientAppService
{
	private readonly IMapper _mapper;
	private readonly ILogger<ClientAppService> _logger;
	private readonly IValidator<ClientDto> _clientValidator;
	private readonly IClientInMemoryRepository _clientRepository;

	public ClientAppService(IClientInMemoryRepository clientRepository, 
							IValidator<ClientDto> clientValidator,
							IMapper mapper, ILogger<ClientAppService> logger)
	{
		_mapper = mapper;
		_logger = logger;
		_clientValidator = clientValidator;
		_clientRepository = clientRepository;
	}

	public OperationResult<IEnumerable<ClientDto>> GetAll(Pagination pagination)
    {
		var result = new OperationResult<IEnumerable<ClientDto>>();
		try
		{
			var clients = _clientRepository.GetAll(pagination);
			result.AddResult(_mapper.Map<IEnumerable<ClientDto>>(clients));
			return result;
		}
		catch (Exception ex)
		{
			result.AddError(MessagesResult.RequestFail, 500, ex);
			_logger.LogError(ex, MessagesResult.RequestFail);
			throw;
		}
    }

	public OperationResult<ClientDto> GetById(int id)
	{
		var result = new OperationResult<ClientDto>();
		try
		{
			var client = _clientRepository.GetById(id);
			if (client == null)
			{
				result.AddError(string.Format(MessagesResult.NotExists,"client", id), 400);
				return result;
			}
			result.AddResult(_mapper.Map<ClientDto>(client));
			return result;
		}
		catch (Exception ex)
		{
			result.AddError(MessagesResult.RequestFail, 500, ex);
			_logger.LogError(ex, MessagesResult.RequestFail);
			throw;
		}
    }

    public OperationResult<ClientDto> Create(ClientDto client)
	{
		var result = new OperationResult<ClientDto>();
		try
		{
			client.Id = _clientRepository.GetNextId();
			var validationResult = ValidateDto(client);
			if (!validationResult.IsValid())
			{
				return result.AddErrors(validationResult.Errors);
			}

			validationResult = EnsureClientExists(client);
			if (!validationResult.IsValid())
			{
				return result.AddErrors(validationResult.Errors);
			}

			if (!_clientRepository.Create(_mapper.Map<Client>(client)) && !result.HasErrors)
			{
				result.AddError(MessagesResult.RequestFail, 400);
			}

			result.AddResult(client);
			return result;
		}
		catch (Exception ex)
		{

			result.AddError(MessagesResult.RequestFail, 500, ex);
			_logger.LogError(ex, MessagesResult.RequestFail);
			throw;
		}
	}
	
	public OperationResult Update(ClientDto client)
    {
		var result = new OperationResult();
		try
		{
			if (_clientRepository.GetById(client.Id) == null)
			{
				result.AddError(string.Format(MessagesResult.NotExists, "client", client.Id), 400);
				return result;
			}

			var validationResult = ValidateDto(client);
			if (!validationResult.IsValid())
			{
				return result.AddErrors(validationResult.Errors);
			}

			result = EnsureClientExistsSameNumberDocument(client, result);
			if (result.HasErrors) return result;

			var entity = _mapper.Map<Client>(client);
			if (!_clientRepository.Update(entity))
			{
				result.AddError(MessagesResult.RequestFail, 400);
			}

			return result;
		}
		catch (Exception ex)
		{
			result.AddError(MessagesResult.RequestFail, 500, ex);
			_logger.LogError(ex, MessagesResult.RequestFail);
			throw;
		}
	}

    public OperationResult Delete(int id)
    {
		var result = new OperationResult();
		try
		{
			var client = _clientRepository.GetById(id);
			if (client == null)
			{
				result.AddError(string.Format(MessagesResult.NotExists, "client", id), 400);
				return result;
			}

			if (!_clientRepository.Delete(client))
			{
				result.AddError(MessagesResult.RequestFail, 400);
			}
			return result;
		}
		catch (Exception ex)
		{
			result.AddError(MessagesResult.RequestFail, 500, ex);
			_logger.LogError(ex, MessagesResult.RequestFail);
			throw;
		}
	}

    private ValidationResult ValidateDto(ClientDto client)
    {
		var result = new ValidationResult();
		var validationResult = _clientValidator.Validate(client);
        if (validationResult.Errors.Any())
        {
            var errorsValidations = string.Join("; ", validationResult.Errors.Select(s => s.ErrorMessage));
			result.AddError(string.Format(MessagesResult.ErrorValidation, "client", errorsValidations));
        }

		return result;
    }

	private ValidationResult EnsureClientExists(ClientDto client)
	{
		var result = new ValidationResult();
		if (_clientRepository.Search(p => p.Id == client.Id || p.DocNum.ToLower() == client.DocNum.ToLower()).Any())
		{
			result.AddError(string.Format(MessagesResult.ClientExists, client.Id, client.DocNum));
		}

		return result;
	}

	private OperationResult EnsureClientExistsSameNumberDocument(ClientDto client, OperationResult result)
	{
	    var clientsExisting = _clientRepository.Search(p => p.Id != client.Id && p.DocNum.ToLower() == client.DocNum.ToLower());
		if (clientsExisting.Any())
		{
			var clientExisting = clientsExisting.First();
			result.AddError(string.Format(MessagesResult.ClientExists, clientExisting.Id, clientExisting.DocNum), 400);
		}

		return result;
	}
}
