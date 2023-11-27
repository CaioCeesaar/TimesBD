using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PartidaController : ControllerBase
{
    
    private readonly string _connectionString;
    
    private readonly BusinessClass _businessClass;

    private const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";

    public PartidaController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(_connectionString);
    }
    
    [HttpGet("Partidas")]
    public async Task<IActionResult> GetPartidasById(
        [FromQuery(Name = "id")] int? id = null
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var getPartida = await _businessClass.GetPartidaByIdAsync(autentica, id);
        return Ok(getPartida);
    }

    [HttpPost("Partidas")]
    public async Task<IActionResult> PostPartidas(PartidaPostPatch partida,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        
        if (partida.TimeID > 0)
        {
            var sqlTime = $"SELECT * FROM Times WHERE Id = {partida.TimeID}";
            var time = await sqlConnection.QueryAsync<TimeModel>(sqlTime);
            if (time == null || !time.Any())
            {
                return BadRequest($"Time não encontrado: {partida.TimeID}.");
            }
        }
        else
        {
            return BadRequest("TimeId não pode ser nulo");
        }
        
        if (partida.JogoId > 0)
        {
            var sqlJogo = $"SELECT * FROM Jogo WHERE Id = {partida.JogoId}";
            var jogo = await sqlConnection.QueryAsync<Jogo>(sqlJogo);
            if (jogo == null || !jogo.Any())
            {
                return BadRequest($"Jogo não encontrado: {partida.JogoId}.");
            }
        }
        else
        {
            return BadRequest("JogoId não pode ser nulo");
        }

        if (partida.EstadioId > 0)
        {
            var sqlEstadio = $"SELECT * FROM Estadios WHERE Id = {partida.EstadioId}";
            var estadio = await sqlConnection.QueryAsync<Estadios>(sqlEstadio);
            if (estadio == null || !estadio.Any())
            {
                return BadRequest($"Estádio não encontrado: {partida.EstadioId}.");
            }
        }
        else
        {
            return BadRequest("EstadioId não pode ser nulo");
        }
        
        string sql = $"EXEC sp_InserirPartida {partida.TimeID}, {partida.JogoId}, {partida.EstadioId}";
        await sqlConnection.ExecuteAsync(sql);
        return Ok();
    }

    [HttpPatch("Partidas")]
    public async Task<IActionResult> Patch(PartidaPostPatch partida,
        [FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Partida WHERE Id = @id";
        var partidaConsulta = await sqlConnection.QueryAsync<Partida>(sql, new { id });
        if (partidaConsulta is null)
        {
            return BadRequest("Partida não encontrada");
        }

        if (partida.TimeID >= 0)
        {
            var sqlTime = $"SELECT * FROM Times WHERE Id = {partida.TimeID}";
            var time = await sqlConnection.QueryAsync<TimeModel>(sqlTime);
            if (time == null || !time.Any())
            {
                return BadRequest($"Time não encontrado: {partida.TimeID}.");
            }
        }

        if (partida.JogoId > 0)
        {
            var sqlJogo = $"SELECT * FROM Jogo WHERE Id = {partida.JogoId}";
            var jogo = await sqlConnection.QueryAsync<Jogo>(sqlJogo);
            if (jogo == null || !jogo.Any())
            {
                return BadRequest($"Jogo não encontrado: {partida.JogoId}.");
            }
        }

        if (partida.EstadioId > 0)
        {
            var sqlEstadio = $"SELECT * FROM Estadios WHERE Id = {partida.EstadioId}";
            var estadio = await sqlConnection.QueryAsync<Estadios>(sqlEstadio);
            if (estadio == null || !estadio.Any())
            {
                return BadRequest($"Estádio não encontrado: {partida.EstadioId}.");
            }
        }
        var sql2 = $"EXEC sp_AtualizarPartida {id}, {partida.TimeID}, {partida.JogoId}, {partida.EstadioId}";
        await sqlConnection.ExecuteAsync(sql2);
        return Ok("Partida atualizada com sucesso");
    }

    [HttpDelete("Partidas")]
    public async Task<IActionResult> DeletePartidas([FromQuery] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync($"EXEC sp_DeletarPartida {id}", new { id });
            return linhaAfetada == 0
                ? NotFound("O id informado não foi encontrado")
                : Ok("Time deletado com sucesso");
        }
    }
    private static bool ValidarAutenticacao(HttpRequest request) => request.Headers.TryGetValue("autentica", out var autentica) && autentica == Autentica;

}