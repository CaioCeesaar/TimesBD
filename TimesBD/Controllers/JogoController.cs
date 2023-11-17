using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Entities;

namespace TimesBD.Controllers;

public class JogoController : Controller
{
    private readonly string _connectionString;

    private const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";

    public JogoController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetJogos([FromHeader(Name = "Autentica")] string? autentica = null, [FromQuery(Name = "id")] int? id = null, [FromQuery(Name = "DataJogo")] DateTime? data = null, [FromQuery(Name = "EstadioId")] int? estadioId = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        string filtro = "";
        if (id != null && id > 0)
        {
            filtro = "WHERE Id = @id";
        }
        else if (data != null)
        {
            filtro = "WHERE DataJogo = @data";
        }
        else if (estadioId != null && estadioId > 0)
        {
            filtro = "WHERE EstadioId = @estadioId";
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Jogo {filtro}";
        var jogos = await sqlConnection.QueryAsync<Jogo>(sql, new { id, data, estadioId });
        return Ok(jogos); 
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