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
    
    public async Task<(Result, IEnumerable<T>?)> GetEstadiosAsync<T>() where T : class
    {
        try
        {
            Reconnect();
            var estadios = await _connection.QueryAsync<T>(SProc.GetEstadios, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), estadios);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
    }
    
    public async Task<(Result, IEnumerable<T>?)> GetJogadoresAsync<T>() where T : class
    {
        try
        {
            Reconnect();
            var jogadores = await _connection.QueryAsync<T>(SProc.GetJogadores, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), jogadores);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
    }
    
    public async Task<(Result, IEnumerable<T>?)> GetCompradoresAsync<T>() where T : class
    {
        try
        {
            Reconnect();
            var compradores = await _connection.QueryAsync<T>(SProc.GetCompradores, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), compradores);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
    }
    
    public async Task<(Result, IEnumerable<T>?)> GetIngressosAsync<T>() where T : class
    {
        try
        {
            Reconnect();
            var ingressos = await _connection.QueryAsync<T>(SProc.GetIngressos, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), ingressos);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
    }
    
    public async Task<(Result, IEnumerable<T>?)> GetJogosAsync<T>() where T : class
    {
        try
        {
            Reconnect();
            var jogos = await _connection.QueryAsync<T>(SProc.GetJogos, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), jogos);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
    }
    
    public async Task<(Result, IEnumerable<T>?)> GetPartidasAsync<T>() where T : class
    {
        try
        {
            Reconnect();
            var partidas = await _connection.QueryAsync<T>(SProc.GetPartidas, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), partidas);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
    }
    
    public async Task<(Result, IEnumerable<T>?)> GetTimesAsync<T>() where T : class
    {
        try
        {
            Reconnect();
            var times = await _connection.QueryAsync<T>(SProc.GetTimes, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), times);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
    }
    
    public async Task<(Result, IEnumerable<T>?)> GetVendasAsync<T>() where T : class
    {
        try
        {
            Reconnect();
            var vendas = await _connection.QueryAsync<T>(SProc.GetVendas, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), vendas);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
    }

    public async Task<(Result, IEnumerable<T>?)> GetJogadorAsync<T>(int id) where T : class
    {
        try
        {
            DynamicParameters dynamicParameters = new();
            dynamicParameters.Add("@Id", id);

            Reconnect();
            var jogador = await _connection.QueryAsync<T>(SProc.GetJogador, dynamicParameters, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), jogador);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
    }
    
    public async Task<(Result, IEnumerable<T>?)> GetCompradorAsync<T>(int id) where T : class
    {
        try
        {
            DynamicParameters dynamicParameters = new();
            dynamicParameters.Add("@Id", id);

            Reconnect();
            var comprador = await _connection.QueryAsync<T>(SProc.GetComprador, dynamicParameters, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), comprador);
        }
        catch (Exception)
        {
            return (new Result(false, "deu ruim"), null);
        }
    } 
    
    public async Task<(Result, IEnumerable<T>?)> GetIngressoAsync<T>(int id) where T : class
    {
        try
        {
            DynamicParameters dynamicParameters = new();
            dynamicParameters.Add("@Id", id);

            Reconnect();
            var ingresso = await _connection.QueryAsync<T>(SProc.GetIngresso, dynamicParameters, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), ingresso);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
    }
    
    public async Task<(Result, IEnumerable<T>?)> GetJogoAsync<T>(int id) where T : class
    {
        try
        {
            DynamicParameters dynamicParameters = new();
            dynamicParameters.Add("@Id", id);

            Reconnect();
            var jogo = await _connection.QueryAsync<T>(SProc.GetJogo, dynamicParameters, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), jogo);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
    }
    
    public async Task<(Result, IEnumerable<T>?)> GetEstadioAsync<T>(int id) where T : class
    {
        try
        {
            DynamicParameters dynamicParameters = new();
            dynamicParameters.Add("@Id", id);

            Reconnect();
            var estadio = await _connection.QueryAsync<T>(SProc.GetEstadio, dynamicParameters, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), estadio);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
    }
    
    public async Task<(Result, IEnumerable<T>?)> GetPartidaAsync<T>(int id) where T : class
    {
        try
        {
            DynamicParameters dynamicParameters = new();
            dynamicParameters.Add("@Id", id);

            Reconnect();
            var partida = await _connection.QueryAsync<T>(SProc.GetPartida, dynamicParameters, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), partida);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
    }
    
    public async Task<(Result, IEnumerable<T>?)> GetTimeAsync<T>(int id) where T : class
    {
        try
        {
            DynamicParameters dynamicParameters = new();
            dynamicParameters.Add("@Id", id);

            Reconnect();
            var time = await _connection.QueryAsync<T>(SProc.GetTime, dynamicParameters, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), time);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
    }
    
    public async Task<(Result, IEnumerable<T>?)> GetVendaAsync<T>(int id) where T : class
    {
        try
        {
            DynamicParameters dynamicParameters = new();
            dynamicParameters.Add("@Id", id);

            Reconnect();
            var venda = await _connection.QueryAsync<T>(SProc.GetVenda, dynamicParameters, commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), venda);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
    }

    public async Task<(Result, IEnumerable<T>?)> GetAsync<T>(string procedureName, DynamicParameters dynamicParameters) where T : class
    {
        try
        {
            Reconnect();
            var result = await _connection.QueryAsync<T>(procedureName, dynamicParameters,
                commandType: CommandType.StoredProcedure);
            return (new Result(true, "Ok"), result);
        }
        catch (Exception)
        {
            return (new Result(false, "Deu ruim"), null);
        }
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
            await _connection.ExecuteAsync(insert);
      
            return new(true, "Ok");
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }
    public async Task<Result> CreateEstadioAsync(string nome, int limite, string cep, string complemento, string bairro, string localidade, string uf, string ibge, string gia, string ddd, string siafi, string logradouro)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.InserirEstadio;
            sqlCommand.Parameters.Add("@Nome", SqlDbType.NVarChar).Value = nome;
            sqlCommand.Parameters.Add("@Limite", SqlDbType.Int).Value = limite;
            sqlCommand.Parameters.Add("@Cep", SqlDbType.NVarChar).Value = cep;
            sqlCommand.Parameters.Add("@Complemento", SqlDbType.NVarChar).Value = complemento;
            sqlCommand.Parameters.Add("@Bairro", SqlDbType.NVarChar).Value = bairro;
            sqlCommand.Parameters.Add("@Localidade", SqlDbType.NVarChar).Value = localidade;
            sqlCommand.Parameters.Add("@Uf", SqlDbType.NVarChar).Value = uf;
            sqlCommand.Parameters.Add("@Ibge", SqlDbType.NVarChar).Value = ibge;
            sqlCommand.Parameters.Add("@Gia", SqlDbType.NVarChar).Value = gia;
            sqlCommand.Parameters.Add("@Ddd", SqlDbType.NVarChar).Value = ddd;
            sqlCommand.Parameters.Add("@Siafi", SqlDbType.NVarChar).Value = siafi;
            sqlCommand.Parameters.Add("@Logradouro", SqlDbType.NVarChar).Value = logradouro;
            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }
    
    public async Task<Result> CreateCompradorAsync(string nome, string cpf)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.InserirComprador;
            
            sqlCommand.Parameters.Add("@Nome", SqlDbType.NVarChar).Value = nome;
            sqlCommand.Parameters.Add("@Cpf", SqlDbType.NVarChar).Value = cpf;
            
            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }

    public async Task<Result> CreateIngressoAsync(double valor, int jogoId)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.InserirIngresso;
            
            sqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal).Value = valor;
            sqlCommand.Parameters.Add("@JogoId", SqlDbType.Int).Value = jogoId;
            
            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }

    public async Task<Result> CreateJogadorAsync(string nome, DateTime? dataNascimento, int timeId,string cep, string complemento, string bairro, string localidade, string uf, string ibge, string gia, string ddd, string siafi, string logradouro)
    {
        try
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.InserirJogador; 

            sqlCommand.Parameters.Add("@Nome", SqlDbType.NVarChar).Value = nome;
            sqlCommand.Parameters.Add("@DataNascimento", SqlDbType.Date).Value = dataNascimento;
            sqlCommand.Parameters.Add("@TimeId", SqlDbType.Int).Value = timeId;
            sqlCommand.Parameters.Add("@Cep", SqlDbType.NVarChar).Value = cep;
            sqlCommand.Parameters.Add("@Complemento", SqlDbType.NVarChar).Value = complemento;
            sqlCommand.Parameters.Add("@Bairro", SqlDbType.NVarChar).Value = bairro;
            sqlCommand.Parameters.Add("@Localidade", SqlDbType.NVarChar).Value = localidade;
            sqlCommand.Parameters.Add("@Uf", SqlDbType.NVarChar).Value = uf;
            sqlCommand.Parameters.Add("@Ibge", SqlDbType.NVarChar).Value = ibge;
            sqlCommand.Parameters.Add("@Gia", SqlDbType.NVarChar).Value = gia;
            sqlCommand.Parameters.Add("@Ddd", SqlDbType.NVarChar).Value = ddd;
            sqlCommand.Parameters.Add("@Siafi", SqlDbType.NVarChar).Value = siafi;
            sqlCommand.Parameters.Add("@Logradouro", SqlDbType.NVarChar).Value = logradouro;

            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }

    public async Task<Result> CreateJogoAsync(DateTime? data, int estadioId)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.InserirJogo;
            
            sqlCommand.Parameters.Add("@Data", SqlDbType.Date).Value = data;
            sqlCommand.Parameters.Add("@EstadioId", SqlDbType.Int).Value = estadioId;
            
            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }

    public async Task<Result> CreatePartidaAsync(int? timeId, int? jogoId, int? estadioId)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.InserirPartida;

            sqlCommand.Parameters.Add("@TimeId", SqlDbType.Int).Value = timeId;
            sqlCommand.Parameters.Add("@JogoId", SqlDbType.Int).Value = jogoId;
            sqlCommand.Parameters.Add("@EstadioId", SqlDbType.Int).Value = estadioId;
            
            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }

    public async Task<Result> CreateTimeAsync(string nome, string cep, string complemento, string bairro,
        string localidade, string uf, string ibge, string gia, string ddd, string siafi, string logradouro)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.InserirTime;
        
            sqlCommand.Parameters.Add("@Nome", SqlDbType.NVarChar).Value = nome;
            sqlCommand.Parameters.Add("@Cep", SqlDbType.NVarChar).Value = cep;
            sqlCommand.Parameters.Add("@Complemento", SqlDbType.NVarChar).Value = complemento;
            sqlCommand.Parameters.Add("@Bairro", SqlDbType.NVarChar).Value = bairro;
            sqlCommand.Parameters.Add("@Localidade", SqlDbType.NVarChar).Value = localidade;
            sqlCommand.Parameters.Add("@Uf", SqlDbType.NVarChar).Value = uf;
            sqlCommand.Parameters.Add("@Ibge", SqlDbType.NVarChar).Value = ibge;
            sqlCommand.Parameters.Add("@Gia", SqlDbType.NVarChar).Value = gia;
            sqlCommand.Parameters.Add("@Ddd", SqlDbType.NVarChar).Value = ddd;
            sqlCommand.Parameters.Add("@Siafi", SqlDbType.NVarChar).Value = siafi;
            sqlCommand.Parameters.Add("@Logradouro", SqlDbType.NVarChar).Value = logradouro;
        
            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }

    public async Task<Result> CreateVendaAsync(DateTime? dataVenda, int? compradorId, int? ingressoId)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.InserirVenda;
            
            sqlCommand.Parameters.Add("@DataVenda", SqlDbType.DateTime).Value = dataVenda;
            sqlCommand.Parameters.Add("@CompradorId", SqlDbType.Int).Value = compradorId;
            sqlCommand.Parameters.Add("@IngressoId", SqlDbType.Int).Value = ingressoId;
            
            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }

    public async Task<Result> UpdateEstadioAsync(int id, string nome, int limite, string cep, string complemento,
        string bairro, string localidade, string uf, string ibge, string gia, string ddd, string siafi,
        string logradouro)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.AtualizarEstadio;
            
            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            sqlCommand.Parameters.Add("@Nome", SqlDbType.NVarChar).Value = nome;
            sqlCommand.Parameters.Add("@Limite", SqlDbType.Int).Value = limite;
            sqlCommand.Parameters.Add("@Cep", SqlDbType.NVarChar).Value = cep;
            sqlCommand.Parameters.Add("@Complemento", SqlDbType.NVarChar).Value = complemento;
            sqlCommand.Parameters.Add("@Bairro", SqlDbType.NVarChar).Value = bairro;
            sqlCommand.Parameters.Add("@Localidade", SqlDbType.NVarChar).Value = localidade;
            sqlCommand.Parameters.Add("@Uf", SqlDbType.NVarChar).Value = uf;
            sqlCommand.Parameters.Add("@Ibge", SqlDbType.NVarChar).Value = ibge;
            sqlCommand.Parameters.Add("@Gia", SqlDbType.NVarChar).Value = gia;
            sqlCommand.Parameters.Add("@Ddd", SqlDbType.NVarChar).Value = ddd;
            sqlCommand.Parameters.Add("@Siafi", SqlDbType.NVarChar).Value = siafi;
            sqlCommand.Parameters.Add("@Logradouro", SqlDbType.NVarChar).Value = logradouro;

            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }
    
    public async Task<Result> UpdateCompradorAsync(int id, string nome, string cpf)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.AtualizarComprador;
            
            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            sqlCommand.Parameters.Add("@Nome", SqlDbType.NVarChar).Value = nome;
            sqlCommand.Parameters.Add("@Cpf", SqlDbType.NVarChar).Value = cpf;
            
            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }
    
    public async Task<Result> UpdateJogadorAsync(int id, string nome, DateTime? dataNascimento, int timeId,string cep, string complemento, string bairro, string localidade, string uf, string ibge, string gia, string ddd, string siafi, string logradouro)
    {
        try
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.AtualizarJogador; 

            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            sqlCommand.Parameters.Add("@Nome", SqlDbType.NVarChar).Value = nome;
            sqlCommand.Parameters.Add("@DataNascimento", SqlDbType.Date).Value = dataNascimento;
            sqlCommand.Parameters.Add("@TimeId", SqlDbType.Int).Value = timeId;
            sqlCommand.Parameters.Add("@Cep", SqlDbType.NVarChar).Value = cep;
            sqlCommand.Parameters.Add("@Complemento", SqlDbType.NVarChar).Value = complemento;
            sqlCommand.Parameters.Add("@Bairro", SqlDbType.NVarChar).Value = bairro;
            sqlCommand.Parameters.Add("@Localidade", SqlDbType.NVarChar).Value = localidade;
            sqlCommand.Parameters.Add("@Uf", SqlDbType.NVarChar).Value = uf;
            sqlCommand.Parameters.Add("@Ibge", SqlDbType.NVarChar).Value = ibge;
            sqlCommand.Parameters.Add("@Gia", SqlDbType.NVarChar).Value = gia;
            sqlCommand.Parameters.Add("@Ddd", SqlDbType.NVarChar).Value = ddd;
            sqlCommand.Parameters.Add("@Siafi", SqlDbType.NVarChar).Value = siafi;
            sqlCommand.Parameters.Add("@Logradouro", SqlDbType.NVarChar).Value = logradouro;

            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }
    
    public async Task<Result> UpdateJogoAsync(int id, DateTime? data, int estadioId)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.AtualizarJogo;
            
            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            sqlCommand.Parameters.Add("@Data", SqlDbType.Date).Value = data;
            sqlCommand.Parameters.Add("@EstadioId", SqlDbType.Int).Value = estadioId;
            
            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }
    
    public async Task<Result> UpdatePartidaAsync(int id, int? timeId, int? jogoId, int? estadioId)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.AtualizarPartida;

            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            sqlCommand.Parameters.Add("@TimeId", SqlDbType.Int).Value = timeId;
            sqlCommand.Parameters.Add("@JogoId", SqlDbType.Int).Value = jogoId;
            sqlCommand.Parameters.Add("@EstadioId", SqlDbType.Int).Value = estadioId;
            
            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }

    public async Task<Result> UpdateTimeAsync(int id, string nome, string cep, string complemento, string bairro,
        string localidade, string uf, string ibge, string gia, string ddd, string siafi, string logradouro)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.AtualizarTime;

            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            sqlCommand.Parameters.Add("@Nome", SqlDbType.NVarChar).Value = nome;
            sqlCommand.Parameters.Add("@Cep", SqlDbType.NVarChar).Value = cep;
            sqlCommand.Parameters.Add("@Complemento", SqlDbType.NVarChar).Value = complemento;
            sqlCommand.Parameters.Add("@Bairro", SqlDbType.NVarChar).Value = bairro;
            sqlCommand.Parameters.Add("@Localidade", SqlDbType.NVarChar).Value = localidade;
            sqlCommand.Parameters.Add("@Uf", SqlDbType.NVarChar).Value = uf;
            sqlCommand.Parameters.Add("@Ibge", SqlDbType.NVarChar).Value = ibge;
            sqlCommand.Parameters.Add("@Gia", SqlDbType.NVarChar).Value = gia;
            sqlCommand.Parameters.Add("@Ddd", SqlDbType.NVarChar).Value = ddd;
            sqlCommand.Parameters.Add("@Siafi", SqlDbType.NVarChar).Value = siafi;
            sqlCommand.Parameters.Add("@Logradouro", SqlDbType.NVarChar).Value = logradouro;
        
            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }

    public async Task<Result> DeleteEstadioAsync(int id)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.DeletarEstadio;

            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }
    
    public async Task<Result> DeleteCompradorAsync(int id)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.DeletarComprador;

            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }
    
    public async Task<Result> DeleteIngressoAsync(int id)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.DeletarIngresso;

            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }

    public async Task<Result> DeleteJogadorAsync(int id)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.DeletarJogador;

            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }
    
    public async Task<Result> DeleteJogoAsync(int id)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.DeletarJogo;

            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }
    
    public async Task<Result> DeletePartidaAsync(int id)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.DeletarPartida;

            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }
    
    public async Task<Result> DeleteTimeAsync(int id)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.DeletarTime;

            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }
    
    public async Task<Result> DeleteVendaAsync(int id)
    {
        try
        {
            SqlCommand sqlCommand = new();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = SProc.DeletarVenda;

            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            return await ExecuteSqlCommandAsync(sqlCommand);
        }
        catch (Exception)
        {
            return new(false, "Deu ruim");
        }
    }

    public async Task<Result> ExecuteSqlCommandAsync(SqlCommand sqlCommand)
    {
        try
        {
            Reconnect();
            sqlCommand.Connection = _connection;
            var inserted = await sqlCommand.ExecuteNonQueryAsync() != 0;
            if (!inserted)
            {
                return new(false, "Falha ao inserir");
            }
            return new(true, "Ok");
        }
        catch (Exception)
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
    public async Task<string?> DeleteQueryAsync(string delete)
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