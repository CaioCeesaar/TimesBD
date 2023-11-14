using TimesBD.Entities;

namespace TimesBD.Models;

public class TimeModel
{
    public string Nome { get; set; }
    
    public int EnderecoId { get; set; }
    
    public Endereco EnderecoModeloTime { get; set; }
}