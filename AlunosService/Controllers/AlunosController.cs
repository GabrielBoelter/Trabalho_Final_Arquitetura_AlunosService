using AlunosService.DTOs;
using AlunosService.Models;
using AlunosService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlunosService.Controllers
{
    [ApiController]
    [Route("api/alunos")]
    [Produces("application/json")]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoService _service;
        private readonly ILogger<AlunosController> _logger;

        public AlunosController(IAlunoService service, ILogger<AlunosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Cria um novo aluno
        /// </summary>
        /// <param name="dto">Dados do aluno a ser criado</param>
        /// <returns>O aluno criado</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Criar([FromBody] AlunoCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var aluno = await _service.CreateAlunoAsync(dto);

                if (aluno == null)
                {
                    _logger.LogWarning("Tentativa de criar aluno falhou - Email ou CPF podem j� existir");
                    return BadRequest("N�o foi poss�vel criar o aluno. Email ou CPF podem j� existir.");
                }

                _logger.LogInformation("Aluno criado com sucesso. ID: {AlunoId}", aluno.Id);
                return CreatedAtAction(nameof(ObterPorId), new { id = aluno.Id }, aluno);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar aluno");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obt�m todos os alunos
        /// </summary>
        /// <returns>Lista de alunos</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var alunos = await _service.GetAllAlunosAsync();
                _logger.LogInformation("Retornados {Count} alunos", alunos?.Count() ?? 0);
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os alunos");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obt�m um aluno por ID
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>O aluno encontrado</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("ID deve ser maior que zero");
                }

                var aluno = await _service.GetAlunoByIdAsync(id);

                if (aluno == null)
                {
                    _logger.LogWarning("Aluno com ID {AlunoId} n�o encontrado", id);
                    return NotFound($"Aluno com ID {id} n�o encontrado");
                }

                return Ok(aluno);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter aluno por ID {AlunoId}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Atualiza um aluno existente
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <param name="dto">Dados para atualiza��o</param>
        /// <returns>O aluno atualizado</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AlunoUpdateDTO dto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("ID deve ser maior que zero");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var aluno = await _service.UpdateAlunoAsync(id, dto);

                if (aluno == null)
                {
                    _logger.LogWarning("Falha ao atualizar aluno ID {AlunoId} - n�o encontrado ou dados inv�lidos", id);
                    return NotFound($"Aluno com ID {id} n�o encontrado ou dados inv�lidos");
                }

                _logger.LogInformation("Aluno ID {AlunoId} atualizado com sucesso", id);
                return Ok(aluno);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar aluno ID {AlunoId}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Deleta um aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>NoContent se sucesso</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("ID deve ser maior que zero");
                }

                var sucesso = await _service.DeleteAlunoAsync(id);

                if (!sucesso)
                {
                    _logger.LogWarning("Falha ao deletar aluno ID {AlunoId} - n�o encontrado", id);
                    return NotFound($"Aluno com ID {id} n�o encontrado ou n�o pode ser deletado");
                }

                _logger.LogInformation("Aluno ID {AlunoId} deletado com sucesso", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar aluno ID {AlunoId}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Busca alunos por termo
        /// </summary>
        /// <param name="termo">Termo de busca</param>
        /// <returns>Lista de alunos encontrados</returns>
        [HttpGet("buscar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Buscar([FromQuery] string termo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(termo))
                {
                    return BadRequest("Termo de busca � obrigat�rio");
                }

                if (termo.Length < 2)
                {
                    return BadRequest("Termo de busca deve ter pelo menos 2 caracteres");
                }

                var alunos = await _service.SearchAlunosAsync(termo);
                _logger.LogInformation("Busca por '{Termo}' retornou {Count} resultados", termo, alunos?.Count() ?? 0);
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar alunos com termo '{Termo}'", termo);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obt�m alunos por status
        /// </summary>
        /// <param name="status">Status do aluno</param>
        /// <returns>Lista de alunos com o status especificado</returns>
        [HttpGet("status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterPorStatus(StatusAluno status)
        {
            try
            {
                if (!Enum.IsDefined(typeof(StatusAluno), status))
                {
                    return BadRequest("Status inv�lido");
                }

                var alunos = await _service.GetAlunosByStatusAsync(status);
                _logger.LogInformation("Retornados {Count} alunos com status {Status}", alunos?.Count() ?? 0, status);
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter alunos por status {Status}", status);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obt�m aluno por email
        /// </summary>
        /// <param name="email">Email do aluno</param>
        /// <returns>O aluno encontrado</returns>
        [HttpGet("email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterPorEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest("Email � obrigat�rio");
                }

                // Valida��o b�sica de email
                if (!email.Contains('@') || !email.Contains('.'))
                {
                    return BadRequest("Formato de email inv�lido");
                }

                var aluno = await _service.GetAlunoByEmailAsync(email);

                if (aluno == null)
                {
                    _logger.LogWarning("Aluno com email {Email} n�o encontrado", email);
                    return NotFound($"Aluno com email {email} n�o encontrado");
                }

                return Ok(aluno);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter aluno por email {Email}", email);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obt�m aluno por CPF
        /// </summary>
        /// <param name="cpf">CPF do aluno</param>
        /// <returns>O aluno encontrado</returns>
        [HttpGet("cpf/{cpf}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterPorCPF(string cpf)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cpf))
                {
                    return BadRequest("CPF � obrigat�rio");
                }

                // Remove formata��o do CPF para valida��o
                var cpfLimpo = cpf.Replace(".", "").Replace("-", "");
                if (cpfLimpo.Length != 11 || !cpfLimpo.All(char.IsDigit))
                {
                    return BadRequest("CPF deve conter 11 d�gitos num�ricos");
                }

                var aluno = await _service.GetAlunoByCPFAsync(cpf);

                if (aluno == null)
                {
                    _logger.LogWarning("Aluno com CPF {CPF} n�o encontrado", cpf);
                    return NotFound($"Aluno com CPF {cpf} n�o encontrado");
                }

                return Ok(aluno);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter aluno por CPF {CPF}", cpf);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obt�m o total de alunos cadastrados
        /// </summary>
        /// <returns>Objeto com o total de alunos</returns>
        [HttpGet("total")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterTotal()
        {
            try
            {
                var total = await _service.GetAllAlunosCountAsync();
                _logger.LogInformation("Total de alunos: {Total}", total);
                return Ok(new { Total = total });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter total de alunos");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Verifica se um aluno existe
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>200 se existe, 404 se n�o existe</returns>
        [HttpHead("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerificarExistencia(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                var existe = await _service.AlunoExisteAsync(id);
                return existe ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar exist�ncia do aluno ID {AlunoId}", id);
                return StatusCode(500);
            }
        }
    }
}