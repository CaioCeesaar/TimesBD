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
    public async Task<IActionResult> Estadios(
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getEstadios) = await _backgroundService.Estadios();
        await _backgroundService.InserirLogAsync("GetEstadios", "Busca de todos os estadios", "");
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getEstadios)));
    }
    
    [HttpGet("busca-por-id")]
    public async Task<IActionResult> EstadiosById(
        [FromQuery(Name = "id")] int id
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getEstadio) = await _backgroundService.EstadiosById(id, autentica);
        await _backgroundService.InserirLogAsync("GetEstadiosById", "Busca de estadio por Id", $"ID: {id}");
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getEstadio)));
    }
    
    [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, EstadiosModel atualizaEstadio,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.AtualizarEstadioAsync(id, atualizaEstadio.Nome, atualizaEstadio.Limite, atualizaEstadio.Cep);
        await _backgroundService.InserirLogAsync("AtualizarEstadio",
            $"Estadio({id})",
            $"Nome: {atualizaEstadio.Nome}, \nLimite: {atualizaEstadio.Limite}, \nCEP: {atualizaEstadio.Cep}");
        return new Result(true, "Estadio atualizado com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(EstadiosModel estadio, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.InserirEstadioAsync(estadio.Nome, estadio.Limite, estadio.Cep);
        await _backgroundService.InserirLogAsync("InserirEstadio",
            $"Estadio {estadio.Nome} inserido com sucesso!",
            $"Nome: {estadio.Nome}, \nLimite: {estadio.Limite}, \nCEP: {estadio.Cep}");
        return new Result(true, "Estadio inserido com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.DeletarEstadioAsync(id);
        await _backgroundService.InserirLogAsync("DeletarEstadio", "Estadio deletado com sucesso!", $"ID: {id}");
        return new Result(true, "Estadio deletado com sucesso!");
    }
}