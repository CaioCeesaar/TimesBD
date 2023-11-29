using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using TimesBD.Entities;
using TimesBD.Repositories;

namespace TimesBD.Business;

public class BusinessClass
{
    public const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";

    private readonly string _connectionString;

    private readonly SqlRep _sqlRep;

    private readonly ApiRep _apiRep;

    public BusinessClass(string connectionString)
    {
        _sqlRep = new(connectionString);
        _apiRep = new();
    }

    public async Task<IEnumerable<Jogador>> GetJogadorByIdAsync(string? autentica, int? id)
    {
        //if (!BusinessClass.ValidarAutenticacao(Request))
        //{
        //    ("Autenticação inválida");
        //}
        var sql = $"SELECT * FROM Jogadores ";
        if (id is not null)
        {
            sql += $"WHERE Id = {id}";
        }

        return await _sqlRep.GetQueryAsync<Jogador>(sql);
       
    }
    
    public async Task<IEnumerable<Comprador>> GetCompradorByIdAsync(string? autentica, int? id)
    {
        //if (!BusinessClass.ValidarAutenticacao(Request))
        //{
        //    ("Autenticação inválida");
        //}
        var sql = $"SELECT * FROM Comprador ";
        if (id is not null)
        {
            sql += $"WHERE Id = {id}";
        }

        return await _sqlRep.GetQueryAsync<Comprador>(sql);
       
    }
    
    public async Task<IEnumerable<Estadios>> GetEstadiosByIdAsync(string? autentica, int? id)
    {
        //if (!BusinessClass.ValidarAutenticacao(Request))
        //{
        //    ("Autenticação inválida");
        //}
        var sql = $"SELECT * FROM Estadios ";
        if (id is not null)
        {
            sql += $"WHERE Id = {id}";
        }

        return await _sqlRep.GetQueryAsync<Estadios>(sql);
       
    }
    
    public async Task<IEnumerable<Ingresso>> GetIngressoByIdAsync(string? autentica, int? id)
    {
        //if (!BusinessClass.ValidarAutenticacao(Request))
        //{
        //    ("Autenticação inválida");
        //}
        var sql = $"SELECT * FROM Ingresso ";
        if (id is not null)
        {
            sql += $"WHERE Id = {id}";
        }

        return await _sqlRep.GetQueryAsync<Ingresso>(sql);
       
    }
    
    public async Task<IEnumerable<Partida>> GetPartidaByIdAsync(string? autentica, int? id)
    {
        //if (!BusinessClass.ValidarAutenticacao(Request))
        //{
        //    ("Autenticação inválida");
        //}
        var sql = $"SELECT * FROM Partida ";
        if (id is not null)
        {
            sql += $"WHERE Id = {id}";
        }

        return await _sqlRep.GetQueryAsync<Partida>(sql);
       
    }
    
    public async Task<IEnumerable<Times>> GetTimeByIdAsync(string? autentica, int? id)
    {
        //if (!BusinessClass.ValidarAutenticacao(Request))
        //{
        //    ("Autenticação inválida");
        //}
        var sql = $"SELECT * FROM Times ";
        if (id is not null)
        {
            sql += $"WHERE Id = {id}";
        }

        return await _sqlRep.GetQueryAsync<Times>(sql);
       
    }
    
    public async Task<IEnumerable<Jogo>> GetJogoByIdAsync(string? autentica, int? id)
    {
        //if (!BusinessClass.ValidarAutenticacao(Request))
        //{
        //    ("Autenticação inválida");
        //}
        var sql = $"SELECT * FROM Jogo ";
        if (id is not null)
        {
            sql += $"WHERE Id = {id}";
        }

        return await _sqlRep.GetQueryAsync<Jogo>(sql);
       
    }
    
    public async Task<IEnumerable<Vendas>> GetVendaByIdAsync(string? autentica, int? id)
    {
        //if (!BusinessClass.ValidarAutenticacao(Request))
        //{
        //    ("Autenticação inválida");
        //}
        var sql = $"SELECT * FROM Vendas ";
        if (id is not null)
        {
            sql += $"WHERE Id = {id}";
        }

        return await _sqlRep.GetQueryAsync<Vendas>(sql);
       
    }
        
    public async Task InserirJogadorAsync(string nome, DateTime dataNascimento, int timeId, string cep)
    {
        var endereco = await _apiRep.ConsultarCep(cep);
        if (endereco is not null)
        {
            endereco.Localidade = endereco.Localidade.Replace("'", "''");
            var sql = $"EXEC sp_InserirJogador '{nome}', {dataNascimento}, {timeId}, {cep}, {endereco.Complemento}, {endereco.Bairro}, {endereco.Localidade}, {endereco.Uf}, {endereco.Ibge}, {endereco.Gia}, {endereco.Ddd}, {endereco.Siafi}, {endereco.Logradouro}";
            
            await _sqlRep.InserirJogadorAsync(sql);
        }
    }
    
    public async Task DeletarJogadorAsync(int id)
    {
        var sql = $"EXEC sp_DeletarJogador {id}";
        await _sqlRep.DeletarJogadorAsync(sql);
    }
    
    public static bool ValidarAutenticacao(HttpRequest request) => request.Headers.TryGetValue("autentica", out var autentica) && autentica == Autentica; 
}