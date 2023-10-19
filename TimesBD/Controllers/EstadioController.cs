using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstadioController : ControllerBase
{
    private readonly string _connectionString;
    
    private const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";
    
    public EstadioController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery(Name = "id")] int? id = null, [FromQuery(Name = "name")] string? name = null,
        [FromQuery(Name = "Cep")] string? cep = null, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        string filtro = "";
        if (!String.IsNullOrEmpty(name))
        {
            filtro = "WHERE Nome = @name";
        }
        else if (!String.IsNullOrEmpty(cep))
        {
            filtro = "WHERE Cep = @cep";
        } else if (id != null && id > 0)
        {
            filtro = "WHERE Id = @id";
        }

        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT E.* FROM Estadios E INNER JOIN Endereco EN ON (E.EnderecoId = EN.Id) {filtro}";
        var estadios = await sqlConnection.QueryAsync<Estadios>(sql, new { name, id, cep });
        
        foreach (var estadio in estadios)
        {
            var enderecoEstadio = estadio.EnderecoId;
            var sqlEndereco = $"SELECT * FROM Endereco WHERE Id = {enderecoEstadio}";
            var enderecoConsulta = await sqlConnection.QueryAsync<Endereco>(sqlEndereco);
            estadio.EnderecoEstadio = enderecoConsulta.ToList()[0];
        }
        return Ok(estadios);
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