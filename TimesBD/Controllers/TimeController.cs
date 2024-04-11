using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;
using TimesBD.Framework;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimeController : TimeDbControllerBase2
{
    public TimeController(IConfiguration configuration, TimesBackgroundService backgroundService) : base(backgroundService)
    {
        _ = configuration.GetConnectionString("DefaultConnection");
    }
    
    [HttpGet]
    public async Task<IActionResult> Times(
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getTimes) = await _backgroundService.Times();
        await _backgroundService.InserirLogAsync("GetTimes", "Busca de todos os times", "");
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getTimes)));
    }
    
    [HttpGet("busca-por-id")]
    public async Task<IActionResult> TimesById(
        [FromQuery(Name = "id")] int id
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getTime) = await _backgroundService.TimesById(id);
        await _backgroundService.InserirLogAsync("GetTimesById", "Busca de time por Id", $"ID: {id}");
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getTime)));
    }
    
    [HttpPatch]
    public async Task<Result> Patch([FromQuery] int id, TimeModel atualizaTime,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.AtualizarTimeAsync(id, atualizaTime.Nome, atualizaTime.Cep);
        await _backgroundService.InserirLogAsync("AtualizarTime", $"Time({id})", $"Nome: {atualizaTime.Nome}, \nCEP: {atualizaTime.Cep}");
        return new Result(true, "Time atualizado com sucesso!");
    }
    
    [HttpPost]
    public async Task<Result> Post(TimeModel time, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.InserirTimeAsync(time.Nome, time.Cep);
        await _backgroundService.InserirLogAsync("InserirTime", $"Time {time.Nome} inserido com sucesso!", $"Nome: {time.Nome}, \nCEP: {time.Cep}");
        return new Result(true, "Time inserido com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.DeletarTimeAsync(id);
        await _backgroundService.InserirLogAsync("DeletarTime", "Time deletado com sucesso!", $"ID: {id}");
        return new Result(true, "Time deletado com sucesso!");
    }
}