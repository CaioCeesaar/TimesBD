using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TimesBD.Entities;
using TimesBD.Models;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JogadorController : ControllerBase
{
    private readonly string _connectionString;

    private const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";

    public JogadorController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery(Name = "name")] string? name = null,
        [FromQuery(Name = "id")] int? id = null, [FromQuery(Name = "Cep")] string? cep = null,
        [FromHeader(Name = "Autentica")] string? autentica = null)
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

        using var sqlConnection = new SqlConnection(_connectionString);
        var sql =
            $"SELECT J.*, T.Nome AS NomeTime FROM Jogadores J INNER JOIN Endereco E ON (J.EnderecoId = E.Id) JOIN Times T ON (J.TimeId = T.Id) {filtro}";
        var jogadores = await sqlConnection.QueryAsync<Jogador>(sql, new { name, id, cep });

        foreach (var jogador in jogadores)
        {
            var enderecoJogador = jogador.EnderecoId;
            var sqlEndereco = $"SELECT * FROM Endereco WHERE Id = {enderecoJogador}";
            var enderecoConsulta = await sqlConnection.QueryAsync<Endereco>(sqlEndereco);
            jogador.EnderecoJogador = enderecoConsulta.ToList()[0];
        }

        return Ok(jogadores);
    }

    [HttpPatch]
    // do the patch request here
    public async Task<IActionResult> Patch([FromQuery] int id, JogadorModel atualizaJogador,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
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

        if (!String.IsNullOrEmpty(atualizaJogador.Nome))
        {
            sql = $"UPDATE Jogadores SET Nome = @Nome WHERE Id = @id";
            await sqlConnection.ExecuteAsync(sql, new { atualizaJogador.Nome, id });
        }

        if (atualizaJogador.DataNascimento != null)
        {
            sql = $"UPDATE Jogadores SET DataNascimento = @DataNascimento WHERE Id = @id";
            await sqlConnection.ExecuteAsync(sql, new { atualizaJogador.DataNascimento, id });
        }

        if (atualizaJogador.TimeId != null)
        {
            sql = $"UPDATE Jogadores SET TimeId = @TimeId WHERE Id = @id";
            await sqlConnection.ExecuteAsync(sql, new { atualizaJogador.TimeId, id });
        }

        if (atualizaJogador.EnderecoId != null)
        {
            sql = $"UPDATE Jogadores SET EnderecoId = @EnderecoId WHERE Id = @id";
            await sqlConnection.ExecuteAsync(sql, new { atualizaJogador.EnderecoId, id });
        }

        if (atualizaJogador.EnderecoModeloJogador.Cep != null)
        {
            var endereco = await ConsultarCep(atualizaJogador.EnderecoModeloJogador.Cep);
            sql = $"UPDATE Endereco SET Cep = '{endereco.Cep}', Logradouro = '{endereco.Logradouro}', Complemento = '{endereco.Complemento}', Bairro = '{endereco.Bairro}', Localidade = '{endereco.Localidade}', Uf = '{endereco.Uf}', Ibge = '{endereco.Ibge}', Gia = '{endereco.Gia}', Ddd = '{endereco.Ddd}', Siafi = '{endereco.Siafi}' WHERE Id = {atualizaJogador.EnderecoId}";
            await sqlConnection.ExecuteAsync(sql);
        }

        return Ok("Jogador atualizado com sucesso");
    }


    [HttpPost]
    public async Task<IActionResult> Post(Jogador jogador, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }

        using var sqlConnection = new SqlConnection(_connectionString);


        //if (string.IsNullOrEmpty(jogador.Nome))
        //{
        //    return BadRequest("Nome não pode ser nulo ou vazio");
        //}

        //if (jogador.DataNascimento > DateTime.Now || jogador.DataNascimento < DateTime.Now.AddYears(-100))
        //{
        //    return BadRequest("Data de nascimento não pode ser maior que a data atual ou menor que 100 anos atrás");
        //}

        //if (jogador.TimeId < 0)
        //{
        //    return BadRequest("TimeId não pode ser menor que zero");
        //}
        var endereco = await ConsultarCep(jogador.EnderecoJogador.Cep);


        if (endereco is not null)
        {
            endereco.Localidade = endereco.Localidade.Replace("'", "''");
            string sql =
                $"INSERT INTO Endereco (Cep, Logradouro, Complemento, Bairro, Localidade, Uf, Ibge, Gia, Ddd, Siafi) OUTPUT INSERTED.Id VALUES ('{endereco.Cep}', '{endereco.Logradouro}', '{endereco.Complemento}','{endereco.Bairro}', '{endereco.Localidade}', '{endereco.Uf}', '{endereco.Ibge}', '{endereco.Gia}', '{endereco.Ddd}', '{endereco.Siafi}')";
            int linhaAfetada = await sqlConnection.ExecuteScalarAsync<int>(sql);

            if (linhaAfetada != 0)
            {
                jogador.EnderecoId = linhaAfetada;
                await sqlConnection.ExecuteAsync(
                    "INSERT INTO Jogadores (Nome, DataNascimento, TimeId, EnderecoId) VALUES (@Nome, @DataNascimento, @TimeId, @EnderecoId)",
                    jogador);
            }
        }
        else
        {
            return BadRequest($"CEP inválido: {jogador}");
        }

        return Ok(jogador);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }

        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync("DELETE FROM Jogadores WHERE Id = @id", new { id });
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

    private static bool ValidarAutenticacao(HttpRequest request)
    {
        var autentica = request.Headers["autentica"];
        if (!request.Headers.ContainsKey("autentica"))
        {
            return false;
        }

        if (autentica == Autentica)
        {
            return true;
        }

        return false;
    }
}