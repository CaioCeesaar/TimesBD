namespace TimesBD.Entities;

public class Times
{
    protected Times(){}
    
    public Times(string nome, int enderecoId)
    {
        Nome = nome;
        EnderecoId = enderecoId;
    }

    public int Id { get;  set; }
    public string Nome { get; set; }
    public int EnderecoId { get;  set; }
    public Endereco EnderecoTime { get; set; }
}

public class TimesPost
{
    public string Nome { get; set; }
    public string Cep { get; set; }
}

public class TimeModel
{
    public string Nome { get; set; }
    
    public int EnderecoId { get; set; }
    
    public Endereco EnderecoModeloTime { get; set; }
}