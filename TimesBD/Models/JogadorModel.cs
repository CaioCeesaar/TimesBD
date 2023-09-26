using Newtonsoft.Json;

namespace TimesBD.Models;

public class JogadorModel
{
    public string Nome { get; set; }
    
    public DateTime DataNascimento { get; set; }
    
    public int TimeId { get; set; }
}