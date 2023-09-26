using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TimesBD.Entities;
using TimesBD.Models;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimesController : ControllerBase
{
    
    private readonly string _connectionString;

    private const string AUTENTICA = "d41d8cd98f00b204e9800998ecf8427e";
    public TimesController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery(Name = "name")]string name = null,[FromQuery(Name = "id")]int? id = null, [FromQuery(Name = "Cep")]string cep = null, [FromHeader(Name = "Autentica")]string autentica = null)
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
        else if (id != null && id > 0)
        {
            filtro = "WHERE Id = @id";
        }
        else if (!String.IsNullOrEmpty(cep))
        {
            filtro = "WHERE Cep = @cep";
        }

        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var sql = $"SELECT * FROM Jogadores {filtro}";
            var jogadores = await sqlConnection.QueryAsync<Jogador>(sql, new { name, id, cep });
            return Ok(jogadores); 
        }
    }

    [HttpPatch]
    public async Task<IActionResult> Patch([FromQuery]int id, JogadorModel atualizaJogador, [FromHeader(Name = "Autentica")]string autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        if(string.IsNullOrEmpty(atualizaJogador.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }

        if(atualizaJogador.DataNascimento > DateTime.Now || atualizaJogador.DataNascimento < DateTime.Now.AddYears(-100))
        {
            return BadRequest("Data de nascimento não pode ser maior que a data atual ou menor que 100 anos atrás");
        } 
        
        if(atualizaJogador.TimeId < 0)
        {
            return BadRequest("TimeId não pode ser menor que zero");
        }

        var query = "UPDATE Jogadores SET DataNascimento = @DataNascimento, TimeId = @TimeId";
        
        var parametros = new DynamicParameters();
        parametros.Add("DataNascimento", atualizaJogador.DataNascimento, DbType.DateTime);
        parametros.Add("TimeId", atualizaJogador.TimeId, DbType.Int32);
        
        if (!string.Equals(atualizaJogador.Nome, "string", StringComparison.OrdinalIgnoreCase))
        {
            query += ", Nome = @Nome";
            parametros.Add("Nome", atualizaJogador.Nome, DbType.String);
        }

        query += " WHERE Id = @Id";
        parametros.Add("Id", id, DbType.Int32);
        
        using (var connection = new SqlConnection(_connectionString))
        {

            await connection.ExecuteAsync(query, parametros);
            return Ok();
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(Jogador jogador, [FromHeader(Name = "Autentica")]string autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var endereco = await ConsultarCEP(jogador.Cep);

            if (endereco is not null)
            {
                jogador.Logradouro = endereco.Logradouro;
                jogador.Complemento = endereco.Complemento;
                jogador.Bairro = endereco.Bairro;
                jogador.Localidade = endereco.Localidade;
                jogador.Uf = endereco.Uf;
                jogador.Ibge = endereco.Ibge;
                jogador.Gia = endereco.Gia;
                jogador.Ddd = endereco.Ddd;
                jogador.Siafi = endereco.Siafi;
                
                if (string.IsNullOrEmpty(jogador.Nome))
                {
                    return BadRequest("Nome não pode ser nulo ou vazio");
                }

                if(jogador.DataNascimento > DateTime.Now || jogador.DataNascimento < DateTime.Now.AddYears(-100))
                {
                    return BadRequest("Data de nascimento não pode ser maior que a data atual ou menor que 100 anos atrás");
                } 
                
                if(jogador.TimeId < 0)
                {
                    return BadRequest("TimeId não pode ser menor que zero");
                }
                
                await sqlConnection.ExecuteAsync("INSERT INTO Jogadores (Nome, DataNascimento, TimeId, Cep, Logradouro, Complemento, Bairro, Localidade, Uf, Ibge, Gia, Ddd, Siafi) VALUES (@Nome, @DataNascimento , @TimeId, @Cep, @Logradouro, @Complemento, @Bairro, @Localidade, @Uf, @Ibge, @Gia, @Ddd, @Siafi)", jogador);
                return Ok(jogador);
            }
            return BadRequest($"CEP inválido: {jogador.Cep}");
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery]int id, [FromHeader(Name = "Autentica")]string autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync("DELETE FROM Jogadores WHERE Id = @id", new { id });
            return linhaAfetada == 0 ? NotFound("O id informado não foi encontrado") : Ok("Jogador deletado com sucesso");
        }
    }
    
    private static async Task<Endereco?> ConsultarCEP(string cep)
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
    
    private static bool ValidarAutenticacao(HttpRequest request)
    {
        var autentica = request.Headers["autentica"];
        if (!request.Headers.ContainsKey("autentica"))
        {
            return false;
        }
        if(request.Headers["autentica"] == AUTENTICA)
        {
            return true;
        }
        return false;
    }
}

