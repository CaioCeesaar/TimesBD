using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Entities;
using TimesBD.Business;
using TimesBD.Framework;

namespace TimesBD.Controllers;

[Route("api/jogadores")]
[ApiController]
public class JogadorController : TimeDbControllerBase2
{
    public JogadorController(IConfiguration configuration, TimesBackgroundService backgroundService) : base(backgroundService)
    {
        _ = configuration.GetConnectionString("DefaultConnection");
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(Jogador), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJogadores(
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getJogadores) = await _backgroundService.GetJogadores();
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getJogadores)));
    }

    [HttpGet("busca-por-id")]
    [ProducesResponseType(typeof(Jogador), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJogadoresById(
        [FromQuery(Name = "id")] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getJogador) = await _backgroundService.GetJogadorById(id);
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getJogador)));
    }
    
     [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, JogadorModel atualizaJogador,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
           await _backgroundService.AtualizarJogadorAsync(id, atualizaJogador.Nome, atualizaJogador.DataNascimento, atualizaJogador.TimeId, atualizaJogador.Cep);
           return new Result(true, "Jogador atualizado com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(JogadorModel jogador, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.InserirJogadorAsync(jogador.Nome, jogador.DataNascimento, jogador.TimeId, jogador.Cep);
        return new Result(true, "Jogador inserido com sucesso!");
    }

     [HttpDelete]
     public async Task<Result> Delete([FromQuery] int id,
         [FromHeader(Name = "Autentica")] string? autentica = null)
     {
         await _backgroundService.DeletarJogadorAsync(id);
         return new Result(true, "Jogador deletado com sucesso!");
     }
    
}