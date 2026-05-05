using GestaoOrdensServico.Infrastructure.Logging;
using GestaoOrdensServico.Infrastructure.Repositories;

namespace GestaoOrdensServico.Application.Services
{
    public class RelatorioService
    {
        private readonly OrdemServicoRepository _ordemServicoRepository;
        private readonly Logger _logger;

        public RelatorioService(OrdemServicoRepository ordemServicoRepository, Logger logger)
        {
            _ordemServicoRepository = ordemServicoRepository;
            _logger = logger;
        }
    }
}
