using TimesBD.Business;
using TimesBD.Entities;

namespace TimesBD.Framework
{
    public class TimesBackgroundService : BackgroundService
    {
        private BusinessClass _businessClass;

        public TimesBackgroundService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _businessClass = new BusinessClass(connectionString);
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

        public async Task<(Result, IEnumerable<Jogador>?)> GetJogadores(string? autentica = null)
        {
            return await _businessClass.GetJogadoresAsync();
        }
        
        public async Task<(Result, IEnumerable<Time>?)> GetTimes(string? autentica = null)
        {
            return await _businessClass.GetTimesAsync();
        }
        
        public async Task<(Result, IEnumerable<Estadio>?)> GetEstadios(string? autentica = null)
        {
            return await _businessClass.GetEstadiosAsync();
        }
        
        public async Task<(Result, IEnumerable<Jogo>?)> GetJogos(string? autentica = null)
        {
            return await _businessClass.GetJogosAsync();
        }
        
        public async Task<(Result, IEnumerable<Ingresso>?)> GetIngressos(string? autentica = null)
        {
            return await _businessClass.GetIngressosAsync();
        }
        
        public async Task<(Result, IEnumerable<Partida>?)> GetPartidas(string? autentica = null)
        {
            return await _businessClass.GetPartidasAsync();
        }
        
        public async Task<(Result, IEnumerable<Comprador>?)> GetCompradores(string? autentica = null)
        {
            return await _businessClass.GetCompradoresAsync();
        }
        
        public async Task<(Result, IEnumerable<Venda>?)> GetVendas(string? autentica = null)
        {
            return await _businessClass.GetVendasAsync();
        }
        
        public async Task<(Result, IEnumerable<Estadio>?)> GetEstadiosById(int id, string? autentica = null)
        {
            return await _businessClass.GetEstadiosByIdAsync(id);
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
        
        public async Task<(Result, IEnumerable<Jogo>?)> GetJogosById(int id, string? autentica = null)
        {
            return await _businessClass.GetJogoByIdAsync(id);
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
        
        public async Task<(Result, IEnumerable<Ingresso>?)> GetIngressosById(int id, string? autentica = null)
        {
            return await _businessClass.GetIngressoByIdAsync(id);
        }
        
        public async Task<(Result, IEnumerable<Partida>?)> GetPartidasById(int id, string? autentica = null)
        {
            return await _businessClass.GetPartidaByIdAsync(id);
        }
        
        public async Task<(Result, IEnumerable<Time>?)> GetTimesById(int id, string? autentica = null)
        {
            return await _businessClass.GetTimeByIdAsync(id);
        }

        public async Task<(Result, IEnumerable<Jogador>?)> GetJogadorById(int id, string? autentica = null)
        {
            return await _businessClass.GetJogadorByIdAsync(id);
        }
         
        public async Task<(Result, IEnumerable<Comprador>?)> GetCompradorById(int id, string? autentica = null)
        {
            return await _businessClass.GetCompradorByIdAsync(id);
        }
        
        public async Task<(Result, IEnumerable<Venda>?)> GetVendasById(int id, string? autentica = null)
        {
            return await _businessClass.GetVendaByIdAsync(id);
        }
        
    }
}
