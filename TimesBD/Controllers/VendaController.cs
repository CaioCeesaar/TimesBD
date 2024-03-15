using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;
using TimesBD.Framework;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VendaController : TimeDbControllerBase2
{
    public VendaController(IConfiguration configuration, TimesBackgroundService backgroundService) : base(backgroundService)
    {
        _ = configuration.GetConnectionString("DefaultConnection");
    }
    
    [HttpGet]
    public async Task<IActionResult> GetVendas(
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getVendas) = await _backgroundService.GetVendas();
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getVendas)));
    }
    
    [HttpGet("busca-por-id")]
    public async Task<IActionResult> GetVendasById(
        [FromQuery(Name = "id")] int id
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getVenda) = await _backgroundService.GetVendasById(id);
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getVenda)));
    }
    
    [HttpPost]
    public async Task<Result> Post(VendasPostPatch venda, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.InserirVendaAsync(venda.DataVenda, venda.CompradorId, venda.IngressoId);
        return new Result(true, "Venda inserida com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _backgroundService.DeletarVendaAsync(id);
        return new Result(true, "Venda deletada com sucesso!");
    }
}