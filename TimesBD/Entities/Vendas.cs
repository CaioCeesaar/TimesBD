namespace TimesBD.Entities
{
    public class Vendas
    {
        protected Vendas() { }

        public Vendas(DateTime dataVenda, int compradorId, int ingressoID)
        {
            DataVenda = dataVenda;
            CompradorId = compradorId;
            IngressoID = ingressoID;
        }

        public int Id { get; set; }
        public DateTime DataVenda { get; set; }
        public int CompradorId { get; set; }
        public int IngressoID { get; set; }

    }
}
