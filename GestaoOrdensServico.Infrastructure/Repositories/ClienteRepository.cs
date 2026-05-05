using GestaoOrdensServico.Infrastructure.Connection;

namespace GestaoOrdensServico.Infrastructure.Repositories
{
    public class ClienteRepository
    {
        private readonly DbConnectionFactory _factory;

        public ClienteRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }
    }
}
