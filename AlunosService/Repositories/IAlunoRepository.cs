using AlunosService.Models;

namespace AlunosService.Repositories
{
    public interface IAlunoRepository
    {
        Task<IEnumerable<Aluno>> GetAllAsync();
        Task<Aluno?> GetByIdAsync(int id);
        Task<Aluno?> GetByEmailAsync(string email);
        Task<Aluno?> GetByCPFAsync(string cpf);
        Task<Aluno> CreateAsync(Aluno aluno);
        Task<Aluno?> UpdateAsync(int id, Aluno aluno);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Aluno>> GetByStatusAsync(StatusAluno status);
        Task<IEnumerable<Aluno>> SearchAsync(string searchTerm);
        Task<bool> ExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> CPFExistsAsync(string cpf);
        Task<int> GetCountAsync();

    }
}