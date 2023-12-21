namespace TimesBD.Entities;

public class Venda
{
    protected Venda() { }

    public Venda(DateTime dataVenda, int compradorId, int ingressoId)
    {
        DataVenda = dataVenda;
        CompradorId = compradorId;
        IngressoID = ingressoId;
    }

    public int Id { get; set; }
    public DateTime DataVenda { get; set; }
    public int CompradorId { get; set; }
    public int IngressoID { get; set; }

}

public class VendasPostPatch
{
    public DateTime DataVenda { get; set; }
    public int CompradorId { get; set; }
    public int IngressoId { get; set; }
}