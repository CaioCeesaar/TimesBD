using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JogoController : TimeDbControllerBase
{
    public JogoController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetJogosById(
        [FromQuery(Name = "id")] int id
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getJogo) = await _businessClass.GetJogoByIdAsync(id);
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getJogo)));
    }
    
    [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, JogoPostPatch atualizaJogo,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.AtualizarJogoAsync(id, atualizaJogo.Data, atualizaJogo.EstadioId);
        return new Result(true, "Jogo atualizado com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(JogoPostPatch jogo, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.InserirJogoAsync(jogo.Data, jogo.EstadioId);
        return new Result(true, "Jogo inserido com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.DeletarJogoAsync(id);
        return new Result(true, "Jogo deletado com sucesso!");
    }
}