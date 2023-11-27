using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using TimesBD.Entities;

namespace TimesBD.Repositories;

public class SqlRep
{
    private readonly SqlConnection _connection;
    public SqlRep(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
    }

    public async Task<IEnumerable<Jogador>> GetJogadorByIdAsync(string select)
    {
        _connection.Close();
        _connection.Open();
        return await _connection.QueryAsync<Jogador>(select);
    }
    
    public async Task<IEnumerable<Comprador>> GetCompradorByIdAsync(string select)
    {
        _connection.Close();
        _connection.Open();
        return await _connection.QueryAsync<Comprador>(select);
    }
    
    public async Task<IEnumerable<Estadios>> GetEstadiosByIdAsync(string select)
    {
        _connection.Close();
        _connection.Open();
        return await _connection.QueryAsync<Estadios>(select);
    }
    
    public async Task<IEnumerable<Ingresso>> GetIngressoByIdAsync(string select)
    {
        _connection.Close();
        _connection.Open();
        return await _connection.QueryAsync<Ingresso>(select);
    }
    
    public async Task<IEnumerable<Partida>> GetPartidaByIdAsync(string select)
    {
        _connection.Close();
        _connection.Open();
        return await _connection.QueryAsync<Partida>(select);
    }
    
    public async Task<IEnumerable<Times>> GetTimeByIdAsync(string select)
    {
        _connection.Close();
        _connection.Open();
        return await _connection.QueryAsync<Times>(select);
    }
    
    public async Task<IEnumerable<Jogo>> GetJogoByIdAsync(string select)
    {
        _connection.Close();
        _connection.Open();
        return await _connection.QueryAsync<Jogo>(select);
    }
    
    public async Task<IEnumerable<Vendas>> GetVendaByIdAsync(string select)
    {
        _connection.Close();
        _connection.Open();
        return await _connection.QueryAsync<Vendas>(select);
    }
}