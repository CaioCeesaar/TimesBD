using MongoDB.Driver;
using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Repositories;

public class MongoRep : ILogRepository
{
    private readonly IMongoCollection<LogEntry> _logsCollection;

    public MongoRep(IConfiguration configuration)
    {
        var mongoClient = new MongoClient(configuration.GetValue<string>("MongoDB:ConnectionString"));
        var mongoDatabase = mongoClient.GetDatabase(configuration.GetValue<string>("MongoDB:DatabaseName"));
        _logsCollection = mongoDatabase.GetCollection<LogEntry>(configuration.GetValue<string>("MongoDB:LogsCollectionName"));
    }
    
    public async Task<Result> AddLogAsync(string action, string message, string details)
    {
        try
        {
            var logEntry = new LogEntry
            {
                Action = action,
                Message = message,
                Details = details
            };

            await _logsCollection.InsertOneAsync(logEntry);
            return new Result(true, "Log adicionado com sucesso.");
        }
        catch (MongoException mongoEx)
        {
            Console.WriteLine($"Erro ao inserir log no MongoDB: {mongoEx.Message}");
            return new Result(false, "Erro ao adicionar log.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro geral ao inserir log: {ex.Message}");
            return new Result(false, "Erro ao adicionar log.");
        }
    }
}

public interface ILogRepository
{
    Task<Result> AddLogAsync(string action, string message, string details);
}