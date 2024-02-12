using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IngressoController : TimeDbControllerBase 
{
    public IngressoController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetIngressosById(
        [FromQuery(Name = "id")] int id
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getIngresso) = await _businessClass.GetIngressoByIdAsync(id);
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getIngresso)));
    }
    
    [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, IngressoPost atualizaIngresso,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.AtualizarIngressoAsync(id, atualizaIngresso.Valor, atualizaIngresso.JogoId);
        return new Result(true, "Ingresso atualizado com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(IngressoPost ingresso, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.InserirIngressoAsync(ingresso.Valor, ingresso.JogoId);
        return new Result(true, "Ingresso inserido com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.DeletarIngressoAsync(id);
        return new Result(true, "Ingresso deletado com sucesso!");
    }
}