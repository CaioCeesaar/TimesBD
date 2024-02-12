using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstadioController : ControllerBase
{
    private readonly BusinessClass _businessClass;

    public EstadioController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetEstadiosById(
        [FromQuery(Name = "id")] int? id = null
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var getEstadio = await _businessClass.GetEstadiosByIdAsync(autentica, id);
        return Ok(getEstadio);
    }
    
    [HttpPatch]
    public async Task<IActionResult> Patch([FromQuery] int id, EstadiosModel atualizaEstadio,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.AtualizarEstadioAsync(id, atualizaEstadio.Nome, atualizaEstadio.Limite, atualizaEstadio.Cep);
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(EstadiosModel estadio, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var result = await _businessClass.InserirEstadioAsync(estadio.Nome, estadio.Limite, estadio.Cep);
        return Ok(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.DeletarEstadioAsync(id);
        return Ok();
    }
    
}