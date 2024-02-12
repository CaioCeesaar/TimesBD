using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VendaController : TimeDbControllerBase
{
    public VendaController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetVendasById(
        [FromQuery(Name = "id")] int id
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var (getResult, getVenda) = await _businessClass.GetVendaByIdAsync(id);
        return ConvertResultToHttpResult(new Result(getResult.Sucess, JsonSerializer.Serialize(getVenda)));
    }
    
    [HttpPost]
    public async Task<Result> Post(VendasPostPatch venda, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.InserirVendaAsync(venda.DataVenda, venda.CompradorId, venda.IngressoId);
        return new Result(true, "Venda inserida com sucesso!");
    }
    
    [HttpDelete]
    public async Task<Result> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.DeletarVendaAsync(id);
        return new Result(true, "Venda deletada com sucesso!");
    }
}