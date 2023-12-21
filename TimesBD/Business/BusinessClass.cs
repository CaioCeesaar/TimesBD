using TimesBD.Entities;
using TimesBD.Repositories;

namespace TimesBD.Business;

public class BusinessClass
{
    public const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";

    private readonly SqlRep _sqlRep;

    private readonly ApiRep _apiRep;

    public BusinessClass(string connectionString)
    {
        _sqlRep = new(connectionString);
        _apiRep = new();
    }

    public async Task<IEnumerable<Jogador>> GetJogadorByIdAsync(string? autentica, int? id)
    {
        return await GetEntityByIdAsync<Jogador>(id);
    }
    
    public async Task<IEnumerable<Comprador>> GetCompradorByIdAsync(string? autentica, int? id)
    {
        return await GetEntityByIdAsync<Comprador>(id);
    }
    
    public async Task<IEnumerable<Estadio>> GetEstadiosByIdAsync(string? autentica, int? id)
    {
        return await GetEntityByIdAsync<Estadio>(id);
    }
    
    public async Task<IEnumerable<Ingresso>> GetIngressoByIdAsync(string? autentica, int? id)
    {
        return await GetEntityByIdAsync<Ingresso>(id);
    }
    
    public async Task<IEnumerable<Partida>> GetPartidaByIdAsync(string? autentica, int? id)
    {
        return await GetEntityByIdAsync<Partida>(id);
    }
    
    public async Task<IEnumerable<Time>> GetTimeByIdAsync(string? autentica, int? id)
    {
        return await GetEntityByIdAsync<Time>(id);  
    }
    
    public async Task<IEnumerable<Jogo>> GetJogoByIdAsync(string? autentica, int? id)
    {
        return await GetEntityByIdAsync<Jogo>(id);
    }
    
    public async Task<IEnumerable<Venda>> GetVendaByIdAsync(string? autentica, int? id)
    {
        return await GetEntityByIdAsync<Venda>(id);
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
        
        if (!(await GetTimeByIdAsync(null, timeId)).Any())
        {
            return new Result(false, "TimeId não existe, insira um time válido");
        }
        
        var endereco = await _apiRep.ConsultarCep(cep);
        if (endereco is not null)
        {
            endereco.Localidade = endereco.Localidade.Replace("'", "''");
            var sql = $"EXEC sp_InserirJogador '{nome}', '{dataNascimento}', {timeId}, '{cep}', '{endereco.Complemento}', '{endereco.Bairro}', '{endereco.Localidade}', '{endereco.Uf}', '{endereco.Ibge}', '{endereco.Gia}', '{endereco.Ddd}', '{endereco.Siafi}', '{endereco.Logradouro}'";
            
            return await _sqlRep.PostQueryAsync(sql);
            
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
        
        var sql = $"EXEC sp_InserirComprador '{nome}', '{cpf}'";
        return await _sqlRep.PostQueryAsync(sql);
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
            endereco.Localidade = endereco.Localidade.Replace("'", "''");
            var sql = $"EXEC sp_InserirEstadio '{nome}', {limite}, '{cep}', '{endereco.Complemento}', '{endereco.Bairro}', '{endereco.Localidade}', '{endereco.Uf}', '{endereco.Ibge}', '{endereco.Gia}', '{endereco.Ddd}', '{endereco.Siafi}', '{endereco.Logradouro}'";
            
            return await _sqlRep.PostQueryAsync(sql);
            
        }
        return new Result(false, "Cep inválido");
    }
    
    public async Task<Result> InserirIngressoAsync(double valor, int jogoId)
    {
        if (valor <= 0)
        {
            return new Result(false, "Valor não pode ser menor ou igual a 0");
        }
        
        if (!(await GetJogoByIdAsync(null, jogoId)).Any())
        {
            return new Result(false, "JogoId não existe, insira um jogo válido");
        }
        
        var sql = $"EXEC sp_InserirIngresso {valor}, {jogoId}";
            
        return await _sqlRep.PostQueryAsync(sql);
    }

    public async Task<Result> InserirPartidaAsync(int? timeId, int? jogoId, int? estadioId)
    {
        if (timeId != null && jogoId != null && estadioId != null)
        {
            if (!(await GetTimeByIdAsync(null, timeId)).Any())
            {
                return new Result(false, "Time não existe, insira um id de time válido");
            }
        
            if (!(await GetJogoByIdAsync(null, jogoId)).Any())
            {
                return new Result(false, "Jogo não existe, insira um id de jogo válido");
            }
        
            if (!(await GetEstadiosByIdAsync(null, estadioId)).Any())
            {
                return new Result(false, "EstadioId não existe, insira um id de estadio válido");
            }
        
            var sql = $"EXEC sp_InserirPartida {timeId}, {jogoId}, {estadioId}";
            
            return await _sqlRep.PostQueryAsync(sql);
        }

        return new Result(false, "timeId, jogoId ou estadioId não podem ser nulos");
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
            endereco.Localidade = endereco.Localidade.Replace("'", "''");
            var sql = $"EXEC sp_InserirTime '{nome}', {cep}, '{endereco.Complemento}', '{endereco.Bairro}', '{endereco.Localidade}', '{endereco.Uf}', '{endereco.Ibge}', '{endereco.Gia}', '{endereco.Ddd}', '{endereco.Siafi}', '{endereco.Logradouro}'";
            
            return await _sqlRep.PostQueryAsync(sql);
        }
        
        return new Result(false, "Cep inválido");
    }

    public async Task<Result> InserirJogoAsync(DateTime? data, int estadioId)
    {
        if (data != null)
        {
            return new Result(false, "Data não pode ser nula");
        }
        
        if (!(await GetEstadiosByIdAsync(null, estadioId)).Any())
        {
            return new Result(false, "EstadioId não existe, insira um estadio válido");
        }
        
        var sql = $"EXEC sp_InserirJogo '{data}', {estadioId}";
            
        return await _sqlRep.PostQueryAsync(sql);
    }
    
    public async Task<Result> InserirVendaAsync(DateTime? dataVenda, int? compradorId, int? ingressoId)
    {
        if (dataVenda != null && compradorId != null && ingressoId != null)
        {
            if (!(await GetCompradorByIdAsync(null, compradorId)).Any())
            {
                return new Result(false, "CompradorId não existe, insira um comprador válido");
            }
        
            if (!(await GetIngressoByIdAsync(null, ingressoId)).Any())
            {
                return new Result(false, "IngressoId não existe, insira um ingresso válido");
            }
        
            var sql = $"EXEC sp_InserirVenda '{dataVenda}', {compradorId}, {ingressoId}";
            
            return await _sqlRep.PostQueryAsync(sql);
        }
        
        return new Result(false, "dataVenda, compradorId ou ingressoId não podem ser nulos");
    }
    
    public async Task<Result> AtualizarJogadorAsync(int id, string? nome, DateTime? dataNascimento, int timeId, string cep)
    {
        if (!(await GetJogadorByIdAsync(null, id)).Any())
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
        
        if (!(await GetTimeByIdAsync(null, timeId)).Any())
        {
            return new Result(false, "TimeId não existe, insira um time válido");
        }
        
        var endereco = await _apiRep.ConsultarCep(cep);
        if (endereco is not null)
        {
            endereco.Localidade = endereco.Localidade.Replace("'", "''");
            var sql =
                $"EXEC sp_AtualizarJogador {id}, '{nome}', '{dataNascimento}', {timeId}, {cep}, '{endereco.Complemento}', '{endereco.Bairro}', '{endereco.Localidade}', '{endereco.Uf}', '{endereco.Ibge}', '{endereco.Gia}', '{endereco.Ddd}', '{endereco.Siafi}', '{endereco.Logradouro}'";

            await _sqlRep.PatchQueryAsync(sql);
            return new Result(true, "Jogador atualizado com sucesso");
        }
        return new Result(false, "Cep inválido");
    }
    
    public async Task<Result> AtualizarCompradorAsync(int id, string? nome, string? cpf)
    {
        if (!(await GetCompradorByIdAsync(null, id)).Any())
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
        
        var sql = $"EXEC sp_AtualizarComprador {id}, '{nome}', '{cpf}'";
        await _sqlRep.PatchQueryAsync(sql);
        return new Result(true, "Comprador atualizado com sucesso");
    }
    
    public async Task<Result> AtualizarEstadioAsync(int id, string? nome, int limite, string cep)
    {
        if (!(await GetEstadiosByIdAsync(null, id)).Any())
        {
            return new Result(false, "Estadio não existe");
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
            endereco.Localidade = endereco.Localidade.Replace("'", "''");
            var sql =
                $"EXEC sp_AtualizarEstadio {id}, '{nome}', {limite},'{cep}', '{endereco.Complemento}', '{endereco.Bairro}', '{endereco.Localidade}', '{endereco.Uf}', '{endereco.Ibge}', '{endereco.Gia}', '{endereco.Ddd}', '{endereco.Siafi}', '{endereco.Logradouro}'";

            await _sqlRep.PatchQueryAsync(sql);
            return new Result(true, "Estadio atualizado com sucesso");
        }
        return new Result(false, "Cep inválido");
    }
    
    public async Task<Result> AtualizarTimeAsync(int id, string? nome, string cep)
    {
        if (!(await GetTimeByIdAsync(null, id)).Any())
        {
            return new Result(false, "Time não existe");
        }
        
        if (nome is null)
        {
            return new Result(false, "Nome não pode ser nulo");
        }
        
        var endereco = await _apiRep.ConsultarCep(cep);
        if (endereco is not null)
        {
            endereco.Localidade = endereco.Localidade.Replace("'", "''");
            var sql =
                $"EXEC sp_AtualizarTime {id}, '{nome}', {cep}, '{endereco.Complemento}', '{endereco.Bairro}', '{endereco.Localidade}', '{endereco.Uf}', '{endereco.Ibge}', '{endereco.Gia}', '{endereco.Ddd}', '{endereco.Siafi}', '{endereco.Logradouro}'";

            await _sqlRep.PatchQueryAsync(sql);
            return new Result(true, "Time atualizado com sucesso");
        }
        return new Result(false, "Cep inválido");
    }
    
    public async Task<Result> AtualizarIngressoAsync(int id, double valor, int jogoId)
    {
        if (!(await GetIngressoByIdAsync(null, id)).Any())
        {
            return new Result(false, "Ingresso não existe");
        }
        
        if (valor <= 0)
        {
            return new Result(false, "Valor não pode ser menor ou igual a 0");
        }
        
        if (!(await GetJogoByIdAsync(null, jogoId)).Any())
        {
            return new Result(false, "JogoId não existe, insira um jogo válido");
        }
        
        var sql = $"EXEC sp_AtualizarIngresso {id}, {valor}, {jogoId}";
            
        await _sqlRep.PatchQueryAsync(sql);
        return new Result(true, "Ingresso atualizado com sucesso");
    }
    
    public async Task<Result> AtualizarJogoAsync(int id, DateTime? data, int estadioId)
    {
        if (!(await GetJogoByIdAsync(null, id)).Any())
        {
            return new Result(false, "Jogo não existe");
        }
        
        if (data != null)
        {
            return new Result(false, "Data não pode ser nula");
        }
        
        if (!(await GetEstadiosByIdAsync(null, estadioId)).Any())
        {
            return new Result(false, "EstadioId não existe, insira um estadio válido");
        }
        
        var sql = $"EXEC sp_AtualizarJogo {id}, '{data}', {estadioId}";
            
        await _sqlRep.PatchQueryAsync(sql);
        return new Result(true, "Jogo atualizado com sucesso");
    }

    public async Task<Result> AtualizarPartidaAsync(int id, int? timeId, int? jogoId, int? estadioId)
    {
        if (!(await GetPartidaByIdAsync(null, id)).Any())
        {
            return new Result(false, "Partida não existe");
        }
        
        if (timeId != null && jogoId != null && estadioId != null)
        {
            if (!(await GetTimeByIdAsync(null, timeId)).Any())
            {
                return new Result(false, "Time não existe, insira um id de time válido");
            }
        
            if (!(await GetJogoByIdAsync(null, jogoId)).Any())
            {
                return new Result(false, "Jogo não existe, insira um id de jogo válido");
            }
        
            if (!(await GetEstadiosByIdAsync(null, estadioId)).Any())
            {
                return new Result(false, "EstadioId não existe, insira um id de estadio válido");
            }
        
            var sql = $"EXEC sp_AtualizarPartida {id}, {timeId}, {jogoId}, {estadioId}";
            
            await _sqlRep.PatchQueryAsync(sql);
            return new Result(true, "Partida atualizada com sucesso");
        }

        return new Result(false, "timeId, jogoId ou estadioId não podem ser nulos");
    }
     
    public async Task DeletarJogadorAsync(int id)
    {
        await DeleteEntityAsync<Jogador>(id);
    }
    
    public async Task DeletarCompradorAsync(int id)
    {
        await DeleteEntityAsync<Comprador>(id);
    }
    
    public async Task DeletarEstadioAsync(int id)
    {
        await DeleteEntityAsync<Estadio>(id);
    }
    
    public async Task DeletarIngressoAsync(int id)
    {
        await DeleteEntityAsync<Ingresso>(id);
    }
    
    public async Task DeletarPartidaAsync(int id)
    {
        await DeleteEntityAsync<Partida>(id);
    }
    
    public async Task DeletarTimeAsync(int id)
    {
        await DeleteEntityAsync<Time>(id);
    }
    
    public async Task DeletarJogoAsync(int id)
    {
        await DeleteEntityAsync<Jogo>(id);
    }
    
    public async Task DeletarVendaAsync(int id)
    {
        await DeleteEntityAsync<Venda>(id);
    }
    
    public async Task<IEnumerable<T>> GetEntityByIdAsync<T>(int? id) where T : class
    {
        var entityType = typeof(T);
        var sql = $"SELECT * FROM {entityType.Name} ";
        if (id is not null)
        {
            sql += $"WHERE Id = {id}";
        }
        return await _sqlRep.GetQueryAsync<T>(sql);
    }
    
    public async Task<Result> DeleteEntityAsync<T>(int id) where T : class
    {
        if (!(await GetEntityByIdAsync<T>(id)).Any())
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