using System.Text.Json;

namespace AlunosService.External
{
    public class TreinosServiceClient : ITreinosServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TreinosServiceClient> _logger;

        public TreinosServiceClient(HttpClient httpClient, ILogger<TreinosServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<TreinoExternalDTO?> GetTreinoAsync(int treinoId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/treinos/{treinoId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TreinoExternalDTO>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar treino {treinoId}");
                return null;
            }
        }

        public async Task<IEnumerable<TreinoExternalDTO>> GetTreinosAtivosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/treinos/ativos");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<IEnumerable<TreinoExternalDTO>>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
                        new List<TreinoExternalDTO>();
                }
                return new List<TreinoExternalDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar treinos ativos");
                return new List<TreinoExternalDTO>();
            }
        }
    }
}
