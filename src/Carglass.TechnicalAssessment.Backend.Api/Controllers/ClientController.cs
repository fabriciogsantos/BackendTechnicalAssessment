using Carglass.TechnicalAssessment.Backend.BL.Interfaces;
using Carglass.TechnicalAssessment.Backend.Core.Dtos;
using Carglass.TechnicalAssessment.Backend.Core.Extensions;
using Carglass.TechnicalAssessment.Backend.Dtos.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Carglass.TechnicalAssessment.Backend.Api.Controllers;

[ApiController]
[Route("clients")]
public class ClientController : ControllerBase
{
    private readonly IClientAppService _clientAppService;

    public ClientController(IClientAppService clientAppService)
    {
        _clientAppService = clientAppService;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] Pagination pagination)
    {
		var result = _clientAppService.GetAll(pagination);
		return result.ToActionResult(this);
	}

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(int id)
    {
        var result = _clientAppService.GetById(id);
		return result.ToActionResult(this);
	}

    [HttpPost]
    public IActionResult Create([FromBody] ClientDto dto)
	{
		var result = _clientAppService.Create(dto);
		return result.ToActionResult(this);
	}

    [HttpPut]
    public IActionResult Update([FromBody] ClientDto dto)
    {
		var result = _clientAppService.Update(dto);
		return result.ToActionResult(this);
	}

    [HttpDelete]
    public IActionResult Delete([FromBody] int id)
    {
        var result = _clientAppService.Delete(id);
		return result.ToActionResult(this);
	}
}