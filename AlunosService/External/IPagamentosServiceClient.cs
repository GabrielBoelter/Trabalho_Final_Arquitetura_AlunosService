using AlunosService.DTOs;

namespace AlunosService.External
{
    public interface IPagamentosServiceClient
    {
        Task<IEnumerable<PagamentoExternalDTO>> GetPagamentosByAlunoAsync(int alunoId);
        Task<IEnumerable<AcessoTreinoExternalDTO>> GetAcessosAtivosByAlunoAsync(int alunoId);
        Task<bool> AlunoTemPagamentosAsync(int alunoId);
    }
}