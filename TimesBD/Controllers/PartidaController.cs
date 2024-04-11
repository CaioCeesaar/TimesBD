using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;
using TimesBD.Framework;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PartidaController : TimeDbControllerBase2
{
    public PartidaController(IConfiguration configuration, TimesBackgroundService backgroundService) : base(backgroundService)
    {
        _ = configuration.GetConnectionString("DefaultConnection");
    }
    
    [HttpGet]
    public async Task<IActionResult> Partidas(
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getPartidas) = await _backgroundService.Partidas();
        await _backgroundService.InserirLogAsync("GetPartidas", "Busca de todas as partidas", "");
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getPartidas)));
    }
    
    [HttpGet("busca-por-id")]
    public async Task<IActionResult> PartidasById(
        [FromQuery(Name = "id")] int id
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getPartida) = await _backgroundService.PartidasById(id);
        await _backgroundService.InserirLogAsync("GetPartidasById", "Busca de partida por Id", $"ID: {id}");
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getPartida)));
    }
    
    [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, PartidaPostPatch atualizaPartida,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.AtualizarPartidaAsync(id, atualizaPartida.TimeID, atualizaPartida.JogoId, atualizaPartida.EstadioId);
        await _backgroundService.InserirLogAsync("AtualizarPartida",
            $"Partida({id})",
            $"TimeID: {atualizaPartida.TimeID}, \nJogoId: {atualizaPartida.JogoId}, \nEstadioId: {atualizaPartida.EstadioId}");
        return new Result(true, "Partida atualizada com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(PartidaPostPatch partida, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.InserirPartidaAsync(partida.TimeID, partida.JogoId, partida.EstadioId);
        await _backgroundService.InserirLogAsync("InserirPartida",
            $"Partida {partida.TimeID} inserida com sucesso!",
            $"TimeID: {partida.TimeID}, \nJogoId: {partida.JogoId}, \nEstadioId: {partida.EstadioId}");
        return new Result(true, "Partida inserida com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.DeletarPartidaAsync(id);
        await _backgroundService.InserirLogAsync("DeletarPartida", "Partida deletada com sucesso!", $"ID: {id}");
        return new Result(true, "Partida deletada com sucesso!");
    }
}