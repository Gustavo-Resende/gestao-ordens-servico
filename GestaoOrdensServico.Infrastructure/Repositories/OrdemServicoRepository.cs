using GestaoOrdensServico.Infrastructure.Connection;

namespace GestaoOrdensServico.Infrastructure.Repositories
{
    public class OrdemServicoRepository
    {
        private readonly DbConnectionFactory _factory;

        public OrdemServicoRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }
    }
}
