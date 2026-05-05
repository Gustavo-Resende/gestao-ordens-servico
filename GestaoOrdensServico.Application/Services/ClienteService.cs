using GestaoOrdensServico.Infrastructure.Logging;
using GestaoOrdensServico.Infrastructure.Repositories;

namespace GestaoOrdensServico.Application.Services
{
    public class ClienteService
    {
        private readonly ClienteRepository _clienteRepository;
        private readonly Logger _logger;

        public ClienteService(ClienteRepository clienteRepository, Logger logger)
        {
            _clienteRepository = clienteRepository;
            _logger = logger;
        }
    }
}
