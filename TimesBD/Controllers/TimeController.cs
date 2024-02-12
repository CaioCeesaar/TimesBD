using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimeController : TimeDbControllerBase
{
    public TimeController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTimesById(
        [FromQuery(Name = "id")] int id
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getTime) = await _businessClass.GetTimeByIdAsync(id);
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getTime)));
    }
    
    [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, TimeModel atualizaTime,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.AtualizarTimeAsync(id, atualizaTime.Nome, atualizaTime.Cep);
        return new Result(true, "Time atualizado com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(TimeModel time, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.InserirTimeAsync(time.Nome, time.Cep);
        return new Result(true, "Time inserido com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.DeletarTimeAsync(id);
        return new Result(true, "Time deletado com sucesso!");
    }
}