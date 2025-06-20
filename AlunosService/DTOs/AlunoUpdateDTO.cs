using System.ComponentModel.DataAnnotations;
using AlunosService.Models;

namespace AlunosService.DTOs
{
    public class AlunoUpdateDTO
    {
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string? Nome { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido")]
        [MaxLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string? Email { get; set; }

        [MaxLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [MaxLength(14, ErrorMessage = "CPF deve ter no máximo 14 caracteres")]
        public string? CPF { get; set; }

        public DateTime? DataNascimento { get; set; }

        [MaxLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres")]
        public string? Endereco { get; set; }

        [MaxLength(50, ErrorMessage = "Cidade deve ter no máximo 50 caracteres")]
        public string? Cidade { get; set; }

        [MaxLength(2, ErrorMessage = "Estado deve ter no máximo 2 caracteres")]
        public string? Estado { get; set; }

        [MaxLength(10, ErrorMessage = "CEP deve ter no máximo 10 caracteres")]
        public string? CEP { get; set; }

        public StatusAluno? Status { get; set; }

        [MaxLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
        public string? Observacoes { get; set; }
    }
}