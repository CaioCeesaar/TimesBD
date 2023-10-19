namespace TimesBD.Entities
{
    public class Jogo
    {
            protected Jogo() { }

            public Jogo(DateTime hora, DateTime data, int estadioId)
            {
                Hora = hora;
                Data = data;
                EstadioId = estadioId;
            }

            public int Id { get; set; }
            public DateTime Hora { get; set; }
            public DateTime Data { get; set; }
            public int EstadioId { get; set; }
        
    }
}
