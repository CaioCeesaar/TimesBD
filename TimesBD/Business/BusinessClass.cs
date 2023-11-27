using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using TimesBD.Entities;
using TimesBD.Repositories;

namespace TimesBD.Business;

public class BusinessClass
{
    public const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";

    private readonly string _connectionString;

    private readonly SqlRep _sqlRep;
    public BusinessClass(string connectionString)
    {
        _sqlRep = new(connectionString);
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

        return await _sqlRep.GetJogadorByIdAsync(sql);
       
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

        return await _sqlRep.GetCompradorByIdAsync(sql);
       
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

        return await _sqlRep.GetEstadiosByIdAsync(sql);
       
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

        return await _sqlRep.GetIngressoByIdAsync(sql);
       
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

        return await _sqlRep.GetPartidaByIdAsync(sql);
       
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

        return await _sqlRep.GetTimeByIdAsync(sql);
       
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

        return await _sqlRep.GetJogoByIdAsync(sql);
       
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

        return await _sqlRep.GetVendaByIdAsync(sql);
       
    }
        
    public static bool ValidarAutenticacao(HttpRequest request) => request.Headers.TryGetValue("autentica", out var autentica) && autentica == Autentica; 
}