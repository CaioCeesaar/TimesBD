using Newtonsoft.Json;

namespace TimesBD.Models;

public class JogadorModel
{
    [JsonProperty("nome")]
    public string Nome { get; set; }
    
    public DateTime DataNascimento { get; set; }
    
    [JsonProperty("time_id")]
    public int Time_id { get; set; }
}