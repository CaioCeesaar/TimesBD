using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimesController : ControllerBase
{
    
    private readonly string _connectionString;
    
    public TimesController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            const string sql = "SELECT * FROM Jogadores";
            var jogadores = await sqlConnection.QueryAsync<Jogador>(sql);
            return Ok(jogadores);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            const string sql = "SELECT * FROM Jogadores WHERE Id = @id";
            var jogador = await sqlConnection.QuerySingleOrDefaultAsync<Jogador>(sql, new { id });
            if (jogador is null)
            {
                return NotFound();
            }
            return Ok(jogador);
        }
    }
    
    [HttpGet("cep/{cep}")]
    public async Task<IActionResult> GetByCEP(string cep)
    {
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            const string sql = "SELECT * FROM Jogadores WHERE Cep = @cep";
            var jogadores = await sqlConnection.QueryAsync<Jogador>(sql, new { cep });
            if (jogadores is null || !jogadores.Any())
            {
                return NotFound();
            }
    
            foreach (var jogador in jogadores)
            {
                var endereco = await ConsultarCEP(cep);
    
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
                }
            }
    
            return Ok(jogadores);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(Jogador jogador)
    {
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
                
                await sqlConnection.ExecuteAsync("INSERT INTO Jogadores (Nome, Idade, Time_id, Cep, Logradouro, Complemento, Bairro, Localidade, Uf, Ibge, Gia, Ddd, Siafi) VALUES (@Nome, @Idade, @Time_id, @Cep, @Logradouro, @Complemento, @Bairro, @Localidade, @Uf, @Ibge, @Gia, @Ddd, @Siafi)", jogador);
                
                return Ok(jogador);
            }

            return BadRequest($"CEP inválido: {jogador.Cep}");
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
    
}

