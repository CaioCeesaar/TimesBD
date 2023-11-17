using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Entities;

namespace TimesBD.Controllers;

public class IngressoController : ControllerBase 
{
    
    private readonly string _connectionString;

    private const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";

    public IngressoController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

   [HttpGet]
    public async Task<IActionResult> Get([FromQuery(Name = "id")] int? id = null, [FromQuery(Name = "valor")] double? valor = null,
        [FromQuery(Name = "partidaId")] int? partidaId = null, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");

        string filtro = "";
        if (id != null && id > 0)
        {
            filtro = "WHERE Id = @id";
        }
        else if (valor != null && valor > 0)
        {
            filtro = "WHERE Valor = @valor";
        }
        else if (partidaId != null && partidaId > 0)
        {
            filtro = "WHERE PartidaId = @partidaId";
        }

        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Ingresso {filtro}";
        var ingressos = await sqlConnection.QueryAsync<Ingresso>(sql, new { id, valor, partidaId });

        return Ok(ingressos);
    }

    [HttpPost]
    public async Task<IActionResult> PostIngressos(IngressoPost ingresso,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        
        if (ingresso.Valor <= 0)
        {
            return BadRequest("Valor não pode ser menor ou igual a zero");
        }
        
        if (ingresso.PartidaId <= 0)
        {
            return BadRequest("PartidaId não pode ser menor ou igual a zero");
        }
        
        // valida se a partida existe
        var sqlPartida = $"SELECT * FROM Partida WHERE Id = {ingresso.PartidaId}";
        var partida = await sqlConnection.QueryAsync<Partida>(sqlPartida);
        if (partida == null || !partida.Any())
        {
            return BadRequest($"Partida não encontrada: {ingresso.PartidaId}.");
        }
        
        string sql = $"EXEC sp_InserirIngresso {ingresso.Valor}, {ingresso.PartidaId}";
        await sqlConnection.ExecuteAsync(sql);
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteIngressos([FromQuery] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync($"EXEC sp_DeletarIngresso {id}", new { id });
            return linhaAfetada == 0
                ? NotFound("O id informado não foi encontrado")
                : Ok("Time deletado com sucesso");
        }
    }
    
    private static bool ValidarAutenticacao(HttpRequest request) => request.Headers.TryGetValue("autentica", out var autentica) && autentica == Autentica;

}