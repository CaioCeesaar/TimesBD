namespace TimesBD.Entities
{
    public class Ingresso
    {
        protected Ingresso() { }

        public Ingresso(double valor, int partidaId)
        {
            Valor = valor;
            PartidaId = partidaId;
        }

        public int Id { get; set; }
       
        public double Valor { get; set; }

        public int PartidaId { get; set; }

    }
}
