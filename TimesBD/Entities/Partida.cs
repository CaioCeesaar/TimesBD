namespace TimesBD.Entities
{
    public class Partida
    {
        protected Partida() { }

        public Partida(int timeId, int jogoId, int estadioId)
        {
            TimeID = timeId;
            JogoId = jogoId;
            EstadioId = estadioId;
        }

        public int Id { get; set; }
        public int TimeID { get; set; }
        public int JogoId { get; set; }

        public int EstadioId { get; set; }
    }
}
