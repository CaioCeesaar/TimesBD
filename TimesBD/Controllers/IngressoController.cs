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
    public async Task<IActionResult> Ingressos(
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getIngressos) = await _backgroundService.Ingressos();
        await _backgroundService.InserirLogAsync("GetIngressos", "Busca de todos os ingressos", "");
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getIngressos)));
    }
    
    [HttpGet("busca-por-id")]
    public async Task<IActionResult> IngressosById(
        [FromQuery(Name = "id")] int id
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getIngresso) = await _backgroundService.IngressosById(id);
        await _backgroundService.InserirLogAsync("GetIngressosById", "Busca de ingresso por Id", $"ID: {id}");
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getIngresso)));
    }
    
    [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, IngressoPost atualizaIngresso,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.AtualizarIngressoAsync(id, atualizaIngresso.Valor, atualizaIngresso.JogoId);
        await _backgroundService.InserirLogAsync("AtualizarIngresso",
            $"Ingresso({id})",
            $"Valor: {atualizaIngresso.Valor}, \nJogoId: {atualizaIngresso.JogoId}");
        return new Result(true, "Ingresso atualizado com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(IngressoPost ingresso, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.InserirIngressoAsync(ingresso.Valor, ingresso.JogoId);
        await _backgroundService.InserirLogAsync("InserirIngresso",
            $"Ingresso {ingresso.Valor} inserido com sucesso!",
            $"Valor: {ingresso.Valor}, \nJogoId: {ingresso.JogoId}");
        return new Result(true, "Ingresso inserido com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.DeletarIngressoAsync(id);
        await _backgroundService.InserirLogAsync("DeletarIngresso", "Ingresso deletado com sucesso!", $"ID: {id}");
        return new Result(true, "Ingresso deletado com sucesso!");
    }
}