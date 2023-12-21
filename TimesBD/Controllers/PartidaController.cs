using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PartidaController : ControllerBase
{
    private readonly BusinessClass _businessClass;

    public PartidaController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPartidasById(
        [FromQuery(Name = "id")] int? id = null
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var getPartida = await _businessClass.GetPartidaByIdAsync(autentica, id);
        return Ok(getPartida);
    }
    
    [HttpPatch]
    public async Task<IActionResult> Patch([FromQuery] int id, PartidaPostPatch atualizaPartida,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.AtualizarPartidaAsync(id, atualizaPartida.TimeID, atualizaPartida.JogoId, atualizaPartida.EstadioId);
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(PartidaPostPatch partida, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var result = await _businessClass.InserirPartidaAsync(partida.TimeID, partida.JogoId, partida.EstadioId);
        return Ok(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.DeletarPartidaAsync(id);
        return Ok();
    }
}