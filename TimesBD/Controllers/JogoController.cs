using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JogoController : Controller
{
    private readonly string _connectionString;
    
    private readonly BusinessClass _businessClass;

    private const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";

    public JogoController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(_connectionString);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetJogosById(
        [FromQuery(Name = "id")] int? id = null
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var getJogo = await _businessClass.GetJogoByIdAsync(autentica, id);
        return Ok(getJogo);
    }

    [HttpPost]
    public async Task<IActionResult> PostJogos(JogoPostPatch jogo,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        
        if (jogo.EstadioId <= 0)
        {
            return BadRequest("EstadioId não pode ser menor ou igual a zero");
        }
        
        // valida se o estadio existe
        var sqlEstadio = $"SELECT * FROM Estadios WHERE Id = {jogo.EstadioId}";
        var estadio = await sqlConnection.QueryAsync<Estadios>(sqlEstadio);
        if (estadio == null || !estadio.Any())
        {
            return BadRequest($"Estádio não encontrado: {jogo.EstadioId}.");
        }

        string sql = $"EXEC sp_InserirJogo '{jogo.Data}', {jogo.EstadioId}";
        await sqlConnection.ExecuteAsync(sql);
        return Ok();
    }
    
    [HttpPatch]
    public async Task<IActionResult> Patch(JogoPostPatch jogo,
        [FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Jogo WHERE Id = @id";
        var jogoConsulta = await sqlConnection.QueryAsync<Jogo>(sql, new { id });
        if (jogoConsulta is null)
        {
            return BadRequest("Jogo não encontrado");
        }
        
        var parameters = $"{id}";
        if (!(jogo.Data > DateTime.Now))
        {
            parameters += $", '{jogo.Data}'";
        }
        else
        {
            return BadRequest("A data do jogo não pode ser maior que a data atual");
        }
        
        if (jogo.EstadioId > 0)
        {
            var sqlEstadio = $"SELECT * FROM Estadios WHERE Id = {jogo.EstadioId}";
            var estadio = await sqlConnection.QueryAsync<Estadios>(sqlEstadio);
            if (estadio == null || !estadio.Any())
            {
                return BadRequest($"Estádio não encontrado: {jogo.EstadioId}.");
            }
        }
        
        var sql2 = $"EXEC sp_AtualizarJogo {parameters}, {jogo.EstadioId}";
        await sqlConnection.ExecuteAsync(sql2);
        return Ok("Jogo atualizado com sucesso");
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteJogos([FromQuery] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");
        
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync($"EXEC sp_DeletarJogo {id}", new { id });
            return linhaAfetada == 0
                ? NotFound("O id informado não foi encontrado")
                : Ok("Jogo deletado com sucesso");
        }
    }
    
    private static bool ValidarAutenticacao(HttpRequest request) => request.Headers.TryGetValue("autentica", out var autentica) && autentica == Autentica;

}