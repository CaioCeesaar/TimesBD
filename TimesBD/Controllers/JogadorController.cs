using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TimesBD.Entities;
using TimesBD.Business;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JogadorController : ControllerBase
{
    private readonly string _connectionString;

    private readonly BusinessClass _businessClass;
    public JogadorController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new(_connectionString);
    }

    [HttpGet]
    public async Task<IActionResult> GetJogadoresById(
        [FromQuery(Name = "id")] int? id = null
        , [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        var getJogador = await _businessClass.GetJogadorByIdAsync(autentica, id);
        return Ok(getJogador);
    }

    [HttpPatch]
    public async Task<IActionResult> Patch([FromQuery] int id, JogadorModel atualizaJogador,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!BusinessClass.ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }

        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Jogadores WHERE Id = @id";
        var jogador = await sqlConnection.QueryAsync<Jogador>(sql, new { id });
        if (jogador is null)
        {
            return BadRequest("Jogador não encontrado");
        }

        if (String.IsNullOrEmpty(atualizaJogador.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }
        
        var endereco = await ConsultarCep(atualizaJogador.Cep);
        if (endereco is null)
        {
            return BadRequest("Cep inválido");
        }
        endereco.Localidade = endereco.Localidade.Replace("'", "''");
        
        var sql2 = $"EXEC sp_AtualizarJogador {id}, '{atualizaJogador.Nome}', '{atualizaJogador.DataNascimento}', '{atualizaJogador.TimeId}', '{atualizaJogador.Cep}', '{endereco.Complemento}', '{endereco.Bairro}', '{endereco.Localidade}', '{endereco.Uf}', '{endereco.Ibge}', '{endereco.Gia}', '{endereco.Ddd}', '{endereco.Siafi}', '{endereco.Logradouro}'";
        await sqlConnection.ExecuteAsync(sql2);
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(JogadorModel jogador, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!BusinessClass.ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }

        using var sqlConnection = new SqlConnection(_connectionString);
        // verificar se o time existe
        
        
        if (string.IsNullOrEmpty(jogador.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }

        if (jogador.DataNascimento > DateTime.Now || jogador.DataNascimento < DateTime.Now.AddYears(-100))
        {
            return BadRequest("Data de nascimento não pode ser maior que a data atual ou menor que 100 anos atrás");
        }
        
        if (jogador.TimeId != null)
        {
            if (jogador.TimeId <= 0)
            {
                return BadRequest("TimeId não pode ser menor ou igual a zero");
            }
            
            var sqlTime = $"SELECT * FROM Times WHERE Id = {jogador.TimeId}";
            var time = await sqlConnection.QueryAsync<TimeModel>(sqlTime);
            if (time == null || !time.Any())
            {
                return BadRequest($"Time não encontrado: {jogador.TimeId}.");
            }
        }
        else
        {
            return BadRequest("TimeId não pode ser nulo");
        }
        

        var endereco = await ConsultarCep(jogador.Cep);
        if (endereco?.Cep is not null)
        {
            endereco.Localidade = endereco.Localidade.Replace("'", "''");
            string sql = $"EXEC sp_InserirJogador '{jogador.Nome}', '{jogador.DataNascimento}', {jogador.TimeId}, '{jogador.Cep}', '{endereco.Complemento}', '{endereco.Bairro}', '{endereco.Localidade}', '{endereco.Uf}', '{endereco.Ibge}', '{endereco.Gia}', '{endereco.Ddd}', '{endereco.Siafi}', '{endereco.Logradouro}'";
            await sqlConnection.ExecuteScalarAsync<int>(sql);
            return Ok();
        }
        return BadRequest("Cep inválido");
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!BusinessClass.ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }

        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync("EXEC sp_DeletarJogador {id}", new { id });
            return linhaAfetada == 0
                ? NotFound("O id informado não foi encontrado")
                : Ok("Jogador deletado com sucesso");
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

}