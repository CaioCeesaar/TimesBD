using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;
using TimesBD.Framework;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompradorController : TimeDbControllerBase2
{
    public CompradorController(IConfiguration configuration, TimesBackgroundService backgroundService) : base(backgroundService)
    {
        _ = configuration.GetConnectionString("DefaultConnection")!;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCompradores(
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getCompradores) = await _backgroundService.GetCompradores();
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getCompradores)));
    }
    
    [HttpGet("busca-por-id")]
    public async Task<IActionResult> GetCompradoresById(
        [FromQuery(Name = "id")] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getComprador) = await _backgroundService.GetCompradorById(id);
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getComprador)));
    }
    
    [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, CompradorPostPatch atualizaComprador,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.AtualizarCompradorAsync(id, atualizaComprador.Nome, atualizaComprador.Cpf);
        return new Result(true, "Comprador atualizado com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(CompradorPostPatch comprador, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.InserirCompradorAsync(comprador.Nome, comprador.Cpf);
        return new Result(true, "Comprador inserido com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.DeletarCompradorAsync(id);
        return new Result(true, "Comprador deletado com sucesso!");
    }
    
}