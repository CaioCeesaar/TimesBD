using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IngressoController : ControllerBase 
{
    
    private readonly string _connectionString;
    
    private readonly BusinessClass _businessClass;

    private const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";

    public IngressoController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(_connectionString);
    }

   [HttpGet]
    public async Task<IActionResult> GetIngressosById(
        [FromQuery(Name = "id")] int? id = null
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var getIngresso = await _businessClass.GetIngressoByIdAsync(autentica, id);
        return Ok(getIngresso);
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
        
        if (ingresso.JogoId <= 0)
        {
            return BadRequest("JogoId não pode ser menor ou igual a zero");
        }
        
        // valida se a partida existe
        var sqlPartida = $"SELECT * FROM Partida WHERE Id = {ingresso.JogoId}";
        var partida = await sqlConnection.QueryAsync<Partida>(sqlPartida);
        if (partida == null || !partida.Any())
        {
            return BadRequest($"Jogo não encontrado: {ingresso.JogoId}.");
        }
        
        string sql = $"EXEC sp_InserirIngresso {ingresso.Valor}, {ingresso.JogoId}";
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