using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompradorController : ControllerBase
{
    private readonly BusinessClass _businessClass;

    public CompradorController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCompradoresById(
        [FromQuery(Name = "id")] int? id = null
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var getComprador = await _businessClass.GetCompradorByIdAsync(autentica, id);
        return Ok(getComprador);
    }
    
    [HttpPatch]
    public async Task<IActionResult> Patch([FromQuery] int id, CompradorPostPatch atualizaComprador,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.AtualizarCompradorAsync(id, atualizaComprador.Nome, atualizaComprador.CPF);
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(CompradorPostPatch comprador, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var result = await _businessClass.InserirCompradorAsync(comprador.Nome, comprador.CPF);
        return Ok(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.DeletarCompradorAsync(id);
        return Ok();
    }
    
}