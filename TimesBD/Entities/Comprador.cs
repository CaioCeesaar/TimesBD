namespace TimesBD.Entities;

public class Comprador
{
    protected Comprador() { }

    public Comprador(string nome, string cpf) 
    { 
        Nome = nome;
        Cpf = cpf;
    }

    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
}

public class CompradorPostPatch
{
    public CompradorPostPatch(string nome, string cpf)
    {
        Nome = nome;
        Cpf = cpf;
    }

    public string Nome { get; set; }
    public string Cpf { get; set; }
}

