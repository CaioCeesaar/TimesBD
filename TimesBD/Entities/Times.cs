namespace TimesBD.Entities;

public class Times
{
    protected Times(){}
    
    public Times(string nome, string cep, string logradouro, string bairro, string localidade, string complemento, string uf, string ibge, string gia, string ddd, string siafi)
    {
        Nome = nome;
        Cep = cep;
        Logradouro = logradouro;
        Bairro = bairro;
        Localidade = localidade;
        Complemento = complemento;
        Uf = uf;
        Ibge = ibge;
        Gia = gia;
        Ddd = ddd;
        Siafi = siafi;
    }

    public int Id { get;  set; }
    public string Nome { get; set; }
    public string Cep { get;  set; }
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

public class TimeModel
{
    public string Nome { get; set; }
    public string Cep { get; set; }
}
