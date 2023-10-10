namespace TimesBD.Entities;

public class Times
{
    protected Times(){}
    
    public Times(string nome, int EnderecoId)
    {
        Nome = nome;
        EnderecoId = EnderecoId;
    }
    
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public int EnderecoId { get; private set; }
}