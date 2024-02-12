using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PartidaController : TimeDbControllerBase
{
    public PartidaController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPartidasById(
        [FromQuery(Name = "id")] int id
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getPartida) = await _businessClass.GetPartidaByIdAsync(id);
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getPartida)));
    }
    
    [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, PartidaPostPatch atualizaPartida,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.AtualizarPartidaAsync(id, atualizaPartida.TimeID, atualizaPartida.JogoId, atualizaPartida.EstadioId);
        return new Result(true, "Partida atualizada com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(PartidaPostPatch partida, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.InserirPartidaAsync(partida.TimeID, partida.JogoId, partida.EstadioId);
        return new Result(true, "Partida inserida com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.DeletarPartidaAsync(id);
        return new Result(true, "Partida deletada com sucesso!");
    }
}