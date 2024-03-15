using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;
using TimesBD.Framework;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IngressoController : TimeDbControllerBase2
{
    public IngressoController(IConfiguration configuration, TimesBackgroundService backgroundService) : base(backgroundService)
    {
        _ = configuration.GetConnectionString("DefaultConnection");
    }
    
    [HttpGet]
    public async Task<IActionResult> GetIngressos(
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getIngressos) = await _backgroundService.GetIngressos();
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getIngressos)));
    }
    
    [HttpGet("busca-por-id")]
    public async Task<IActionResult> GetIngressosById(
        [FromQuery(Name = "id")] int id
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getIngresso) = await _backgroundService.GetIngressosById(id);
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getIngresso)));
    }
    
    [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, IngressoPost atualizaIngresso,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.AtualizarIngressoAsync(id, atualizaIngresso.Valor, atualizaIngresso.JogoId);
        return new Result(true, "Ingresso atualizado com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(IngressoPost ingresso, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.InserirIngressoAsync(ingresso.Valor, ingresso.JogoId);
        return new Result(true, "Ingresso inserido com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.DeletarIngressoAsync(id);
        return new Result(true, "Ingresso deletado com sucesso!");
    }
}