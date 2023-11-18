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
}