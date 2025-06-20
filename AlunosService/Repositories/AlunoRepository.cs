using Microsoft.EntityFrameworkCore;
using AlunosService.Data;
using AlunosService.Models;

namespace AlunosService.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly AppDbContext _context;

        public AlunoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Aluno>> GetAllAsync()
        {
            return await _context.Alunos
                .OrderBy(a => a.Nome)
                .ToListAsync();
        }

        public async Task<Aluno?> GetByIdAsync(int id)
        {
            return await _context.Alunos.FindAsync(id);
        }

        public async Task<Aluno?> GetByEmailAsync(string email)
        {
            return await _context.Alunos
                .FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower());
        }

        public async Task<Aluno?> GetByCPFAsync(string cpf)
        {
            return await _context.Alunos
                .FirstOrDefaultAsync(a => a.CPF == cpf);
        }

        public async Task<Aluno> CreateAsync(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();
            return aluno;
        }

        public async Task<Aluno?> UpdateAsync(int id, Aluno aluno)
        {
            var existingAluno = await _context.Alunos.FindAsync(id);
            if (existingAluno == null) return null;

            existingAluno.Nome = aluno.Nome;
            existingAluno.Email = aluno.Email;
            existingAluno.Telefone = aluno.Telefone;
            existingAluno.CPF = aluno.CPF;
            existingAluno.DataNascimento = aluno.DataNascimento;
            existingAluno.Endereco = aluno.Endereco;
            existingAluno.Cidade = aluno.Cidade;
            existingAluno.Estado = aluno.Estado;
            existingAluno.CEP = aluno.CEP;
            existingAluno.Status = aluno.Status;
            existingAluno.Observacoes = aluno.Observacoes;
            existingAluno.DataAtualizacao = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingAluno;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null) return false;

            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Aluno>> GetByStatusAsync(StatusAluno status)
        {
            return await _context.Alunos
                .Where(a => a.Status == status)
                .OrderBy(a => a.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Aluno>> SearchAsync(string searchTerm)
        {
            return await _context.Alunos
                .Where(a => a.Nome.Contains(searchTerm) ||
                           a.Email.Contains(searchTerm) ||
                           (a.CPF != null && a.CPF.Contains(searchTerm)))
                .OrderBy(a => a.Nome)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Alunos.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Alunos.AnyAsync(a => a.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> CPFExistsAsync(string cpf)
        {
            return await _context.Alunos.AnyAsync(a => a.CPF == cpf);
        }
    
        public async Task<int> GetCountAsync()
        {
            return await _context.Alunos.CountAsync();
        }
    }
}
