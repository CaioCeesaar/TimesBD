using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;
using TimesBD.Framework;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JogoController : TimeDbControllerBase2
{
    public JogoController(IConfiguration configuration, TimesBackgroundService backgroundService) : base(backgroundService)
    {
        _ = configuration.GetConnectionString("DefaultConnection");
    }
    
    [HttpGet]
    public async Task<IActionResult> Jogos(
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getJogos) = await _backgroundService.Jogos();
        await _backgroundService.InserirLogAsync("GetJogos", "Busca de todos os jogos", "");
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getJogos)));
    }
    
    
    [HttpGet("busca-por-id")]
    public async Task<IActionResult> JogosById(
        [FromQuery(Name = "id")] int id
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getJogo) = await _backgroundService.JogosById(id);
        await _backgroundService.InserirLogAsync("GetJogosById", "Busca de jogo por Id", $"ID: {id}");
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getJogo)));
    }
    
    [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, JogoPostPatch atualizaJogo,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.AtualizarJogoAsync(id, atualizaJogo.Data, atualizaJogo.EstadioId);
        await _backgroundService.InserirLogAsync("AtualizarJogo",
            $"Jogo({id})",
            $"Data: {atualizaJogo.Data}, \nEstadioId: {atualizaJogo.EstadioId}");
        return new Result(true, "Jogo atualizado com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(JogoPostPatch jogo, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.InserirJogoAsync(jogo.Data, jogo.EstadioId);
        await _backgroundService.InserirLogAsync("InserirJogo",
            $"Jogo {jogo.Data} inserido com sucesso!",
            $"Data: {jogo.Data}, \nEstadioId: {jogo.EstadioId}");
        return new Result(true, "Jogo inserido com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.DeletarJogoAsync(id);
        await _backgroundService.InserirLogAsync("DeletarJogo", "Jogo deletado com sucesso!", $"ID: {id}");
        return new Result(true, "Jogo deletado com sucesso!");
    }
}