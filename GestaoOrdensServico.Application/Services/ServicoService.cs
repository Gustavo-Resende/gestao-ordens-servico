using GestaoOrdensServico.Infrastructure.Logging;
using GestaoOrdensServico.Infrastructure.Repositories;

namespace GestaoOrdensServico.Application.Services
{
    public class ServicoService
    {
        private readonly ServicoRepository _servicoRepository;
        private readonly Logger _logger;

        public ServicoService(ServicoRepository servicoRepository, Logger logger)
        {
            _servicoRepository = servicoRepository;
            _logger = logger;
        }
    }
}
