using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Entities;

namespace TimesBD.Controllers;

public class VendaController : ControllerBase
{
    
    private readonly string _connectionString;

    private const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";

    public VendaController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get([FromHeader(Name = "autentica")] string? autentica = null, [FromQuery(Name = "id")] int? id = null, [FromQuery(Name = "compradorId")] int? compradorId = null, [FromQuery(Name = "ingressoId")] int? ingressoId = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");
        
        string filtro = "";
        if (id != null && id > 0)
        {
            filtro = "WHERE Id = @id";
        }
        else if (compradorId != null && compradorId > 0)
        {
            filtro = "WHERE CompradorId = @compradorId";
        }
        else if (ingressoId != null && ingressoId > 0)
        {
            filtro = "WHERE IngressoId = @ingressoId";
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Vendas {filtro}";
        var vendas = await sqlConnection.QueryAsync<Vendas>(sql, new { id, compradorId, ingressoId });

        return Ok(vendas);
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