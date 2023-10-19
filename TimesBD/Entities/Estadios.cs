namespace TimesBD.Entities
{
    public class Estadios
    {
        protected Estadios() { }

        public Estadios(string nome, int enderecoId)
        {
            Nome = nome;
            EnderecoId = enderecoId;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int EnderecoId { get; set; }
        
        public Endereco EnderecoEstadio { get; set; }
    }
}
