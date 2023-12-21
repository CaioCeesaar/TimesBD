namespace TimesBD.Entities;

public class Ingresso
{
    protected Ingresso() { }

    public Ingresso(double valor, int jogoId)
    {
        Valor = valor;
        JogoId = jogoId;
    }

    public int Id { get; set; }
       
    public double Valor { get; set; }

    public int JogoId { get; set; }

}

public class IngressoPost
{
    public double Valor { get; set; }
    public int JogoId { get; set; }
}