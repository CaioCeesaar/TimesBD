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
    public async Task<IActionResult> Jogadores(
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getJogadores) = await _backgroundService.Jogadores();
        await _backgroundService.InserirLogAsync("GetJogadores", "Busca de todos os jogadores", "");
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getJogadores)));
    }

    [HttpGet("busca-por-id")]
    [ProducesResponseType(typeof(Jogador), StatusCodes.Status200OK)]
    public async Task<IActionResult> JogadoresById(
        [FromQuery(Name = "id")] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getJogador) = await _backgroundService.JogadorById(id);
        await _backgroundService.InserirLogAsync("GetJogadoresById", "Busca de jogador por Id", $"ID: {id}");
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getJogador)));
    }
    
     [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, JogadorModel atualizaJogador,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
           await _backgroundService.AtualizarJogadorAsync(id, atualizaJogador.Nome, atualizaJogador.DataNascimento, atualizaJogador.TimeId, atualizaJogador.Cep);
           await _backgroundService.InserirLogAsync("AtualizarJogador",
               $"Jogador({id})",
               $"Nome: {atualizaJogador.Nome}, \nData de Nascimento: {atualizaJogador.DataNascimento}, \nTimeId: {atualizaJogador.TimeId}, \nCEP: {atualizaJogador.Cep}");
           return new Result(true, "Jogador atualizado com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(JogadorModel jogador, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.InserirJogadorAsync(jogador.Nome, jogador.DataNascimento, jogador.TimeId, jogador.Cep);
        await _backgroundService.InserirLogAsync("InserirJogador",
            $"Jogador {jogador.Nome} inserido com sucesso!",
            $"Nome: {jogador.Nome}, \nData de Nascimento: {jogador.DataNascimento}, \nTimeId: {jogador.TimeId}, \nCEP: {jogador.Cep}");
        return new Result(true, "Jogador inserido com sucesso!");
    }

     [HttpDelete]
     public async Task<Result> Delete([FromQuery] int id,
         [FromHeader(Name = "Autentica")] string? autentica = null)
     {
         await _backgroundService.DeletarJogadorAsync(id);
            await _backgroundService.InserirLogAsync("DeletarJogador", "Jogador deletado com sucesso!", $"ID: {id}");
         return new Result(true, "Jogador deletado com sucesso!");
     }
    
}