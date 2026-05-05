using GestaoOrdensServico.Infrastructure.Connection;

namespace GestaoOrdensServico.Infrastructure.Repositories
{
    public class AuditoriaRepository
    {
        private readonly DbConnectionFactory _factory;

        public AuditoriaRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }
    }
}
