using TimesBD.Business;
using TimesBD.Entities;
using TimesBD.Repositories;

namespace TimesBD.Framework;

public class TimesBackgroundService : BackgroundService
{
    private BusinessClass _businessClass;

    public TimesBackgroundService(IConfiguration configuration, ILogRepository logRepository)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _businessClass = new BusinessClass(connectionString, logRepository);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            Console.WriteLine($"BackgroundService is running. TimesBackgroundService: {DateTimeOffset.Now}");
            await Task.Delay(10000);
            if (_businessClass is null)
            {
                return;
            }
        }
    }

    public async Task<(Result, IEnumerable<Jogador>?)> Jogadores(string? autentica = null)
    {
        return await _businessClass.JogadoresAsync();
    }
        
    public async Task<(Result, IEnumerable<Time>?)> Times(string? autentica = null)
    {
        return await _businessClass.TimesAsync();
    }
        
    public async Task<(Result, IEnumerable<Estadio>?)> Estadios(string? autentica = null)
    {
        return await _businessClass.EstadiosAsync();
    }
        
    public async Task<(Result, IEnumerable<Jogo>?)> Jogos(string? autentica = null)
    {
        return await _businessClass.JogosAsync();
    }
        
    public async Task<(Result, IEnumerable<Ingresso>?)> Ingressos(string? autentica = null)
    {
        return await _businessClass.IngressosAsync();
    }
        
    public async Task<(Result, IEnumerable<Partida>?)> Partidas(string? autentica = null)
    {
        return await _businessClass.PartidasAsync();
    }
        
    public async Task<(Result, IEnumerable<Comprador>?)> Compradores(string? autentica = null)
    {
        return await _businessClass.CompradoresAsync();
    }
        
    public async Task<(Result, IEnumerable<Venda>?)> Vendas(string? autentica = null)
    {
        return await _businessClass.VendasAsync();
    }
        
    public async Task<(Result, IEnumerable<Estadio>?)> EstadiosById(int id, string? autentica = null)
    {
        return await _businessClass.EstadiosByIdAsync(id);
    }
        
    public async Task<(Result, IEnumerable<Ingresso>?)> IngressosById(int id, string? autentica = null)
    {
        return await _businessClass.IngressoByIdAsync(id);
    }
        
    public async Task<(Result, IEnumerable<Partida>?)> PartidasById(int id, string? autentica = null)
    {
        return await _businessClass.PartidaByIdAsync(id);
    }
        
    public async Task<(Result, IEnumerable<Time>?)> TimesById(int id, string? autentica = null)
    {
        return await _businessClass.TimeByIdAsync(id);
    }

    public async Task<(Result, IEnumerable<Jogador>?)> JogadorById(int id, string? autentica = null)
    {
        return await _businessClass.JogadorByIdAsync(id);
    }
         
    public async Task<(Result, IEnumerable<Comprador>?)> CompradorById(int id, string? autentica = null)
    {
        return await _businessClass.CompradorByIdAsync(id);
    }
        
    public async Task<(Result, IEnumerable<Venda>?)> VendasById(int id, string? autentica = null)
    {
        return await _businessClass.VendaByIdAsync(id);
    }

    public async Task<Result> AtualizarEstadioAsync(int id, string nome, int limite, string cep, string? autentica = null)
    {
        return await _businessClass.AtualizarEstadioAsync(id, nome, limite, cep);
    }

    public async Task<Result> AtualizarIngressoAsync(int id, double valor, int jogoId, string? autentica = null)
    {
        return await _businessClass.AtualizarIngressoAsync(id, valor, jogoId);
    }
        
    public async Task<Result> AtualizarJogoAsync(int id, DateTime data, int estadioId, string? autentica = null)
    {
        return await _businessClass.AtualizarJogoAsync(id, data, estadioId);
    }
        
    public async Task<Result> AtualizarPartidaAsync(int id, int timeId, int jogoId, int estadioId, string? autentica = null)
    {
        return await _businessClass.AtualizarPartidaAsync(id, timeId, jogoId, estadioId);
    }
        
    public async Task<Result> AtualizarTimeAsync(int id, string nome, string cep, string? autentica = null)
    {
        return await _businessClass.AtualizarTimeAsync(id, nome, cep);
    }
        
    public async Task<Result> AtualizarJogadorAsync(int id, string nome, DateTime dataNascimento, int timeId, string cep, string? autentica = null)
    {
        return await _businessClass.AtualizarJogadorAsync(id, nome, dataNascimento, timeId, cep);
    }
        
    public async Task<Result> AtualizarCompradorAsync(int id, string nome, string cpf, string? autentica = null)
    {
        return await _businessClass.AtualizarCompradorAsync(id, nome, cpf);
    }

    public async Task<Result> InserirLogAsync(string action, string message, string details, string? autentica = null)
    {
        return await _businessClass.AddLogAsync(action, message, details);
    }

    public async Task<Result> InserirEstadioAsync(string nome, int limite, string cep, string? autentica = null)
    {
        return await _businessClass.InserirEstadioAsync(nome, limite, cep);
    }
        
    public async Task<Result> InserirVendaAsync(DateTime dataVenda, int compradorId, int ingressoId, string? autentica = null)
    {
        return await _businessClass.InserirVendaAsync(dataVenda, compradorId, ingressoId);
    }
        
    public async Task<Result> InserirTimeAsync(string nome, string cep, string? autentica = null)
    {
        return await _businessClass.InserirTimeAsync(nome, cep);
    }
        
    public async Task<Result> InserirPartidaAsync(int timeId, int jogoId, int estadioId, string? autentica = null)
    {
        return await _businessClass.InserirPartidaAsync(timeId, jogoId, estadioId);
    }
        
    public async Task<Result> InserirJogoAsync(DateTime data, int estadioId, string? autentica = null)
    {
        return await _businessClass.InserirJogoAsync(data, estadioId);
    }
        
    public async Task<Result> InserirJogadorAsync(string nome, DateTime dataNascimento, int timeId, string cep, string? autentica = null)
    {
        return await _businessClass.InserirJogadorAsync(nome, dataNascimento, timeId, cep);
    }
        
    public async Task<Result> InserirCompradorAsync(string nome, string cpf, string? autentica = null)
    {
        return await _businessClass.InserirCompradorAsync(nome, cpf);
    }
        
    public async Task<Result> InserirIngressoAsync(double valor, int jogoId, string? autentica = null)
    {
        return await _businessClass.InserirIngressoAsync(valor, jogoId);
    }
        
    public async Task<(Result, IEnumerable<Jogo>?)> JogosById(int id, string? autentica = null)
    {
        return await _businessClass.JogoByIdAsync(id);
    }

    public async Task DeletarEstadioAsync(int id, string? autentica = null)
    {
        await _businessClass.DeletarEstadioAsync(id);
    }
        
    public async Task DeletarIngressoAsync(int id, string? autentica = null)
    {
        await _businessClass.DeletarIngressoAsync(id);
    }
        
    public async Task DeletarTimeAsync(int id, string? autentica = null)
    {
        await _businessClass.DeletarTimeAsync(id);
    }
        
    public async Task DeletarJogadorAsync(int id, string? autentica = null)
    {
        await _businessClass.DeletarJogadorAsync(id);
    }
        
    public async Task DeletarPartidaAsync(int id, string? autentica = null)
    {
        await _businessClass.DeletarPartidaAsync(id);
    }
        
    public async Task DeletarJogoAsync (int id, string? autentica = null)
    {
        await _businessClass.DeletarJogoAsync(id);
    }
        
    public async Task DeletarVendaAsync(int id, string? autentica = null)
    {
        await _businessClass.DeletarVendaAsync(id);
    }
        
    public async Task DeletarCompradorAsync(int id, string? autentica = null)
    {
        await _businessClass.DeletarCompradorAsync(id);
    }
}