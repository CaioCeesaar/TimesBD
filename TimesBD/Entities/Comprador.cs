namespace TimesBD.Entities;

public class Comprador
{
    protected Comprador() { }

    public Comprador(string nome, string cpf) 
    { 
        Nome = nome;
        CPF = cpf;
    }

    public int Id { get; set; }
    public string Nome { get; set; }
    public string CPF { get; set; }
}

public class CompradorPostPatch
{
    public string Nome { get; set; }
    public string CPF { get; set; }
}

