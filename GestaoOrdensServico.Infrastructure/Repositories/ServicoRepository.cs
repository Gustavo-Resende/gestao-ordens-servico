using GestaoOrdensServico.Infrastructure.Connection;

namespace GestaoOrdensServico.Infrastructure.Repositories
{
    public class ServicoRepository
    {
        private readonly DbConnectionFactory _factory;

        public ServicoRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }
    }
}
