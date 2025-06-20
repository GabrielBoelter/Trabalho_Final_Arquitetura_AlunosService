namespace AlunosService.DTOs
{
    public class AlunoPagamentosDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<PagamentoExternalDTO> Pagamentos { get; set; } = new();
        public List<AcessoTreinoExternalDTO> AcessosAtivos { get; set; } = new();
    }

    public class PagamentoExternalDTO
    {
        public int Id { get; set; }
        public int TreinoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public string Status { get; set; } = string.Empty;
        public string FormaPagamento { get; set; } = string.Empty;
        public string NomeTreino { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
    }

    public class AcessoTreinoExternalDTO
    {
        public int Id { get; set; }
        public int TreinoId { get; set; }
        public string NomeTreino { get; set; } = string.Empty;
        public DateTime DataLiberacao { get; set; }
        public DateTime DataExpiracao { get; set; }
        public bool Ativo { get; set; }
    }
}