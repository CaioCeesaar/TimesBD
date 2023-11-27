using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VendaController : ControllerBase
{
    
    private readonly string _connectionString;
    
    private readonly BusinessClass _businessClass;

    private const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";

    public VendaController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(_connectionString);
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
    public async Task<IActionResult> PostVendas(VendasPostPatch venda,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");
        
        using var sqlConnection = new SqlConnection(_connectionString);
        
        if (venda.CompradorId <= 0)
        {
            return BadRequest("CompradorId não pode ser menor ou igual a zero");
        }
        
        if (venda.IngressoID <= 0)
        {
            return BadRequest("IngressoId não pode ser menor ou igual a zero");
        }
        
        if(venda.DataVenda > DateTime.Now)
        {
            return BadRequest("Data de venda não pode ser maior que a data atual");
        }
        
        // valida se o comprador existe
        var sqlComprador = $"SELECT * FROM Comprador WHERE Id = {venda.CompradorId}";
        var comprador = await sqlConnection.QueryAsync<Comprador>(sqlComprador);
        if (comprador == null || !comprador.Any())
        {
            return BadRequest($"Comprador não encontrado: {venda.CompradorId}.");
        }
        
        // valida se o ingresso existe
        var sqlIngresso = $"SELECT * FROM Ingresso WHERE Id = {venda.IngressoID}";
        var ingresso = await sqlConnection.QueryAsync<Ingresso>(sqlIngresso);
        if (ingresso == null || !ingresso.Any())
        {
            return BadRequest($"Ingresso não encontrado: {venda.IngressoID}.");
        }

        string sql = $"EXEC sp_InserirVenda '{venda.DataVenda}', {venda.CompradorId}, {venda.IngressoID}";
        await sqlConnection.ExecuteAsync(sql);
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteVendas([FromQuery] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");
        
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync($"EXEC sp_DeletarVenda {id}", new { id });
            return linhaAfetada == 0
                ? NotFound("O id informado não foi encontrado")
                : Ok("Venda deletada com sucesso");
        }
    }
    
    private static bool ValidarAutenticacao(HttpRequest request) => request.Headers.TryGetValue("autentica", out var autentica) && autentica == Autentica;
}