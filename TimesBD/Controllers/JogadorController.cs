using Microsoft.AspNetCore.Mvc;
using TimesBD.Entities;
using TimesBD.Business;

namespace TimesBD.Controllers;

[Route("api/jogadores")]
[ApiController]
public class JogadorController : ControllerBase
{
    private readonly BusinessClass _businessClass;

    public JogadorController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Jogador), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJogadoresById(
        [FromQuery(Name = "id")] int? id = null
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var getJogador = await _businessClass.GetJogadorByIdAsync(autentica, id);
        return Ok(getJogador);
    }

     [HttpPatch]
     public async Task<IActionResult> Patch([FromQuery] int id, JogadorModel atualizaJogador,
         [FromHeader(Name = "Autentica")] string? autentica = null)
     {
            await _businessClass.AtualizarJogadorAsync(id, atualizaJogador.Nome, atualizaJogador.DataNascimento, atualizaJogador.TimeId, atualizaJogador.Cep);
            return Ok();
     }
    
    [HttpPost]
    public async Task<IActionResult> Post(JogadorModel jogador, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var result = await _businessClass.InserirJogadorAsync(jogador.Nome, jogador.DataNascimento, jogador.TimeId, jogador.Cep);
        return Ok(result);
    }

     [HttpDelete]
     public async Task<IActionResult> Delete([FromQuery] int id,
         [FromHeader(Name = "Autentica")] string? autentica = null)
     {
         await _businessClass.DeletarJogadorAsync(id);
         return Ok();
     }
    
}