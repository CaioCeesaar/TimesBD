using Dapper;
using System.Data;
using System.Data.SqlClient;
using TimesBD.Business;

namespace TimesBD.Repositories;

public class SqlRep
{
    private readonly SqlConnection _connection;
    public SqlRep(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
        Reconnect();
    }

    public async Task<IEnumerable<T>> GetQueryAsync<T> (string select) where T : class
    {
        Reconnect();
        return await _connection.QueryAsync<T>(select);
    }
    
    public async Task<Result> PostQueryAsync(string insert)
    {
        try
        {
            Reconnect();
            var ins = await _connection.ExecuteAsync(insert);
      
            return new(true, "Ok");
        }
        catch (Exception ex)
        {
            return new(false, "Deu ruim");
        }
    }
    
    public async Task<string> PatchQueryAsync(string update)
    {
        try
        {
            Reconnect();
            var upd = await _connection.ExecuteAsync(update);
            return upd.ToString();
        }
        catch (DBConcurrencyException exConcurrency)
        {
            return $"{exConcurrency.Message} - {exConcurrency.StackTrace}";
        }
        catch (SqlException exSql)
        {
            return $"{exSql.Message} - {exSql.StackTrace}";
        }
        catch (Exception ex)
        {
            return $"{ex.Message} - {ex.StackTrace}";
        }
    }

    public async Task<string> DeleteQueryAsync(string delete)
    {
        try
        {
            Reconnect();
            var del = await _connection.QueryAsync(delete);
            return del.ToString();
        }
        catch (Exception ex)
        {
            return $"{ex.Message} - {ex.StackTrace}";
        }
    }

    private void Reconnect()
    {
        if (_connection.State == ConnectionState.Closed)
        {
            _connection.Open();
        }
    }
}