using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompradorController : TimeDbControllerBase
{
    public CompradorController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCompradoresById(
        [FromQuery(Name = "id")] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getComprador) = await _businessClass.GetCompradorByIdAsync(id);
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getComprador)));
    }
    
    [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, CompradorPostPatch atualizaComprador,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.AtualizarCompradorAsync(id, atualizaComprador.Nome, atualizaComprador.Cpf);
        return new Result(true, "Comprador atualizado com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(CompradorPostPatch comprador, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.InserirCompradorAsync(comprador.Nome, comprador.Cpf);
        return new Result(true, "Comprador inserido com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.DeletarCompradorAsync(id);
        return new Result(true, "Comprador deletado com sucesso!");
    }
    
}