using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;
using TimesBD.Framework;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstadioController : TimeDbControllerBase2
{
    public EstadioController(IConfiguration configuration, TimesBackgroundService backgroundService) : base(backgroundService)
    {
        _ = configuration.GetConnectionString("DefaultConnection");
    }
    
    [HttpGet]
    public async Task<IActionResult> GetEstadios(
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getEstadios) = await _backgroundService.GetEstadios();
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getEstadios)));
    }
    
    [HttpGet("busca-por-id")]
    public async Task<IActionResult> GetEstadiosById(
        [FromQuery(Name = "id")] int id
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getEstadio) = await _backgroundService.GetEstadiosById(id, autentica);
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getEstadio)));
    }
    
    [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, EstadiosModel atualizaEstadio,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.AtualizarEstadioAsync(id, atualizaEstadio.Nome, atualizaEstadio.Limite, atualizaEstadio.Cep);
        return new Result(true, "Estadio atualizado com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(EstadiosModel estadio, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.InserirEstadioAsync(estadio.Nome, estadio.Limite, estadio.Cep);
        return new Result(true, "Estadio inserido com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.DeletarEstadioAsync(id);
        return new Result(true, "Estadio deletado com sucesso!");
    }
}