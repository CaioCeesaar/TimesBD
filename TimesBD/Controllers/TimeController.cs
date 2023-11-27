using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimeController : Controller
{
    private readonly string _connectionString;
    
    private readonly BusinessClass _businessClass;
    
    private const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";
    public TimeController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(_connectionString);
    }

    [HttpGet]
    public async Task<IActionResult> GetTimesById(
        [FromQuery(Name = "id")] int? id = null
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var getTime = await _businessClass.GetTimeByIdAsync(autentica, id);
        return Ok(getTime);
    }

    [HttpPatch]
    public async Task<IActionResult> Patch([FromQuery] int id, TimeModel atualizaTime,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Times WHERE Id = @id";
        var time = await sqlConnection.QueryAsync<Times>(sql, new { id });
        if (time is null)
        {
            return BadRequest("Time não encontrado");
        }
        
        if (String.IsNullOrEmpty(atualizaTime.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }
        
        var endereco = await ConsultarCep(atualizaTime.Cep);
        if (endereco is null)
        {
            return BadRequest("Cep inválido");
        }
        endereco.Localidade = endereco.Localidade.Replace("'", "''");

        var sql2 = $"EXEC sp_AtualizarTime {id}, '{atualizaTime.Nome}', '{atualizaTime.Cep}', '{endereco.Complemento}', '{endereco.Bairro}', '{endereco.Localidade}', '{endereco.Uf}', '{endereco.Ibge}', '{endereco.Gia}', '{endereco.Ddd}', '{endereco.Siafi}', '{endereco.Logradouro}'";
        await sqlConnection.ExecuteAsync(sql2);
        return Ok("Time atualizado com sucesso");
    }

    [HttpPost]
    public async Task<IActionResult> Post(TimeModel time, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        
        if (string.IsNullOrEmpty(time.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }
        
        var endereco = await ConsultarCep(time.Cep);
        if (endereco?.Cep is not null)
        {
            endereco.Localidade = endereco.Localidade.Replace("'", "''");

            string sql = $"EXEC sp_InserirTime '{time.Nome}', '{time.Cep}', '{endereco.Complemento}', '{endereco.Bairro}', '{endereco.Localidade}', '{endereco.Uf}', '{endereco.Ibge}', '{endereco.Gia}', '{endereco.Ddd}', '{endereco.Siafi}', '{endereco.Logradouro}'";
            await sqlConnection.ExecuteScalarAsync<int>(sql);
            return Ok();
        }
        return BadRequest("Cep inválido");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTimes([FromQuery] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        CommandDefinition cDTeste = new CommandDefinition($"EXEC sp_DeletarTime {id}");
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync(cDTeste);
            return linhaAfetada == 0
                ? BadRequest("O id informado não foi encontrado")
                : Ok("Time deletado com sucesso");
        }
    }
    
    private static async Task<Endereco?> ConsultarCep(string cep)
    {
        var client = new HttpClient();
        var url = $"https://viacep.com.br/ws/{cep}/json/";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var endereco = JsonConvert.DeserializeObject<Endereco>(content);
            return endereco;
        }
        return null;
    }
    
    private static bool ValidarAutenticacao(HttpRequest request) => request.Headers.TryGetValue("autentica", out var autentica) && autentica == Autentica;

}