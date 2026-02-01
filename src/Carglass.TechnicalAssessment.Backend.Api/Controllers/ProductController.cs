using Carglass.TechnicalAssessment.Backend.BL.Interfaces;
using Carglass.TechnicalAssessment.Backend.Core.Dtos;
using Carglass.TechnicalAssessment.Backend.Core.Extensions;
using Carglass.TechnicalAssessment.Backend.Dtos.Products;
using Microsoft.AspNetCore.Mvc;

namespace Carglass.TechnicalAssessment.Backend.Api.Controllers;

[ApiController]
[Route("Products")]
public class ProductController : ControllerBase
{
    private readonly IProductAppService _productAppService;

    public ProductController(IProductAppService productAppService)
    {
        _productAppService = productAppService;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] Pagination pagination)
    {
		var result = _productAppService.GetAll(pagination);
		return result.ToActionResult(this);
	}

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(int id)
    {
        var result = _productAppService.GetById(id);
		return result.ToActionResult(this);
	}

    [HttpPost]
    public IActionResult Create([FromBody] ProductDto dto)
	{
		var result = _productAppService.Create(dto);
		return result.ToActionResult(this);
	}

    [HttpPut]
    public IActionResult Update([FromBody] ProductDto dto)
    {
		var result = _productAppService.Update(dto);
		return result.ToActionResult(this);
	}

    [HttpDelete]
    public IActionResult Delete([FromBody] int id)
    {
        var result = _productAppService.Delete(id);
		return result.ToActionResult(this);
	}
}