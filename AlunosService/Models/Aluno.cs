using System.ComponentModel.DataAnnotations;

namespace AlunosService.Models
{
    public class Aluno
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Telefone { get; set; }

        [MaxLength(14)]
        public string? CPF { get; set; }

        public DateTime DataNascimento { get; set; }

        [MaxLength(200)]
        public string? Endereco { get; set; }

        [MaxLength(50)]
        public string? Cidade { get; set; }

        [MaxLength(2)]
        public string? Estado { get; set; }

        [MaxLength(10)]
        public string? CEP { get; set; }

        public StatusAluno Status { get; set; } = StatusAluno.Ativo;

        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        [MaxLength(500)]
        public string? Observacoes { get; set; }
    }

    public enum StatusAluno
    {
        Ativo = 1,
        Inativo = 2,
        Suspenso = 3,
        Bloqueado = 4
    }
}