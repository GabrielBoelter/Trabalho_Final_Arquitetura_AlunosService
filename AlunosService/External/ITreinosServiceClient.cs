namespace AlunosService.External
{
    public interface ITreinosServiceClient
    {
        Task<TreinoExternalDTO?> GetTreinoAsync(int treinoId);
        Task<IEnumerable<TreinoExternalDTO>> GetTreinosAtivosAsync();
    }

    public class TreinoExternalDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public bool Ativo { get; set; }
    }
}