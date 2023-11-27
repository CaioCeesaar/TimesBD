using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompradorController : ControllerBase
{
    private readonly string _connectionString;
    
    private readonly BusinessClass _businessClass;

    private const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";

    public CompradorController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(_connectionString);        
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCompradoresById(
        [FromQuery(Name = "id")] int? id = null
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var getComprador = await _businessClass.GetCompradorByIdAsync(autentica, id);
        return Ok(getComprador);
    }

    [HttpPost]
    public async Task<IActionResult> PostCompradores(CompradorPostPatch comprador,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);

        if (string.IsNullOrEmpty(comprador.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }
        
        if (string.IsNullOrEmpty(comprador.CPF))
        {
            return BadRequest("CPF não pode ser nulo ou vazio");
        }
        
        if (comprador.CPF.Length != 11)
        {
            return BadRequest("CPF inválido");
        }
        
        var cpfExiste = $"SELECT * FROM Comprador WHERE CPF = '{comprador.CPF}'";
        var cpfConsulta = await sqlConnection.QueryAsync<Comprador>(cpfExiste);
        if (cpfConsulta != null && cpfConsulta.Any())
        {
            return BadRequest("CPF já cadastrado");
        }
        
        string sql = $"EXEC sp_InserirComprador '{comprador.Nome}', '{comprador.CPF}'";
        await sqlConnection.ExecuteAsync(sql);
        return Ok();
    }
    
    [HttpPatch]
    public async Task<IActionResult> Patch(CompradorPostPatch comprador,
        [FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Comprador WHERE Id = @id";
        var compradorConsulta = await sqlConnection.QueryAsync<Comprador>(sql, new { id });
        if (compradorConsulta is null)
        {
            return BadRequest("Comprador não encontrado");
        }

        if (String.IsNullOrEmpty(comprador.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }
        var parameters = $"{id}";
        if (!string.Equals(comprador.Nome, "string", StringComparison.OrdinalIgnoreCase))
        {
            parameters += $", '{comprador.Nome}'";
        }

        if (String.IsNullOrEmpty(comprador.CPF))
        {
            return BadRequest("CPF não pode ser nulo ou vazio");
        }
        
        if (comprador.CPF.Length != 11)
        {
            return BadRequest("CPF inválido");
        }
        
        var cpfExiste = $"SELECT * FROM Comprador WHERE CPF = '{comprador.CPF}'";
        var cpfConsulta = await sqlConnection.QueryAsync<Comprador>(cpfExiste);
        if (cpfConsulta != null && cpfConsulta.Any())
        {
            return BadRequest("CPF já cadastrado");
        }
        
        sql = $"EXEC sp_AtualizarComprador {parameters}, '{comprador.CPF}'";
        await sqlConnection.ExecuteAsync(sql);
        return Ok("Comprador atualizado com sucesso");
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteCompradores([FromQuery] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");
        
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync($"EXEC sp_DeletarComprador {id}", new { id });
            return linhaAfetada == 0
                ? NotFound("O id informado não foi encontrado")
                : Ok("Comprador deletado com sucesso");
        }
    }
    
    private static bool ValidarAutenticacao(HttpRequest request) => request.Headers.TryGetValue("autentica", out var autentica) && autentica == Autentica;

}