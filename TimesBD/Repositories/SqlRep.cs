using Dapper;
using System.Data.SqlClient;
using TimesBD.Entities;

namespace TimesBD.Repositories;

public class SqlRep
{
    private readonly SqlConnection _connection;
    public SqlRep(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
    }

    public async Task<IEnumerable<T>> GetQueryAsync<T> (string select) where T : class
    {
        _connection.Close();
        _connection.Open();
        return await _connection.QueryAsync<T>(select);
    }
    
    public async Task InserirJogadorAsync(string insert)
    {
        _connection.Close();
        _connection.Open();
        await _connection.ExecuteAsync(insert);
    }

    public async Task DeletarJogadorAsync(string delete)
    {
        _connection.Close();
        _connection.Open();
        await _connection.QueryAsync(delete);
    }
}