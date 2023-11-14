namespace TimesBD.Entities
{
    protected Estadios() { }

    public Estadios(string nome, int limite, string cep, string logradouro, string complemento, string bairro, string localidade, string uf, string ibge, string gia, string ddd, string siafi)
    {
        Nome = nome;
        Limite = limite;
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

    public int Id { get; set; }
    public string Nome { get; set; }
    public int Limite { get; set; }
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

public class EstadiosModel
{
    public string Nome { get; set; }
    public int Limite { get; set; }
    public string Cep { get; set; }

}
