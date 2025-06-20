using AlunosService.Models;

namespace AlunosService.DTOs
{
    public class AlunoResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string? Endereco { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? CEP { get; set; }
        public StatusAluno Status { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string? Observacoes { get; set; }
        public int Idade { get; set; }
    }
}
