using System.Text.Json;
using AlunosService.DTOs;

namespace AlunosService.External
{
    public class PagamentosServiceClient : IPagamentosServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PagamentosServiceClient> _logger;

        public PagamentosServiceClient(HttpClient httpClient, ILogger<PagamentosServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<PagamentoExternalDTO>> GetPagamentosByAlunoAsync(int alunoId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/pagamentos/aluno/{alunoId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<IEnumerable<PagamentoExternalDTO>>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
                        new List<PagamentoExternalDTO>();
                }
                return new List<PagamentoExternalDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar pagamentos do aluno {alunoId}");
                return new List<PagamentoExternalDTO>();
            }
        }

        public async Task<IEnumerable<AcessoTreinoExternalDTO>> GetAcessosAtivosByAlunoAsync(int alunoId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/acessos/aluno/{alunoId}/ativos");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<IEnumerable<AcessoTreinoExternalDTO>>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
                        new List<AcessoTreinoExternalDTO>();
                }
                return new List<AcessoTreinoExternalDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar acessos ativos do aluno {alunoId}");
                return new List<AcessoTreinoExternalDTO>();
            }
        }

        public async Task<bool> AlunoTemPagamentosAsync(int alunoId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/pagamentos/aluno/{alunoId}/existe");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao verificar se aluno {alunoId} tem pagamentos");
                return false;
            }
        }
    }
}
