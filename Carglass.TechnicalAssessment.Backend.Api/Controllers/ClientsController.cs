using Carglass.TechnicalAssessment.Backend.BL;
using Carglass.TechnicalAssessment.Backend.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Carglass.TechnicalAssessment.Backend.Api.Controllers;

[ApiController]
[Route("clients")]
public class ClientsController : ControllerBase
{
    private readonly IClientAppService _clientAppService;

    public ClientsController(IClientAppService clientAppService)
    {
        this._clientAppService = clientAppService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        // TODO Implement
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(int id)
    {
        return Ok(_clientAppService.GetById(id));
    }

    [HttpPost]
    public IActionResult Create([FromBody] ClientDto dto)
    {
        // TODO Implement
        throw new NotImplementedException();
    }

    [HttpPut]
    public IActionResult Update([FromBody] ClientDto dto)
    {
        _clientAppService.Update(dto);
        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete([FromBody] ClientDto dto)
    {
        // TODO Implement
        throw new NotImplementedException();
    }
}