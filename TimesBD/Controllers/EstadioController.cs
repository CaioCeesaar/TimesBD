﻿using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TimesBD.Entities;

namespace TimesBD.Controllers;

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
        }
        else if (id != null && id > 0)
        {
            filtro = "WHERE Id = @id";
        }

        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Estadios {filtro}";
        var estadios = await sqlConnection.QueryAsync<Estadios>(sql, new { name, cep, id });
        return Ok(estadios);
    }

    [HttpPost]
    public async Task<IActionResult> PostEstadios(EstadiosModel estadio, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }

        using var sqlConnection = new SqlConnection(_connectionString);

        if (string.IsNullOrEmpty(estadio.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }

        if (estadio.Limite <= 0)
        {
            return BadRequest("O limite do estádio não pode ser menor ou igual a zero");
        }

        var endereco = await ConsultarCep(estadio.Cep);
        if (endereco?.Cep is not null)
        {
            endereco.Localidade = endereco.Localidade.Replace("'", "''");
            string sql = $"INSERT INTO Estadios (Nome, Limite, Cep, Complemento, Bairro, Localidade, Uf, Ibge, Gia, Ddd, Siafi, Logradouro) OUTPUT INSERTED.Id VALUES ('{estadio.Nome}', {estadio.Limite}, '{endereco.Cep}', '{endereco.Complemento}','{endereco.Bairro}', '{endereco.Localidade}', '{endereco.Uf}', '{endereco.Ibge}', '{endereco.Gia}', '{endereco.Ddd}', '{endereco.Siafi}', '{endereco.Logradouro}')";
            await sqlConnection.ExecuteScalarAsync<int>(sql);
            return Ok();
        }

        return BadRequest("Cep inválido");
    }

    [HttpPatch]
    public async Task<IActionResult> Patch([FromQuery] int id, EstadiosModel estadio,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Estadios WHERE Id = @id";
        var estadioConsulta = await sqlConnection.QueryAsync<Estadios>(sql, new { id });
        if (estadioConsulta is null)
        {
            return BadRequest("Estádio não encontrado");
        }

        if (String.IsNullOrEmpty(estadio.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }

        if (estadio.Limite <= 0)
        {
            return BadRequest("Limite inválido, não pode ser menor ou igual a zero");
        }


        var endereco = await ConsultarCep(estadio.Cep);
        if (endereco is null)
        {
            return BadRequest("Cep inválido");
        }
        endereco.Localidade.Replace("'", "''");

        var sql2 = $"UPDATE Estadios SET Nome = '{estadio.Nome}', Limite = {estadio.Limite}, Cep = '{estadio.Cep}', Complemento = '{endereco.Complemento}', Bairro = '{endereco.Bairro}', Localidade = '{endereco.Localidade}', Uf = '{endereco.Uf}', Ibge = '{endereco.Ibge}', Gia = '{endereco.Gia}', Ddd = '{endereco.Ddd}', Siafi = '{endereco.Siafi}', Logradouro = '{endereco.Logradouro}' WHERE Id = {id}";
        await sqlConnection.ExecuteAsync(sql2);
        return Ok("Estadio atualizado com sucesso");
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteEstadios([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync($"EXEC sp_DeletarEstadio {id}", new { id });
            return linhaAfetada == 0
                ? NotFound("O id informado não foi encontrado")
                : Ok("Estádio deletado com sucesso");
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