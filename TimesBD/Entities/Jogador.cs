namespace TimesBD.Entities;

public class Jogador
{
    protected Jogador() { }

    public Jogador(string nome, DateTime dataNascimento, int time_id, string cep, string logradouro, string complemento, string bairro, string localidade, string uf, string ibge, string gia, string ddd, string siafi)
    {
        Nome = nome;
        DataNascimento = dataNascimento;
        Time_id = time_id;
        Cep = cep;
        Logradouro = logradouro;
        Complemento = complemento;
        Bairro = bairro;
        Localidade = localidade;
        Uf = uf;
        Ibge = ibge;
        Gia = gia;
        Ddd = ddd;
        Siafi = siafi;
    }
    
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public int Time_id { get; private set; }
    public string Cep { get; private set; }
    public string Logradouro { get; set; }
    public string Complemento { get; set; }
    public string Bairro { get; set; }
    public string Localidade { get; set; }
    public string Uf { get; set; }
    public string Ibge { get; set; }
    public string Gia { get; set; }
    public string Ddd { get; set; }
    public string Siafi { get; set; }
}