namespace TimesBD.Entities;

public class Estadios
{
    protected Estadios() { }

    public Estadios(string nome, int enderecoId, int limite)
    {
        Nome = nome;
        EnderecoId = enderecoId;
        Limite = limite;
    }

    public int Id { get; set; }
    public string Nome { get; set; }
    public int EnderecoId { get; set; }
    public int Limite { get; set; }
    public Endereco EnderecoEstadio { get; set; }
}

public class EstadiosPost
{
    public string Nome { get; set; }
    public int Limite { get; set; }
    public string Cep { get; set; }

}

public class EstadiosPatch
{
    public string Nome { get; set; }
    public int Limite { get; set; }
    public int EnderecoId { get; set; }
    public Endereco EnderecoModeloEstadio { get; set; }
}
