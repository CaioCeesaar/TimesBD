using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JogoController : ControllerBase
{
    private readonly BusinessClass _businessClass;

    public JogoController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetJogosById(
        [FromQuery(Name = "id")] int? id = null
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var getJogo = await _businessClass.GetJogoByIdAsync(autentica, id);
        return Ok(getJogo);
    }
    
    [HttpPatch]
    public async Task<IActionResult> Patch([FromQuery] int id, JogoPostPatch atualizaJogo,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.AtualizarJogoAsync(id, atualizaJogo.Data, atualizaJogo.EstadioId);
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(JogoPostPatch jogo, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var result = await _businessClass.InserirJogoAsync(jogo.Data, jogo.EstadioId);
        return Ok(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.DeletarJogoAsync(id);
        return Ok();
    }
}