using TimesBD.Entities;
using TimesBD.Repositories;

namespace TimesBD.Business;

public class BusinessClass
{
    public const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";

    private readonly SqlRep _sqlRep;

    private readonly ApiRep _apiRep;

    private readonly ILogRepository _logRepository;

    public BusinessClass(string connectionString, ILogRepository logRepository)
    {
        _sqlRep = new(connectionString);
        _apiRep = new();
        _logRepository = logRepository;
    }

    public async Task<(Result, IEnumerable<Jogador>?)> JogadoresAsync()
    {
        return await _sqlRep.JogadoresAsync<Jogador>();
    }
    
    public async Task<(Result, IEnumerable<Comprador>?)> CompradoresAsync()
    {
        return await _sqlRep.CompradoresAsync<Comprador>();
    }
    
    public async Task<(Result, IEnumerable<Estadio>?)> EstadiosAsync()
    {
        return await _sqlRep.EstadiosAsync<Estadio>();
    }
    
    public async Task<(Result, IEnumerable<Ingresso>?)> IngressosAsync()
    {
        return await _sqlRep.IngressosAsync<Ingresso>();
    }
    
    public async Task<(Result, IEnumerable<Partida>?)> PartidasAsync()
    {
        return await _sqlRep.PartidasAsync<Partida>();
    }
    
    public async Task<(Result, IEnumerable<Time>?)> TimesAsync()
    {
        return await _sqlRep.TimesAsync<Time>();
    }
    
    public async Task<(Result, IEnumerable<Jogo>?)> JogosAsync()
    {
        return await _sqlRep.JogosAsync<Jogo>();
    }
    
    public async Task<(Result, IEnumerable<Venda>?)> VendasAsync()
    {
        return await _sqlRep.VendasAsync<Venda>();
    }
    
    public async Task<(Result, IEnumerable<Jogador>?)> JogadorByIdAsync(int id)
    {
        return await _sqlRep.JogadorAsync<Jogador>(id);
    }
    
    public async Task<(Result, IEnumerable<Comprador>?)> CompradorByIdAsync(int id)
    {
        return await _sqlRep.CompradorAsync<Comprador>(id);
    }
    
    public async Task<(Result, IEnumerable<Estadio>?)> EstadiosByIdAsync(int id)
    {
        return await _sqlRep.EstadioAsync<Estadio>(id);
    }
    
    public async Task<(Result, IEnumerable<Ingresso>?)> IngressoByIdAsync(int id)
    {
        return await _sqlRep.IngressoAsync<Ingresso>(id);
    }
    
    public async Task<(Result, IEnumerable<Partida>?)> PartidaByIdAsync(int id)
    {
        return await _sqlRep.PartidaAsync<Partida>(id);
    }
    
    public async Task<(Result, IEnumerable<Time>?)> TimeByIdAsync(int id)
    {
        return await _sqlRep.TimeAsync<Time>(id);
    }
    
    public async Task<(Result, IEnumerable<Jogo>?)> JogoByIdAsync(int id)
    {
        return await _sqlRep.JogoAsync<Jogo>(id);
    }
    
    public async Task<(Result, IEnumerable<Venda>?)> VendaByIdAsync(int id)
    {
        return await _sqlRep.VendaAsync<Venda>(id);
    }
        
    public async Task<Result> InserirJogadorAsync(string? nome, DateTime? dataNascimento, int timeId, string cep)
    {
        if (nome is null)
        {
            return new Result(false, "Nome não pode ser nulo");
        }

        if (dataNascimento != null)
        {
            if (dataNascimento > DateTime.Now || dataNascimento < DateTime.Now.AddYears(-100))
            {
                return new Result(false, "Data de nascimento não pode ser maior que a data atual ou menor que 100 anos atrás");
            }
        }
        else
        {
            return new Result(false, "Data de nascimento não pode ser nula");
        }
        
        if (timeId <= 0)
        {
            return new Result(false, "TimeId não pode ser menor ou igual a 0");
        }
        
        var time = (await TimeByIdAsync(timeId)).Item2;
        if (time != null && !time.Any())
        {
            return new Result(false, "TimeId não existe, insira um time válido");
        }
        
        var endereco = await _apiRep.ConsultarCep(cep);
        if (endereco is not null)
        {
            return await _sqlRep.CreateJogadorAsync(nome, dataNascimento, timeId, cep, endereco.Complemento, endereco.Bairro, endereco.Localidade.Replace("'", "''"), endereco.Uf, endereco.Ibge, endereco.Gia, endereco.Ddd, endereco.Siafi, endereco.Logradouro);
        }
        return new Result(false, "Cep inválido");
    }
    
    public async Task<Result> InserirCompradorAsync(string? nome, string? cpf)
    {
        if (nome is null)
        {
            return new Result(false, "Nome não pode ser nulo");
        }
        
        if (cpf is null)
        {
            return new Result(false, "CPF não pode ser nulo");
        }
        
        if (cpf.Length != 11)
        {
            return new Result(false, "CPF inválido");
        }
        
        return await _sqlRep.CreateCompradorAsync(nome, cpf);
    }
    
    public async Task<Result> InserirEstadioAsync(string? nome, int limite, string cep)
    {
        if (nome is null)
        {
            return new Result(false, "Nome não pode ser nulo");
        }
        
        if (limite <= 0)
        {
            return new Result(false, "Limite não pode ser menor ou igual a 0");
        }
        
        var endereco = await _apiRep.ConsultarCep(cep);
        if (endereco is not null)
        {
            return await _sqlRep.CreateEstadioAsync(nome, limite, cep, endereco.Complemento, endereco.Bairro, 
                endereco.Localidade.Replace("'", "''"), endereco.Uf, endereco.Ibge, endereco.Gia, endereco.Ddd, endereco.Siafi, endereco.Logradouro);
        }
        return new Result(false, "Cep inválido");
    }
    
    public async Task<Result> InserirIngressoAsync(double valor, int jogoId)
    {
        if (valor <= 0)
        {
            return new Result(false, "Valor não pode ser menor ou igual a 0");
        }

        var jogo = (await JogoByIdAsync(jogoId)).Item2;
        if (jogo != null && !jogo.Any())
        {
            return new Result(false, "JogoId não existe, insira um jogo válido");
        }

        return await _sqlRep.CreateIngressoAsync(valor, jogoId);
    }

    public async Task<Result> AddLogAsync(string action, string message, string details)
    {
        return await _logRepository.AddLogAsync(action, message, details);
    }

    public async Task<Result> InserirPartidaAsync(int timeId, int jogoId, int estadioId)
    {
            var time = (await TimeByIdAsync(timeId)).Item2;
            if (time != null && !time.Any())
            {
                return new Result(false, "TimeId não existe, insira um time válido");
            }
        
            var jogo = (await JogoByIdAsync(jogoId)).Item2;
            if (jogo != null && !jogo.Any())
            {
                return new Result(false, "JogoId não existe, insira um jogo válido");
            }
        
            var estadio = (await EstadiosByIdAsync(estadioId)).Item2;
            if (estadio != null && !estadio.Any())
            {
                return new Result(false, "EstadioId não existe, insira um id de estadio válido");
            }

            return await _sqlRep.CreatePartidaAsync(timeId, jogoId, estadioId);
    }
    
    public async Task<Result> InserirTimeAsync(string? nome, string cep)
    {
        if (nome is null)
        {
            return new Result(false, "Nome não pode ser nulo");
        }
        
        var endereco = await _apiRep.ConsultarCep(cep);
        if (endereco is not null)
        {
            return await _sqlRep.CreateTimeAsync(nome, cep, endereco.Complemento, endereco.Bairro, endereco.Localidade.Replace("'", "''"),
                endereco.Uf, endereco.Ibge, endereco.Gia, endereco.Ddd, endereco.Siafi, endereco.Logradouro);
        }
        
        return new Result(false, "Cep inválido");
    }

    public async Task<Result> InserirJogoAsync(DateTime? data, int estadioId)
    {
        if (data != null)
        {
            return new Result(false, "Data não pode ser nula");
        }
        
        var estadio = (await EstadiosByIdAsync(estadioId)).Item2;
        if (estadio != null && !estadio.Any())
        {
            return new Result(false, "EstadioId não existe, insira um id de estadio válido");
        }

        return await _sqlRep.CreateJogoAsync(data, estadioId);
    }
    
    public async Task<Result> InserirVendaAsync(DateTime? dataVenda, int compradorId, int ingressoId)
    {
        if (dataVenda != null)
        {
            var compradors = (await CompradorByIdAsync(compradorId)).Item2;
            if (compradors != null && !compradors.Any())
            {
                return new Result(false, "CompradorId não existe, insira um comprador válido");
            }

            var ingresso = (await IngressoByIdAsync(ingressoId)).Item2;
            if (ingresso != null && !ingresso.Any())
            {
                return new Result(false, "IngressoId não existe, insira um ingresso válido");
            }

            return await _sqlRep.CreateVendaAsync(dataVenda, compradorId, ingressoId);
        }
        
        return new Result(false, "dataVenda, compradorId ou ingressoId não podem ser nulos");
    }
    
    public async Task<Result> AtualizarJogadorAsync(int id, string? nome, DateTime? dataNascimento, int timeId, string cep)
    {
        var jogador = (await JogadorByIdAsync(id)).Item2;
        if (jogador != null && !jogador.Any())
        {
            return new Result(false, "Jogador não existe");
        }
        
        if (nome is null)
        {
            return new Result(false, "Nome não pode ser nulo");
        }

        if (dataNascimento != null)
        {
            if (dataNascimento > DateTime.Now || dataNascimento < DateTime.Now.AddYears(-100))
            {
                return new Result(false, "Data de nascimento não pode ser maior que a data atual ou menor que 100 anos atrás");
            }
        }
        else
        {
            return new Result(false, "Data de nascimento não pode ser nula");
        }
        
        if (timeId <= 0)
        {
            return new Result(false, "TimeId não pode ser menor ou igual a 0");
        }
        
        var time = (await TimeByIdAsync(timeId)).Item2;
        if (time != null && !time.Any())
        {
            return new Result(false, "TimeId não existe, insira um time válido");
        }
        
        var endereco = await _apiRep.ConsultarCep(cep);
        if (endereco is not null)
        {
            await _sqlRep.UpdateJogadorAsync(id, nome, dataNascimento, timeId, cep, endereco.Complemento, endereco.Bairro, endereco.Localidade.Replace("'", "''"), endereco.Uf, endereco.Ibge, endereco.Gia, endereco.Ddd, endereco.Siafi, endereco.Logradouro);
            return new Result(true, "Jogador atualizado com sucesso");
        }
        return new Result(false, "Cep inválido");
    }
    
    public async Task<Result> AtualizarCompradorAsync(int id, string? nome, string? cpf)
    {
        var comprador = (await CompradorByIdAsync(id)).Item2;
        if (comprador != null && !comprador.Any())
        {
            return new Result(false, "Comprador não existe");
        }
        
        if (nome is null)
        {
            return new Result(false, "Nome não pode ser nulo");
        }
        
        if (cpf is null)
        {
            return new Result(false, "CPF não pode ser nulo");
        }
        
        if (cpf.Length != 11)
        {
            return new Result(false, "CPF inválido");
        }
        
        await _sqlRep.UpdateCompradorAsync(id, nome, cpf);
        return new Result(true, "Comprador atualizado com sucesso");
    }
    
    public async Task<Result> AtualizarEstadioAsync(int id, string? nome, int limite, string cep)
    {
        var estadio = (await EstadiosByIdAsync(id)).Item2;
        if (estadio != null && !estadio.Any())
        {
            return new Result(false, "EstadioId não existe, insira um id de estadio válido");
        }
        
        if (nome is null)
        {
            return new Result(false, "Nome não pode ser nulo");
        }
        
        if (limite <= 0)
        {
            return new Result(false, "Limite não pode ser menor ou igual a 0");
        }
        
        var endereco = await _apiRep.ConsultarCep(cep);
        if (endereco is not null)
        {
            await _sqlRep.UpdateEstadioAsync(id, nome, limite, cep, endereco.Complemento, endereco.Bairro, endereco.Localidade.Replace("'", "''"), endereco.Uf, endereco.Ibge, endereco.Gia, endereco.Ddd, endereco.Siafi, endereco.Logradouro);
            return new Result(true, "Estadio atualizado com sucesso");
        }
        return new Result(false, "Cep inválido");
    }
    
    public async Task<Result> AtualizarTimeAsync(int id, string? nome, string cep)
    {
        var time = (await TimeByIdAsync(id)).Item2;
        if (time != null && !time.Any())
        {
            return new Result(false, "TimeId não existe, insira um time válido");
        }
        
        if (nome is null)
        {
            return new Result(false, "Nome não pode ser nulo");
        }
        
        var endereco = await _apiRep.ConsultarCep(cep);
        if (endereco is not null)
        {
            await _sqlRep.UpdateTimeAsync(id, nome, cep, endereco.Complemento, endereco.Bairro, endereco.Localidade.Replace("'", "''"),
                endereco.Uf, endereco.Ibge, endereco.Gia, endereco.Ddd, endereco.Siafi, endereco.Logradouro);
            return new Result(true, "Time atualizado com sucesso");
        }
        return new Result(false, "Cep inválido");
    }
    
    public async Task<Result> AtualizarIngressoAsync(int id, double valor, int jogoId)
    {
        var ingresso = (await IngressoByIdAsync(id)).Item2;
        if (ingresso != null && !ingresso.Any())
        {
            return new Result(false, "Ingresso não existe");
        }
        
        if (valor <= 0)
        {
            return new Result(false, "Valor não pode ser menor ou igual a 0");
        }
        
        var jogo = (await JogoByIdAsync(jogoId)).Item2;
        if (jogo != null && !jogo.Any())
        {
            return new Result(false, "JogoId não existe, insira um jogo válido");
        }
        
        var sql = $"EXEC sp_AtualizarIngresso {id}, {valor}, {jogoId}";
            
        await _sqlRep.PatchQueryAsync(sql);
        return new Result(true, "Ingresso atualizado com sucesso");
    }
    
    public async Task<Result> AtualizarJogoAsync(int id, DateTime? data, int estadioId)
    {
        var jogo = (await JogoByIdAsync(id)).Item2;
        if (jogo != null && !jogo.Any())
        {
            return new Result(false, "JogoId não existe, insira um jogo válido");
        }
        
        if (data != null)
        {
            return new Result(false, "Data não pode ser nula");
        }
        
        var estadio = (await EstadiosByIdAsync(estadioId)).Item2;
        if (estadio != null && !estadio.Any())
        {
            return new Result(false, "EstadioId não existe, insira um id de estadio válido");
        }
        
        await _sqlRep.UpdateJogoAsync(id, data, estadioId);
        return new Result(true, "Jogo atualizado com sucesso");
    }

    public async Task<Result> AtualizarPartidaAsync(int id, int timeId, int jogoId, int estadioId)
    {
        var partida = (await PartidaByIdAsync(id)).Item2;
        if (partida != null && !partida.Any())
        {
            return new Result(false, "Partida não existe");
        }
        
        var time = (await TimeByIdAsync(timeId)).Item2;
        if (time != null && !time.Any())
        {
            return new Result(false, "TimeId não existe, insira um time válido");
        }
        
        var jogo = (await JogoByIdAsync(jogoId)).Item2;
        if (jogo != null && !jogo.Any())
        {
            return new Result(false, "JogoId não existe, insira um jogo válido");
        }
        
        var estadio = (await EstadiosByIdAsync(estadioId)).Item2;
        if (estadio != null && !estadio.Any())
        {
            return new Result(false, "EstadioId não existe, insira um id de estadio válido");
        }
            
        await _sqlRep.UpdatePartidaAsync(id, timeId, jogoId, estadioId);
        return new Result(true, "Partida atualizada com sucesso");
    }
     
    public async Task DeletarJogadorAsync(int id)
    {
        await _sqlRep.DeleteJogadorAsync(id);
    }
    
    public async Task DeletarCompradorAsync(int id)
    {
        await _sqlRep.DeleteCompradorAsync(id);
    }
    
    public async Task DeletarEstadioAsync(int id)
    {
        await _sqlRep.DeleteEstadioAsync(id);
    }
    
    public async Task DeletarIngressoAsync(int id)
    {
        await _sqlRep.DeleteIngressoAsync(id);
    }
    
    public async Task DeletarPartidaAsync(int id)
    {
        await _sqlRep.DeletePartidaAsync(id);
    }
    
    public async Task DeletarTimeAsync(int id)
    {
        await _sqlRep.DeleteTimeAsync(id);
    }
    
    public async Task DeletarJogoAsync(int id)
    {
        await _sqlRep.DeleteJogoAsync(id);
    }
    
    public async Task DeletarVendaAsync(int id)
    {
        await _sqlRep.DeleteVendaAsync(id);
    }
    
    public async Task<IEnumerable<T>> EntityByIdAsync<T>(int? id) where T : class
    {
        var entityType = typeof(T);
        var sql = $"SELECT * FROM {entityType.Name} ";
        if (id is not null)
        {
            sql += $"WHERE Id = {id}";
        }
        return await _sqlRep.QueryAsync<T>(sql);
    }
    
    public async Task<Result> DeleteEntityAsync<T>(int id) where T : class
    {
        if (!(await EntityByIdAsync<T>(id)).Any())
        {
            return new Result(false, $"{typeof(T).Name} não existe");
        }
        
        var entityType = typeof(T);
        var sql = $"EXEC sp_Deletar{entityType.Name} {id}";
        await _sqlRep.DeleteQueryAsync(sql);
        return new Result(true, $"{typeof(T).Name} deletado com sucesso");
    }
    
    public static bool ValidarAutenticacao(HttpRequest request) => request.Headers.TryGetValue("autentica", out var autentica) && autentica == Autentica; 
}