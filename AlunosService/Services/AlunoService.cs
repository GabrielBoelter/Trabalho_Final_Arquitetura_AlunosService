using AlunosService.DTOs;
using AlunosService.External;
using AlunosService.Models;
using AlunosService.Repositories;

namespace AlunosService.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IPagamentosServiceClient _pagamentosClient;
        private readonly ITreinosServiceClient _treinosClient;
        private readonly ILogger<AlunoService> _logger;

        public AlunoService(
            IAlunoRepository alunoRepository,
            IPagamentosServiceClient pagamentosClient,
            ITreinosServiceClient treinosClient,
            ILogger<AlunoService> logger)
        {
            _alunoRepository = alunoRepository;
            _pagamentosClient = pagamentosClient;
            _treinosClient = treinosClient;
            _logger = logger;
        }

        public async Task<IEnumerable<AlunoResponseDTO>> GetAllAlunosAsync()
        {
            try
            {
                _logger.LogInformation("Obtendo todos os alunos");
                var alunos = await _alunoRepository.GetAllAsync();
                return alunos.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os alunos");
                throw;
            }
        }

        public async Task<int> GetAllAlunosCountAsync()
        {
            try
            {
                _logger.LogInformation("Obtendo total de alunos");
                return await _alunoRepository.GetCountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter total de alunos");
                throw;
            }
        }

        public async Task<AlunoResponseDTO?> GetAlunoByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Obtendo aluno por ID: {Id}", id);
                var aluno = await _alunoRepository.GetByIdAsync(id);
                return aluno != null ? MapToResponseDto(aluno) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter aluno por ID: {Id}", id);
                throw;
            }
        }

        public async Task<AlunoResponseDTO?> GetAlunoByEmailAsync(string email)
        {
            try
            {
                _logger.LogInformation("Obtendo aluno por email: {Email}", email);
                var aluno = await _alunoRepository.GetByEmailAsync(email);
                return aluno != null ? MapToResponseDto(aluno) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter aluno por email: {Email}", email);
                throw;
            }
        }

        public async Task<AlunoResponseDTO?> GetAlunoByCPFAsync(string cpf)
        {
            try
            {
                _logger.LogInformation("Obtendo aluno por CPF: {CPF}", cpf);
                var aluno = await _alunoRepository.GetByCPFAsync(cpf);
                return aluno != null ? MapToResponseDto(aluno) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter aluno por CPF: {CPF}", cpf);
                throw;
            }
        }

        public async Task<AlunoResponseDTO?> CreateAlunoAsync(AlunoCreateDTO alunoDto)
        {
            try
            {
                _logger.LogInformation("Criando novo aluno: {Nome}", alunoDto.Nome);

                // Validações usando os novos métodos
                if (!await ValidarEmailUnicoAsync(alunoDto.Email))
                {
                    _logger.LogWarning("Tentativa de criar aluno com email já existente: {Email}", alunoDto.Email);
                    return null;
                }

                if (!string.IsNullOrEmpty(alunoDto.CPF) && !await ValidarCPFUnicoAsync(alunoDto.CPF))
                {
                    _logger.LogWarning("Tentativa de criar aluno com CPF já existente: {CPF}", alunoDto.CPF);
                    return null;
                }

                var aluno = new Aluno
                {
                    Nome = alunoDto.Nome,
                    Email = alunoDto.Email,
                    Telefone = alunoDto.Telefone,
                    CPF = alunoDto.CPF,
                    DataNascimento = alunoDto.DataNascimento,
                    Endereco = alunoDto.Endereco,
                    Cidade = alunoDto.Cidade,
                    Estado = alunoDto.Estado,
                    CEP = alunoDto.CEP,
                    Observacoes = alunoDto.Observacoes,
                    Status = StatusAluno.Ativo,
                    DataCadastro = DateTime.Now
                };

                var createdAluno = await _alunoRepository.CreateAsync(aluno);
                _logger.LogInformation("Aluno criado com sucesso. ID: {Id}", createdAluno.Id);

                return MapToResponseDto(createdAluno);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar aluno: {Nome}", alunoDto.Nome);
                throw;
            }
        }

        public async Task<AlunoResponseDTO?> UpdateAlunoAsync(int id, AlunoUpdateDTO alunoDto)
        {
            try
            {
                _logger.LogInformation("Atualizando aluno. ID: {Id}", id);

                var existingAluno = await _alunoRepository.GetByIdAsync(id);
                if (existingAluno == null)
                {
                    _logger.LogWarning("Aluno não encontrado para atualização. ID: {Id}", id);
                    return null;
                }

                // Validar email único se foi alterado usando o novo método
                if (!string.IsNullOrEmpty(alunoDto.Email) &&
                    !await ValidarEmailUnicoAsync(alunoDto.Email, id))
                {
                    _logger.LogWarning("Tentativa de atualizar aluno {Id} com email já existente: {Email}", id, alunoDto.Email);
                    return null;
                }

                // Validar CPF único se foi alterado usando o novo método
                if (!string.IsNullOrEmpty(alunoDto.CPF) &&
                    !await ValidarCPFUnicoAsync(alunoDto.CPF, id))
                {
                    _logger.LogWarning("Tentativa de atualizar aluno {Id} com CPF já existente: {CPF}", id, alunoDto.CPF);
                    return null;
                }

                // Aplicar updates apenas nos campos fornecidos
                if (!string.IsNullOrEmpty(alunoDto.Nome)) existingAluno.Nome = alunoDto.Nome;
                if (!string.IsNullOrEmpty(alunoDto.Email)) existingAluno.Email = alunoDto.Email;
                if (alunoDto.Telefone != null) existingAluno.Telefone = alunoDto.Telefone;
                if (alunoDto.CPF != null) existingAluno.CPF = alunoDto.CPF;
                if (alunoDto.DataNascimento.HasValue) existingAluno.DataNascimento = alunoDto.DataNascimento.Value;
                if (alunoDto.Endereco != null) existingAluno.Endereco = alunoDto.Endereco;
                if (alunoDto.Cidade != null) existingAluno.Cidade = alunoDto.Cidade;
                if (alunoDto.Estado != null) existingAluno.Estado = alunoDto.Estado;
                if (alunoDto.CEP != null) existingAluno.CEP = alunoDto.CEP;
                if (alunoDto.Status.HasValue) existingAluno.Status = alunoDto.Status.Value;
                if (alunoDto.Observacoes != null) existingAluno.Observacoes = alunoDto.Observacoes;

                var updatedAluno = await _alunoRepository.UpdateAsync(id, existingAluno);

                if (updatedAluno != null)
                {
                    _logger.LogInformation("Aluno atualizado com sucesso. ID: {Id}", id);
                    return MapToResponseDto(updatedAluno);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar aluno. ID: {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAlunoAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deletando aluno. ID: {Id}", id);

                // Verificar se o aluno tem pagamentos antes de deletar
                var temPagamentos = await _pagamentosClient.AlunoTemPagamentosAsync(id);
                if (temPagamentos)
                {
                    _logger.LogWarning("Tentativa de deletar aluno {Id} que possui pagamentos", id);
                    return false;
                }

                var result = await _alunoRepository.DeleteAsync(id);

                if (result)
                {
                    _logger.LogInformation("Aluno deletado com sucesso. ID: {Id}", id);
                }
                else
                {
                    _logger.LogWarning("Falha ao deletar aluno. ID: {Id}", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar aluno. ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<AlunoResponseDTO>> GetAlunosByStatusAsync(StatusAluno status)
        {
            try
            {
                _logger.LogInformation("Obtendo alunos por status: {Status}", status);
                var alunos = await _alunoRepository.GetByStatusAsync(status);
                return alunos.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter alunos por status: {Status}", status);
                throw;
            }
        }

        public async Task<IEnumerable<AlunoResponseDTO>> SearchAlunosAsync(string searchTerm)
        {
            try
            {
                _logger.LogInformation("Buscando alunos com termo: {SearchTerm}", searchTerm);
                var alunos = await _alunoRepository.SearchAsync(searchTerm);
                return alunos.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar alunos com termo: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<bool> AlunoExisteAsync(int id)
        {
            try
            {
                return await _alunoRepository.ExistsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar se aluno existe. ID: {Id}", id);
                throw;
            }
        }

        // Novos métodos adicionados para implementar a interface
        public async Task<bool> ValidarEmailUnicoAsync(string email, int? alunoId = null)
        {
            try
            {
                _logger.LogInformation("Validando unicidade do email: {Email}", email);

                var alunoExistente = await _alunoRepository.GetByEmailAsync(email);

                if (alunoExistente == null)
                {
                    return true; // Email não existe, é único
                }

                // Se um ID foi fornecido, verifica se o email pertence ao mesmo aluno
                if (alunoId.HasValue && alunoExistente.Id == alunoId.Value)
                {
                    return true; // Email pertence ao mesmo aluno, é válido
                }

                return false; // Email já existe para outro aluno
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar unicidade do email: {Email}", email);
                throw;
            }
        }

        public async Task<bool> ValidarCPFUnicoAsync(string cpf, int? alunoId = null)
        {
            try
            {
                _logger.LogInformation("Validando unicidade do CPF: {CPF}", cpf);

                var alunoExistente = await _alunoRepository.GetByCPFAsync(cpf);

                if (alunoExistente == null)
                {
                    return true; // CPF não existe, é único
                }

                // Se um ID foi fornecido, verifica se o CPF pertence ao mesmo aluno
                if (alunoId.HasValue && alunoExistente.Id == alunoId.Value)
                {
                    return true; // CPF pertence ao mesmo aluno, é válido
                }

                return false; // CPF já existe para outro aluno
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar unicidade do CPF: {CPF}", cpf);
                throw;
            }
        }

        private static AlunoResponseDTO MapToResponseDto(Aluno aluno)
        {
            return new AlunoResponseDTO
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                Email = aluno.Email,
                Telefone = aluno.Telefone,
                CPF = aluno.CPF,
                DataNascimento = aluno.DataNascimento,
                Endereco = aluno.Endereco,
                Cidade = aluno.Cidade,
                Estado = aluno.Estado,
                CEP = aluno.CEP,
                Status = aluno.Status,
                DataCadastro = aluno.DataCadastro,
                Observacoes = aluno.Observacoes
            };
        }
    }
}