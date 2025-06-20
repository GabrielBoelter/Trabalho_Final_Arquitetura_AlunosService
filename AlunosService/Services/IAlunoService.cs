using AlunosService.DTOs;
using AlunosService.Models;

namespace AlunosService.Services
{
    public interface IAlunoService
    {
        Task<IEnumerable<AlunoResponseDTO>> GetAllAlunosAsync();
        Task<int> GetAllAlunosCountAsync(); 
        Task<AlunoResponseDTO?> GetAlunoByIdAsync(int id);
        Task<AlunoResponseDTO?> GetAlunoByEmailAsync(string email);
        Task<AlunoResponseDTO?> GetAlunoByCPFAsync(string cpf);
        Task<AlunoResponseDTO?> CreateAlunoAsync(AlunoCreateDTO alunoDto);
        Task<AlunoResponseDTO?> UpdateAlunoAsync(int id, AlunoUpdateDTO alunoDto);
        Task<bool> DeleteAlunoAsync(int id);
        Task<IEnumerable<AlunoResponseDTO>> GetAlunosByStatusAsync(StatusAluno status);
        Task<IEnumerable<AlunoResponseDTO>> SearchAlunosAsync(string searchTerm);
        Task<bool> AlunoExisteAsync(int id);
        Task<bool> ValidarEmailUnicoAsync(string email, int? alunoId = null);
        Task<bool> ValidarCPFUnicoAsync(string cpf, int? alunoId = null);
    }
}