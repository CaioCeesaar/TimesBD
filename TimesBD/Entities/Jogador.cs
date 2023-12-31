﻿using System.ComponentModel.DataAnnotations.Schema;

namespace TimesBD.Entities;

public class Jogador
{
    protected Jogador() { }

    public Jogador(string nome, DateTime dataNascimento, int timeId, int enderecoId)
    {
        Nome = nome;
        DataNascimento = dataNascimento;
        TimeId = timeId;
        EnderecoId = enderecoId;
    }
    
    public int Id { get; set; }
    public string NomeTime {get; set;}
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }
    public int TimeId { get; set; }
    public int EnderecoId { get; set; }
    public Endereco EnderecoJogador { get; set; }
    
}

public class JogadorPost
{
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }
    public int TimeId { get; set; }
    public string Cep { get; set; }
}

public class JogadorModel
{
    public string Nome { get; set; }
    
    public DateTime DataNascimento { get; set; }
    
    public int? TimeId { get; set; }
    
    public int EnderecoId { get; set; }
    
    public Endereco EnderecoModeloJogador { get; set; }
    
}   