using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TimesBD.Entities;

namespace TimesBD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JogadorController : ControllerBase
{
    private readonly string _connectionString;

    private const string Autentica = "d41d8cd98f00b204e9800998ecf8427e";

    public JogadorController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    [HttpGet("Jogadores")]
    public async Task<IActionResult> Get([FromQuery(Name = "name")] string? name = null,
        [FromQuery(Name = "id")] int? id = null, [FromQuery(Name = "Cep")] string? cep = null,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }

        string filtro = "";
        if (!String.IsNullOrEmpty(name))
        {
            filtro = "WHERE Nome = @name";
        }
        else if (id != null && id > 0)
        {
            filtro = "WHERE Id = @id";
        }
        else if (!String.IsNullOrEmpty(cep))
        {
            filtro = "WHERE Cep = @cep";
        }

        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Jogadores {filtro}";
        var jogadores = await sqlConnection.QueryAsync<Jogador>(sql, new { name, id, cep });
        return Ok(jogadores);
    }

    [HttpPatch("Jogadores")]
    public async Task<IActionResult> Patch([FromQuery] int id, JogadorModel atualizaJogador,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }

        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Jogadores WHERE Id = @id";
        var jogador = await sqlConnection.QueryAsync<Jogador>(sql, new { id });
        if (jogador is null)
        {
            return BadRequest("Jogador não encontrado");
        }

        if (String.IsNullOrEmpty(atualizaJogador.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }
        
        var endereco = await ConsultarCep(atualizaJogador.Cep);
        if (endereco is null)
        {
            return BadRequest("Cep inválido");
        }
        endereco.Localidade = endereco.Localidade.Replace("'", "''");
        
        var sql2 = $"UPDATE Jogadores SET Nome = '{atualizaJogador.Nome}', DataNascimento = '{atualizaJogador.DataNascimento}', TimeId = {atualizaJogador.TimeId}, Cep = '{atualizaJogador.Cep}', Complemento = '{endereco.Complemento}', Bairro = '{endereco.Bairro}', Localidade = '{endereco.Localidade}', Uf = '{endereco.Uf}', Ibge = '{endereco.Ibge}', Gia = '{endereco.Gia}', Ddd = '{endereco.Ddd}', Siafi = '{endereco.Siafi}', Logradouro = '{endereco.Logradouro}' WHERE Id = {id}";
        await sqlConnection.ExecuteAsync(sql2);
        return Ok();
    }


    [HttpPost("Jogadores")]
    public async Task<IActionResult> Post(JogadorModel jogador, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }

        using var sqlConnection = new SqlConnection(_connectionString);
        // verificar se o time existe
        
        
        if (string.IsNullOrEmpty(jogador.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }

        if (jogador.DataNascimento > DateTime.Now || jogador.DataNascimento < DateTime.Now.AddYears(-100))
        {
            return BadRequest("Data de nascimento não pode ser maior que a data atual ou menor que 100 anos atrás");
        }
        
        if (jogador.TimeId != null)
        {
            if (jogador.TimeId <= 0)
            {
                return BadRequest("TimeId não pode ser menor ou igual a zero");
            }
            
            var sqlTime = $"SELECT * FROM Times WHERE Id = {jogador.TimeId}";
            var time = await sqlConnection.QueryAsync<TimeModel>(sqlTime);
            if (time == null || !time.Any())
            {
                return BadRequest($"Time não encontrado: {jogador.TimeId}.");
            }
        }
        else
        {
            return BadRequest("TimeId não pode ser nulo");
        }
        

        var endereco = await ConsultarCep(jogador.Cep);
        if (endereco?.Cep is not null)
        {
            endereco.Localidade = endereco.Localidade.Replace("'", "''");
            string sql = $"INSERT INTO Jogadores (Nome, DataNascimento, TimeId, Cep, Logradouro, Complemento, Bairro, Localidade, Uf, Ibge, Gia, Ddd, Siafi) OUTPUT INSERTED.Id VALUES ('{jogador.Nome}', '{jogador.DataNascimento}', {jogador.TimeId}, '{endereco.Cep}', '{endereco.Logradouro}', '{endereco.Complemento}','{endereco.Bairro}', '{endereco.Localidade}', '{endereco.Uf}', '{endereco.Ibge}', '{endereco.Gia}', '{endereco.Ddd}', '{endereco.Siafi}')";
            await sqlConnection.ExecuteScalarAsync<int>(sql);
            return Ok();
        }
        return BadRequest("Cep inválido");
    }

    [HttpDelete("Jogadores")]
    public async Task<IActionResult> Delete([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }

        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync("EXEC sp_DeletarJogador {id}", new { id });
            return linhaAfetada == 0
                ? NotFound("O id informado não foi encontrado")
                : Ok("Jogador deletado com sucesso");
        }
    }

    [HttpGet("Times")]
    public async Task<IActionResult> Get([FromQuery(Name = "name")] string? name = null, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }

        string filtro = "";
        if (!String.IsNullOrEmpty(name))
        {
            filtro = "WHERE Nome = @name";
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Times {filtro}";
        var times = await sqlConnection.QueryAsync<Times>(sql, new { name });
        return Ok(times);
    }

    [HttpPatch("Times")]
    public async Task<IActionResult> Patch([FromQuery] int id, TimeModel atualizaTime,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Times WHERE Id = @id";
        var time = await sqlConnection.QueryAsync<Times>(sql, new { id });
        if (time is null)
        {
            return BadRequest("Time não encontrado");
        }
        
        if (String.IsNullOrEmpty(atualizaTime.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }
        
        var endereco = await ConsultarCep(atualizaTime.Cep);
        if (endereco is null)
        {
            return BadRequest("Cep inválido");
        }
        endereco.Localidade = endereco.Localidade.Replace("'", "''");
        
        var sql2 = $"UPDATE Times SET Nome = '{atualizaTime.Nome}', Cep = '{atualizaTime.Cep}', Complemento = '{endereco.Complemento}', Bairro = '{endereco.Bairro}', Localidade = '{endereco.Localidade}', Uf = '{endereco.Uf}', Ibge = '{endereco.Ibge}', Gia = '{endereco.Gia}', Ddd = '{endereco.Ddd}', Siafi = '{endereco.Siafi}', Logradouro = '{endereco.Logradouro}' WHERE Id = {id}";
        await sqlConnection.ExecuteAsync(sql2);
        return Ok("Time atualizado com sucesso");
    }

    [HttpPost("Times")]
    public async Task<IActionResult> Post(TimeModel time, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        
        if (string.IsNullOrEmpty(time.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }
        
        var endereco = await ConsultarCep(time.Cep);
        if (endereco?.Cep is not null)
        {
            endereco.Localidade = endereco.Localidade.Replace("'", "''");
            
            string sql = $"INSERT INTO Times (Nome, Cep, Logradouro, Complemento, Bairro, Localidade, Uf, Ibge, Gia, Ddd, Siafi) OUTPUT INSERTED.Id VALUES ('{time.Nome}', '{endereco.Cep}', '{endereco.Logradouro}', '{endereco.Complemento}','{endereco.Bairro}', '{endereco.Localidade}', '{endereco.Uf}', '{endereco.Ibge}', '{endereco.Gia}', '{endereco.Ddd}', '{endereco.Siafi}')";
            await sqlConnection.ExecuteScalarAsync<int>(sql);
            return Ok();
        }
        return BadRequest("Cep inválido");
    }

    [HttpDelete("Times")]
    public async Task<IActionResult> DeleteTimes([FromQuery] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        CommandDefinition cDTeste = new CommandDefinition($"EXEC sp_DeletarTime {id}");
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync(cDTeste);
            return linhaAfetada == 0
                ? BadRequest("O id informado não foi encontrado")
                : Ok("Time deletado com sucesso");
        }
    }

    [HttpGet("Estadios")]
    public async Task<IActionResult> Get([FromQuery(Name = "id")] int? id = null, [FromQuery(Name = "name")] string? name = null,
        [FromQuery(Name = "Cep")] string? cep = null, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }

        string filtro = "";
        if (!String.IsNullOrEmpty(name))
        {
            filtro = "WHERE Nome = @name";
        }
        else if (!String.IsNullOrEmpty(cep))
        {
            filtro = "WHERE Cep = @cep";
        }
        else if (id != null && id > 0)
        {
            filtro = "WHERE Id = @id";
        }

        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Estadios {filtro}";
        var estadios = await sqlConnection.QueryAsync<Estadios>(sql, new { name, cep, id });
        return Ok(estadios);
    }

    [HttpPost("Estadios")]
    public async Task<IActionResult> PostEstadios(EstadiosModel estadio, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }

        using var sqlConnection = new SqlConnection(_connectionString);

        if (string.IsNullOrEmpty(estadio.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }

        if (estadio.Limite <= 0)
        {
            return BadRequest("O limite do estádio não pode ser menor ou igual a zero");
        }

        var endereco = await ConsultarCep(estadio.Cep);
        if (endereco?.Cep is not null)
        {
            endereco.Localidade = endereco.Localidade.Replace("'", "''");
            string sql = $"INSERT INTO Estadios (Nome, Limite, Cep, Complemento, Bairro, Localidade, Uf, Ibge, Gia, Ddd, Siafi, Logradouro) OUTPUT INSERTED.Id VALUES ('{estadio.Nome}', {estadio.Limite}, '{endereco.Cep}', '{endereco.Complemento}','{endereco.Bairro}', '{endereco.Localidade}', '{endereco.Uf}', '{endereco.Ibge}', '{endereco.Gia}', '{endereco.Ddd}', '{endereco.Siafi}', '{endereco.Logradouro}')";
            await sqlConnection.ExecuteScalarAsync<int>(sql);
            return Ok();
        }

        return BadRequest("Cep inválido");
    }

    [HttpPatch("Estadios")]
    public async Task<IActionResult> Patch([FromQuery] int id, EstadiosModel estadio,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Estadios WHERE Id = @id";
        var estadioConsulta = await sqlConnection.QueryAsync<Estadios>(sql, new { id });
        if (estadioConsulta is null)
        {
            return BadRequest("Estádio não encontrado");
        }

        if (String.IsNullOrEmpty(estadio.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }

        if (estadio.Limite <= 0)
        {
            return BadRequest("Limite inválido, não pode ser menor ou igual a zero");
        }


        var endereco = await ConsultarCep(estadio.Cep);
        if (endereco is null)
        {
            return BadRequest("Cep inválido");
        }
        endereco.Localidade.Replace("'", "''");

        var sql2 = $"UPDATE Estadios SET Nome = '{estadio.Nome}', Limite = {estadio.Limite}, Cep = '{estadio.Cep}', Complemento = '{endereco.Complemento}', Bairro = '{endereco.Bairro}', Localidade = '{endereco.Localidade}', Uf = '{endereco.Uf}', Ibge = '{endereco.Ibge}', Gia = '{endereco.Gia}', Ddd = '{endereco.Ddd}', Siafi = '{endereco.Siafi}', Logradouro = '{endereco.Logradouro}' WHERE Id = {id}";
        await sqlConnection.ExecuteAsync(sql2);
        return Ok("Estadio atualizado com sucesso");
    }
    
    [HttpDelete("Estadios")]
    public async Task<IActionResult> DeleteEstadios([FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync($"EXEC sp_DeletarEstadio {id}", new { id });
            return linhaAfetada == 0
                ? NotFound("O id informado não foi encontrado")
                : Ok("Estádio deletado com sucesso");
        }
    } 

    [HttpGet("Ingressos")]
    public async Task<IActionResult> Get([FromQuery(Name = "id")] int? id = null, [FromQuery(Name = "valor")] double? valor = null,
        [FromQuery(Name = "partidaId")] int? partidaId = null, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");

        string filtro = "";
        if (id != null && id > 0)
        {
            filtro = "WHERE Id = @id";
        }
        else if (valor != null && valor > 0)
        {
            filtro = "WHERE Valor = @valor";
        }
        else if (partidaId != null && partidaId > 0)
        {
            filtro = "WHERE PartidaId = @partidaId";
        }

        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Ingresso {filtro}";
        var ingressos = await sqlConnection.QueryAsync<Ingresso>(sql, new { id, valor, partidaId });

        return Ok(ingressos);
    }

    [HttpPost("Ingressos")]
    public async Task<IActionResult> PostIngressos(IngressoPost ingresso,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        
        if (ingresso.Valor <= 0)
        {
            return BadRequest("Valor não pode ser menor ou igual a zero");
        }
        
        if (ingresso.PartidaId <= 0)
        {
            return BadRequest("PartidaId não pode ser menor ou igual a zero");
        }
        
        // valida se a partida existe
        var sqlPartida = $"SELECT * FROM Partida WHERE Id = {ingresso.PartidaId}";
        var partida = await sqlConnection.QueryAsync<Partida>(sqlPartida);
        if (partida == null || !partida.Any())
        {
            return BadRequest($"Partida não encontrada: {ingresso.PartidaId}.");
        }
        
        string sql = $"EXEC sp_InserirIngresso {ingresso.Valor}, {ingresso.PartidaId}";
        await sqlConnection.ExecuteAsync(sql);
        return Ok();
    }
    
    [HttpDelete("Ingressos")]
    public async Task<IActionResult> DeleteIngressos([FromQuery] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync($"EXEC sp_DeletarIngresso {id}", new { id });
            return linhaAfetada == 0
                ? NotFound("O id informado não foi encontrado")
                : Ok("Time deletado com sucesso");
        }
    }

    [HttpGet("Partidas")]
    public async Task<IActionResult> Get([FromHeader(Name = "autentica")] string? autentica = null,
        [FromQuery(Name = "id")] int? id = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        string filtro = "";
        if (id != null && id > 0)
        {
            filtro = "WHERE Id = @id";
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Partida {filtro}";
        var partidas = await sqlConnection.QueryAsync<Partida>(sql, new { id });

        return Ok(partidas);
    }

    [HttpPost("Partidas")]
    public async Task<IActionResult> PostPartidas(PartidaPostPatch partida,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        
        if (partida.TimeID > 0)
        {
            var sqlTime = $"SELECT * FROM Times WHERE Id = {partida.TimeID}";
            var time = await sqlConnection.QueryAsync<TimeModel>(sqlTime);
            if (time == null || !time.Any())
            {
                return BadRequest($"Time não encontrado: {partida.TimeID}.");
            }
        }
        else
        {
            return BadRequest("TimeId não pode ser nulo");
        }
        
        if (partida.JogoId > 0)
        {
            var sqlJogo = $"SELECT * FROM Jogo WHERE Id = {partida.JogoId}";
            var jogo = await sqlConnection.QueryAsync<Jogo>(sqlJogo);
            if (jogo == null || !jogo.Any())
            {
                return BadRequest($"Jogo não encontrado: {partida.JogoId}.");
            }
        }
        else
        {
            return BadRequest("JogoId não pode ser nulo");
        }

        if (partida.EstadioId > 0)
        {
            var sqlEstadio = $"SELECT * FROM Estadios WHERE Id = {partida.EstadioId}";
            var estadio = await sqlConnection.QueryAsync<Estadios>(sqlEstadio);
            if (estadio == null || !estadio.Any())
            {
                return BadRequest($"Estádio não encontrado: {partida.EstadioId}.");
            }
        }
        else
        {
            return BadRequest("EstadioId não pode ser nulo");
        }
        
        string sql = $"EXEC sp_InserirPartida {partida.TimeID}, {partida.JogoId}, {partida.EstadioId}";
        await sqlConnection.ExecuteAsync(sql);
        return Ok();
    }

    [HttpPatch("Partidas")]
    public async Task<IActionResult> Patch(PartidaPostPatch partida,
        [FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Partida WHERE Id = @id";
        var partidaConsulta = await sqlConnection.QueryAsync<Partida>(sql, new { id });
        if (partidaConsulta is null)
        {
            return BadRequest("Partida não encontrada");
        }

        if (partida.TimeID >= 0)
        {
            var sqlTime = $"SELECT * FROM Times WHERE Id = {partida.TimeID}";
            var time = await sqlConnection.QueryAsync<TimeModel>(sqlTime);
            if (time == null || !time.Any())
            {
                return BadRequest($"Time não encontrado: {partida.TimeID}.");
            }
        }

        if (partida.JogoId > 0)
        {
            var sqlJogo = $"SELECT * FROM Jogo WHERE Id = {partida.JogoId}";
            var jogo = await sqlConnection.QueryAsync<Jogo>(sqlJogo);
            if (jogo == null || !jogo.Any())
            {
                return BadRequest($"Jogo não encontrado: {partida.JogoId}.");
            }
        }

        if (partida.EstadioId > 0)
        {
            var sqlEstadio = $"SELECT * FROM Estadios WHERE Id = {partida.EstadioId}";
            var estadio = await sqlConnection.QueryAsync<Estadios>(sqlEstadio);
            if (estadio == null || !estadio.Any())
            {
                return BadRequest($"Estádio não encontrado: {partida.EstadioId}.");
            }
        }
        var sql2 = $"EXEC sp_AtualizarPartida {id}, {partida.TimeID}, {partida.JogoId}, {partida.EstadioId}";
        await sqlConnection.ExecuteAsync(sql2);
        return Ok("Partida atualizada com sucesso");
    }

    [HttpDelete("Partidas")]
    public async Task<IActionResult> DeletePartidas([FromQuery] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync($"EXEC sp_DeletarPartida {id}", new { id });
            return linhaAfetada == 0
                ? NotFound("O id informado não foi encontrado")
                : Ok("Time deletado com sucesso");
        }
    }

    [HttpGet("Compradores")]
    public async Task<IActionResult> Get([FromHeader(Name = "autentica")] string? autentica = null, [FromQuery(Name = "Nome")] string? nome = null, [FromQuery(Name = "CPF")] string? cpf = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        string filtro = "";
        if (!String.IsNullOrEmpty(nome))
        {
            filtro = "WHERE Nome = @nome";
        }
        else if (!String.IsNullOrEmpty(cpf))
        {
            filtro = "WHERE CPF = @cpf";
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Comprador {filtro}";
        var compradores = await sqlConnection.QueryAsync<Comprador>(sql, new { nome, cpf });
        return Ok(compradores);
    }

    [HttpPost("Compradores")]
    public async Task<IActionResult> PostCompradores(CompradorPostPatch comprador,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);

        if (string.IsNullOrEmpty(comprador.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }
        
        if (string.IsNullOrEmpty(comprador.CPF))
        {
            return BadRequest("CPF não pode ser nulo ou vazio");
        }
        
        if (comprador.CPF.Length != 11)
        {
            return BadRequest("CPF inválido");
        }
        
        var cpfExiste = $"SELECT * FROM Comprador WHERE CPF = '{comprador.CPF}'";
        var cpfConsulta = await sqlConnection.QueryAsync<Comprador>(cpfExiste);
        if (cpfConsulta != null && cpfConsulta.Any())
        {
            return BadRequest("CPF já cadastrado");
        }
        
        string sql = $"EXEC sp_InserirComprador '{comprador.Nome}', '{comprador.CPF}'";
        await sqlConnection.ExecuteAsync(sql);
        return Ok();
    }
    
    [HttpPatch("Compradores")]
    public async Task<IActionResult> Patch(CompradorPostPatch comprador,
        [FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Comprador WHERE Id = @id";
        var compradorConsulta = await sqlConnection.QueryAsync<Comprador>(sql, new { id });
        if (compradorConsulta is null)
        {
            return BadRequest("Comprador não encontrado");
        }

        if (String.IsNullOrEmpty(comprador.Nome))
        {
            return BadRequest("Nome não pode ser nulo ou vazio");
        }
        var parameters = $"{id}";
        if (!string.Equals(comprador.Nome, "string", StringComparison.OrdinalIgnoreCase))
        {
            parameters += $", '{comprador.Nome}'";
        }

        if (String.IsNullOrEmpty(comprador.CPF))
        {
            return BadRequest("CPF não pode ser nulo ou vazio");
        }
        
        if (comprador.CPF.Length != 11)
        {
            return BadRequest("CPF inválido");
        }
        
        var cpfExiste = $"SELECT * FROM Comprador WHERE CPF = '{comprador.CPF}'";
        var cpfConsulta = await sqlConnection.QueryAsync<Comprador>(cpfExiste);
        if (cpfConsulta != null && cpfConsulta.Any())
        {
            return BadRequest("CPF já cadastrado");
        }
        
        sql = $"EXEC sp_AtualizarComprador {parameters}, '{comprador.CPF}'";
        await sqlConnection.ExecuteAsync(sql);
        return Ok("Comprador atualizado com sucesso");
    }
    
    [HttpDelete("Compradores")]
    public async Task<IActionResult> DeleteCompradores([FromQuery] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");
        
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync($"EXEC sp_DeletarComprador {id}", new { id });
            return linhaAfetada == 0
                ? NotFound("O id informado não foi encontrado")
                : Ok("Comprador deletado com sucesso");
        }
    }
    
    [HttpGet("Vendas")]
    public async Task<IActionResult> Get([FromHeader(Name = "autentica")] string? autentica = null, [FromQuery(Name = "id")] int? id = null, [FromQuery(Name = "compradorId")] int? compradorId = null, [FromQuery(Name = "ingressoId")] int? ingressoId = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");
        
        string filtro = "";
        if (id != null && id > 0)
        {
            filtro = "WHERE Id = @id";
        }
        else if (compradorId != null && compradorId > 0)
        {
            filtro = "WHERE CompradorId = @compradorId";
        }
        else if (ingressoId != null && ingressoId > 0)
        {
            filtro = "WHERE IngressoId = @ingressoId";
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Vendas {filtro}";
        var vendas = await sqlConnection.QueryAsync<Vendas>(sql, new { id, compradorId, ingressoId });

        return Ok(vendas);
    }
    
    [HttpPost("Vendas")]
    public async Task<IActionResult> PostVendas(VendasPostPatch venda,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");
        
        using var sqlConnection = new SqlConnection(_connectionString);
        
        if (venda.CompradorId <= 0)
        {
            return BadRequest("CompradorId não pode ser menor ou igual a zero");
        }
        
        if (venda.IngressoID <= 0)
        {
            return BadRequest("IngressoId não pode ser menor ou igual a zero");
        }
        
        if(venda.DataVenda > DateTime.Now)
        {
            return BadRequest("Data de venda não pode ser maior que a data atual");
        }
        
        // valida se o comprador existe
        var sqlComprador = $"SELECT * FROM Comprador WHERE Id = {venda.CompradorId}";
        var comprador = await sqlConnection.QueryAsync<Comprador>(sqlComprador);
        if (comprador == null || !comprador.Any())
        {
            return BadRequest($"Comprador não encontrado: {venda.CompradorId}.");
        }
        
        // valida se o ingresso existe
        var sqlIngresso = $"SELECT * FROM Ingresso WHERE Id = {venda.IngressoID}";
        var ingresso = await sqlConnection.QueryAsync<Ingresso>(sqlIngresso);
        if (ingresso == null || !ingresso.Any())
        {
            return BadRequest($"Ingresso não encontrado: {venda.IngressoID}.");
        }

        string sql = $"EXEC sp_InserirVenda '{venda.DataVenda}', {venda.CompradorId}, {venda.IngressoID}";
        await sqlConnection.ExecuteAsync(sql);
        return Ok();
    }
    
    [HttpDelete("Vendas")]
    public async Task<IActionResult> DeleteVendas([FromQuery] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");
        
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync($"EXEC sp_DeletarVenda {id}", new { id });
            return linhaAfetada == 0
                ? NotFound("O id informado não foi encontrado")
                : Ok("Venda deletada com sucesso");
        }
    }

    [HttpGet("Jogos")]
    public async Task<IActionResult> GetJogos([FromHeader(Name = "Autentica")] string? autentica = null, [FromQuery(Name = "id")] int? id = null, [FromQuery(Name = "DataJogo")] DateTime? data = null, [FromQuery(Name = "EstadioId")] int? estadioId = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        string filtro = "";
        if (id != null && id > 0)
        {
            filtro = "WHERE Id = @id";
        }
        else if (data != null)
        {
            filtro = "WHERE DataJogo = @data";
        }
        else if (estadioId != null && estadioId > 0)
        {
            filtro = "WHERE EstadioId = @estadioId";
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Jogo {filtro}";
        var jogos = await sqlConnection.QueryAsync<Jogo>(sql, new { id, data, estadioId });
        return Ok(jogos); 
    }

    [HttpPost("Jogos")]
    public async Task<IActionResult> PostJogos(JogoPostPatch jogo,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request))
        {
            return BadRequest("Autenticação inválida");
        }
        
        using var sqlConnection = new SqlConnection(_connectionString);
        
        if (jogo.EstadioId <= 0)
        {
            return BadRequest("EstadioId não pode ser menor ou igual a zero");
        }
        
        // valida se o estadio existe
        var sqlEstadio = $"SELECT * FROM Estadios WHERE Id = {jogo.EstadioId}";
        var estadio = await sqlConnection.QueryAsync<Estadios>(sqlEstadio);
        if (estadio == null || !estadio.Any())
        {
            return BadRequest($"Estádio não encontrado: {jogo.EstadioId}.");
        }

        string sql = $"EXEC sp_InserirJogo '{jogo.Data}', {jogo.EstadioId}";
        await sqlConnection.ExecuteAsync(sql);
        return Ok();
    }
    
    [HttpPatch("Jogos")]
    public async Task<IActionResult> Patch(JogoPostPatch jogo,
        [FromQuery] int id,
        [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");
        
        using var sqlConnection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Jogo WHERE Id = @id";
        var jogoConsulta = await sqlConnection.QueryAsync<Jogo>(sql, new { id });
        if (jogoConsulta is null)
        {
            return BadRequest("Jogo não encontrado");
        }
        
        var parameters = $"{id}";
        if (!(jogo.Data > DateTime.Now))
        {
            parameters += $", '{jogo.Data}'";
        }
        else
        {
            return BadRequest("A data do jogo não pode ser maior que a data atual");
        }
        
        if (jogo.EstadioId > 0)
        {
            var sqlEstadio = $"SELECT * FROM Estadios WHERE Id = {jogo.EstadioId}";
            var estadio = await sqlConnection.QueryAsync<Estadios>(sqlEstadio);
            if (estadio == null || !estadio.Any())
            {
                return BadRequest($"Estádio não encontrado: {jogo.EstadioId}.");
            }
        }
        
        var sql2 = $"EXEC sp_AtualizarJogo {parameters}, {jogo.EstadioId}";
        await sqlConnection.ExecuteAsync(sql2);
        return Ok("Jogo atualizado com sucesso");
    }
    
    [HttpDelete("Jogos")]
    public async Task<IActionResult> DeleteJogos([FromQuery] int id, [FromHeader(Name = "Autentica")] string? autentica = null)
    {
        if (!ValidarAutenticacao(Request)) return BadRequest("Autenticação inválida");
        
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            var linhaAfetada = await sqlConnection.ExecuteAsync($"EXEC sp_DeletarJogo {id}", new { id });
            return linhaAfetada == 0
                ? NotFound("O id informado não foi encontrado")
                : Ok("Jogo deletado com sucesso");
        }
    }
    
    private static async Task<Endereco?> ConsultarCep(string cep)
    {
        var client = new HttpClient();
        var url = $"https://viacep.com.br/ws/{cep}/json/";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var endereco = JsonConvert.DeserializeObject<Endereco>(content);
            return endereco;
        }
        return null;
    }
    
    private static bool ValidarAutenticacao(HttpRequest request) => request.Headers.TryGetValue("autentica", out var autentica) && autentica == Autentica;

}