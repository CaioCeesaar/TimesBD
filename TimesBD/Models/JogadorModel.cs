using Newtonsoft.Json;
using TimesBD.Entities;

namespace TimesBD.Models;

public class JogadorModel
{
    public string Nome { get; set; }
    
    public DateTime DataNascimento { get; set; }
    
    public int TimeId { get; set; }
    
    public int EnderecoId { get; set; }
    
    public Endereco EnderecoModeloJogador { get; set; }
    
}   