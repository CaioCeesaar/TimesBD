using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using TimesBD.Entities;
using TimesBD.Models;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimeController : Controller
{
    private readonly string _connectionString;
    
    public TimeController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery(Name = "name")] string name = null)
    {
        string filtro = "";
        if (!String.IsNullOrEmpty(name))
        {
            filtro = "WHERE Nome = @name";
        }

        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var sql = $"SELECT * FROM Times {filtro}";
            var times = await sqlConnection.QueryAsync<Times>(sql, new { name });
            return Ok(times);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(Times time)
    {
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            if (string.IsNullOrEmpty(time.Nome))
            {
                return BadRequest("Nome não pode ser nulo ou vazio");
            }
            var sql = $"INSERT INTO Times (Nome, JogadorId) VALUES (@Nome, @JogadorId)";
            await sqlConnection.ExecuteAsync(sql, time);
            return Ok(time);
        }
    }

    [HttpPatch]
    public async Task<IActionResult> Patch([FromQuery] int id, TimeModel atualizaTime)
    {
        if (string.IsNullOrEmpty(atualizaTime.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }
        
        var query = "UPDATE Times SET JogadorId = @JogadorId";
        var parametros = new DynamicParameters();
        parametros.Add("JogadorId", atualizaTime.JogadorId, DbType.Int32);
        if(!string.Equals(atualizaTime.Nome, "string", StringComparison.OrdinalIgnoreCase))
        {
            query += ", Nome = @Nome";
            parametros.Add("Nome", atualizaTime.Nome, DbType.String);
        }
        query += " WHERE Id = @Id";
        parametros.Add("Id", id, DbType.Int32);
        
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync(query, parametros);
            return Ok();
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync("DELETE FROM Times WHERE Id = @Id", new { id });
            return linhaAfetada == 0 ? NotFound("O time não foi encontrado") : Ok("Time deletado com sucesso");
        }
    }
    
}