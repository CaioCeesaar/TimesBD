using System.ComponentModel.DataAnnotations.Schema;

namespace TimesBD.Entities;

public class Jogador
{
    protected Jogador() { }

    public Jogador(string nome, DateTime dataNascimento, int timeId, string cep, string logradouro, string complemento, string uf, string ibge, string gia, string ddd, string siafi)
    {
        Nome = nome;
        DataNascimento = dataNascimento;
        TimeId = timeId;
        Cep = cep;
        Logradouro = logradouro;
        Complemento = complemento;
        Uf = uf;
        Ibge = ibge;
        Gia = gia;
        Ddd = ddd;
        Siafi = siafi;
    }
    
    public int Id { get; set; }
    public string NomeTime {get; set;}
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }
    public int TimeId { get; set; }
    public string Cep { get; set; }
    public string Logradouro { get;  set; }
    public string Complemento { get;  set; }
    public string Bairro { get;  set; }
    public string Localidade { get;  set; }
    public string Uf { get;  set; }
    public string Ibge { get;  set; }
    public string Gia { get;  set; }
    public string Ddd { get;  set; }
    public string Siafi { get;  set; }
    
}

public class JogadorModel
{
    public string Nome { get; set; }
    
    public DateTime DataNascimento { get; set; }
    
    public int? TimeId { get; set; }
    
    public string Cep { get; set; }
}   
