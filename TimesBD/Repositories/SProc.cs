namespace TimesBD.Repositories
{
    public static class SProc
    {
        public const string Jogadores = "sp_GetAllJogadores";
        public const string Compradores = "sp_GetAllCompradores";
        public const string Estadios = "sp_GetAllEstadios";
        public const string Ingressos = "sp_GetAllIngressos";
        public const string Jogos = "sp_GetAllJogos";
        public const string Partidas = "sp_GetAllPartidas";
        public const string Times = "sp_GetAllTimes";
        public const string Vendas = "sp_GetAllVendas";
        
        public const string Jogador = "sp_GetJogador";
        public const string Comprador = "sp_GetComprador";
        public const string Ingresso = "sp_GetIngresso";
        public const string Estadio = "sp_GetEstadio";
        public const string Jogo = "sp_GetJogo";
        public const string Partida = "sp_GetPartida";
        public const string Time = "sp_GetTime";
        public const string Venda = "sp_GetVenda";
            
        public const string InserirEstadio = "sp_InserirEstadio";
        public const string InserirComprador = "sp_InserirComprador";
        public const string InserirIngresso = "sp_InserirIngresso";
        public const string InserirJogador = "sp_InserirJogador";
        public const string InserirJogo = "sp_InserirJogo";
        public const string InserirPartida = "sp_InserirPartida";
        public const string InserirTime = "sp_InserirTime";
        public const string InserirVenda = "sp_InserirVenda";

        public const string AtualizarEstadio = "sp_AtualizarEstadio";
        public const string AtualizarComprador = "sp_AtualizarComprador";
        public const string AtualizarJogador = "sp_AtualizarJogador";
        public const string AtualizarJogo = "sp_AtualizarJogo";
        public const string AtualizarPartida = "sp_AtualizarPartida";
        public const string AtualizarTime = "sp_AtualizarTime";

        public const string DeletarComprador = "sp_DeletarComprador";
        public const string DeletarEstadio = "sp_DeletarEstadio";
        public const string DeletarIngresso = "sp_DeletarIngresso";
        public const string DeletarJogador = "sp_DeletarJogador";
        public const string DeletarJogo = "sp_DeletarJogo";
        public const string DeletarPartida = "sp_DeletarPartida";
        public const string DeletarTime = "sp_DeletarTime";
        public const string DeletarVenda = "sp_DeletarVenda";
    }
}