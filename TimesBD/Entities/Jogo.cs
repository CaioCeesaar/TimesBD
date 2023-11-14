namespace TimesBD.Entities;

public class Jogo
{
    protected Jogo() { }

    //public Jogo(string hora, string data, int estadioId)
    //{
    //    Hora = hora;
    //    DataJogo = data;
    //    EstadioId = estadioId;
    //}

    public int Id { get; set; }
    public DateTime DataJogo { get; set; }
    public int EstadioId { get; set; }
        
}

public class JogoPostPatch
{
    public DateTime Data { get; set; }
    public int EstadioId { get; set; }
}