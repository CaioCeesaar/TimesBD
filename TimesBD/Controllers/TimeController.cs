using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimeController : ControllerBase
{
    private readonly BusinessClass _businessClass;

    public TimeController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTimesById(
        [FromQuery(Name = "id")] int? id = null
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var getTime = await _businessClass.GetTimeByIdAsync(autentica, id);
        return Ok(getTime);
    }
    
    [HttpPatch]
    public async Task<IActionResult> Patch([FromQuery] int id, TimeModel atualizaTime,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.AtualizarTimeAsync(id, atualizaTime.Nome, atualizaTime.Cep);
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(TimeModel time, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var result = await _businessClass.InserirTimeAsync(time.Nome, time.Cep);
        return Ok(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.DeletarTimeAsync(id);
        return Ok();
    }
}