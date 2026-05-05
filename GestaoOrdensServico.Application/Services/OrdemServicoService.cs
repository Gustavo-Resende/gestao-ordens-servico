using GestaoOrdensServico.Infrastructure.Logging;
using GestaoOrdensServico.Infrastructure.Repositories;

namespace GestaoOrdensServico.Application.Services
{
    public class OrdemServicoService
    {
        private readonly OrdemServicoRepository _ordemServicoRepository;
        private readonly ClienteRepository _clienteRepository;
        private readonly ServicoRepository _servicoRepository;
        private readonly AuditoriaRepository _auditoriaRepository;
        private readonly Logger _logger;

        public OrdemServicoService(
            OrdemServicoRepository ordemServicoRepository,
            ClienteRepository clienteRepository,
            ServicoRepository servicoRepository,
            AuditoriaRepository auditoriaRepository,
            Logger logger)
        {
            _ordemServicoRepository = ordemServicoRepository;
            _clienteRepository = clienteRepository;
            _servicoRepository = servicoRepository;
            _auditoriaRepository = auditoriaRepository;
            _logger = logger;
        }
    }
}
