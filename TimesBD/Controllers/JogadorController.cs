using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Entities;
using TimesBD.Business;

namespace TimesBD.Controllers;

[Route("api/jogadores")]
[ApiController]
public class JogadorController : TimeDbControllerBase
{
    public JogadorController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Jogador), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJogadoresById(
        [FromQuery(Name = "id")] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getJogador) = await _businessClass.GetJogadorByIdAsync(id);
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getJogador)));
    }

     [HttpPatch]
     public async Task<Result> Patch([FromQuery] int id, JogadorModel atualizaJogador,
         [FromHeader(Name = "Autentica")] string? autentica = null)
     {
            await _businessClass.AtualizarJogadorAsync(id, atualizaJogador.Nome, atualizaJogador.DataNascimento, atualizaJogador.TimeId, atualizaJogador.Cep);
            return new Result(true, "Jogador atualizado com sucesso!");
     }
    
    [HttpPost]
    public async Task<Result> Post(JogadorModel jogador, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.InserirJogadorAsync(jogador.Nome, jogador.DataNascimento, jogador.TimeId, jogador.Cep);
        return new Result(true, "Jogador inserido com sucesso!");
    }

     [HttpDelete]
     public async Task<Result> Delete([FromQuery] int id,
         [FromHeader(Name = "Autentica")] string? autentica = null)
     {
         await _businessClass.DeletarJogadorAsync(id);
         return new Result(true, "Jogador deletado com sucesso!");
     }
    
}