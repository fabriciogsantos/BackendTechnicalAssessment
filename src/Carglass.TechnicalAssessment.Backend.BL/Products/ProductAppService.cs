using AutoMapper;
using FluentValidation;
using Carglass.TechnicalAssessment.Backend.BL.Interfaces;
using Carglass.TechnicalAssessment.Backend.Core.Dtos;
using Carglass.TechnicalAssessment.Backend.Core.Extensions;
using Carglass.TechnicalAssessment.Backend.Core.Resources;
using Carglass.TechnicalAssessment.Backend.DL.Interfaces.Repositories;
using Carglass.TechnicalAssessment.Backend.Dtos.Products;
using Carglass.TechnicalAssessment.Backend.Entities;
using Microsoft.Extensions.Logging;

namespace Carglass.TechnicalAssessment.Backend.BL.Products;

public class ProductAppService : IProductAppService
{
	private readonly IMapper _mapper;
	private readonly ILogger<ProductAppService> _logger;
	private readonly IValidator<ProductDto> _productValidator;
	private readonly IProductInMemoryRepository _productRepository;

	public ProductAppService(IProductInMemoryRepository productRepository, 
							IValidator<ProductDto> productValidator,
							IMapper mapper, ILogger<ProductAppService> logger)
	{
		_mapper = mapper;
		_logger = logger;
        _productValidator = productValidator;
        _productRepository = productRepository;
	}

	public OperationResult<IEnumerable<ProductDto>> GetAll(Pagination pagination)
    {
		var result = new OperationResult<IEnumerable<ProductDto>>();
		try
		{
			var products = _productRepository.GetAll(pagination);
			result.AddResult(_mapper.Map<IEnumerable<ProductDto>>(products));
			return result;
		}
		catch (Exception ex)
		{
			result.AddError(MessagesResult.RequestFail, 500, ex);
			_logger.LogError(ex, MessagesResult.RequestFail);
			throw;
		}
    }

	public OperationResult<ProductDto> GetById(int id)
	{
		var result = new OperationResult<ProductDto>();
		try
		{
			var product = _productRepository.GetById(id);
			if (product == null)
			{
				result.AddError(string.Format(MessagesResult.NotExists,"producto", id), 400);
				return result;
			}
			result.AddResult(_mapper.Map<ProductDto>(product));
			return result;
		}
		catch (Exception ex)
		{
			result.AddError(MessagesResult.RequestFail, 500, ex);
			_logger.LogError(ex, MessagesResult.RequestFail);
			throw;
		}
    }

    public OperationResult<ProductDto> Create(ProductDto product)
	{
		var result = new OperationResult<ProductDto>();
		try
		{
			product.Id = _productRepository.GetNextId();
			var validationResult = ValidateDto(product);
			if (!validationResult.IsValid())
			{
				return result.AddErrors(validationResult.Errors);
			}

			validationResult = EnsureProductExists(product);
			if (!validationResult.IsValid())
			{
				return result.AddErrors(validationResult.Errors);
			}

			if (!_productRepository.Create(_mapper.Map<Product>(product)) && !result.HasErrors)
			{
				result.AddError(MessagesResult.RequestFail, 400);
			}
			result.AddResult(product);
			return result;
		}
		catch (Exception ex)
		{

			result.AddError(MessagesResult.RequestFail, 500, ex);
			_logger.LogError(ex, MessagesResult.RequestFail);
			throw;
		}
	}
	
	public OperationResult Update(ProductDto product)
    {
		var result = new OperationResult();
		try
		{
			if (_productRepository.GetById(product.Id) == null)
			{
				result.AddError(string.Format(MessagesResult.NotExists,"producto", product.Id), 400);
				return result;
			}

			var validationResult = ValidateDto(product);
			if (!validationResult.IsValid())
			{
				return result.AddErrors(validationResult.Errors);
			}
            var entity = _mapper.Map<Product>(product);
			if (!_productRepository.Update(entity))
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
			var product = _productRepository.GetById(id);
			if (product == null)
			{
				result.AddError(string.Format(MessagesResult.NotExists,"producto", id), 400);
				return result;
			}

			if (!_productRepository.Delete(product))
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

    private ValidationResult ValidateDto(ProductDto productDto)
    {
		var result = new ValidationResult();
		var validationResult = _productValidator.Validate(productDto);
        if (validationResult.Errors.Any())
        {
            var errorsValidations = string.Join("; ", validationResult.Errors.Select(s => s.ErrorMessage));
			result.AddError(string.Format(MessagesResult.ErrorValidation,"producto", errorsValidations));
        }

		return result;
    }

	private ValidationResult EnsureProductExists(ProductDto product)
	{
		var result = new ValidationResult();
		if (_productRepository.GetById(product.Id) != null)
		{
			result.AddError(string.Format(MessagesResult.ProductoExists, product.Id));
		}

		return result;
	}
}
