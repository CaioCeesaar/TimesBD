using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IngressoController : ControllerBase 
{
    private readonly BusinessClass _businessClass;

    public IngressoController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetIngressosById(
        [FromQuery(Name = "id")] int? id = null
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var getIngresso = await _businessClass.GetIngressoByIdAsync(autentica, id);
        return Ok(getIngresso);
    }
    
    [HttpPatch]
    public async Task<IActionResult> Patch([FromQuery] int id, IngressoPost atualizaIngresso,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.AtualizarIngressoAsync(id, atualizaIngresso.Valor, atualizaIngresso.JogoId);
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(IngressoPost ingresso, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var result = await _businessClass.InserirIngressoAsync(ingresso.Valor, ingresso.JogoId);
        return Ok(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.DeletarIngressoAsync(id);
        return Ok();
    }
}