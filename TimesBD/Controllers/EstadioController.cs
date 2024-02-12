using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstadioController : TimeDbControllerBase
{
    public EstadioController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetEstadiosById(
        [FromQuery(Name = "id")] int id
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getEstadio) = await _businessClass.GetEstadiosByIdAsync(id);
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getEstadio)));
    }
    
    [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, EstadiosModel atualizaEstadio,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.AtualizarEstadioAsync(id, atualizaEstadio.Nome, atualizaEstadio.Limite, atualizaEstadio.Cep);
        return new Result(true, "Estadio atualizado com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(EstadiosModel estadio, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.InserirEstadioAsync(estadio.Nome, estadio.Limite, estadio.Cep);
        return new Result(true, "Estadio inserido com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.DeletarEstadioAsync(id);
        return new Result(true, "Estadio deletado com sucesso!");
    }
}