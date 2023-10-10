namespace TimesBD.Entities;

public class Times
{
    protected Times(){}
    
    public Times(string nome, int jogadorId)
    {
        Nome = nome;
        JogadorId = jogadorId;
    }
    
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public int JogadorId { get; private set; }
}