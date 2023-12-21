using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VendaController : ControllerBase
{
    private readonly BusinessClass _businessClass;

    public VendaController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(connectionString);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetVendasById(
        [FromQuery(Name = "id")] int? id = null
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var getVenda = await _businessClass.GetVendaByIdAsync(autentica, id);
        return Ok(getVenda);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(VendasPostPatch venda, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var result = await _businessClass.InserirVendaAsync(venda.DataVenda, venda.CompradorId, venda.IngressoId);
        return Ok(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        await _businessClass.DeletarVendaAsync(id);
        return Ok();
    }
}