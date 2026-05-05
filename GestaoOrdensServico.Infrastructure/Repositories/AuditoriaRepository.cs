using System;
using GestaoOrdensServico.Domain.Auditoria;
using GestaoOrdensServico.Infrastructure.Connection;
using Npgsql;

namespace GestaoOrdensServico.Infrastructure.Repositories
{
    public class AuditoriaRepository
    {
        private readonly DbConnectionFactory _factory;

        public AuditoriaRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        public void Inserir(AuditoriaRegistro registro)
        {
            const string sql = @"
                INSERT INTO auditoria (id, entidade, id_registro, operacao, data_hora, usuario, snapshot)
                VALUES (@id, @entidade, @idRegistro, @operacao, @dataHora, @usuario, @snapshot)";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    AdicionarParametros(command, registro);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void AdicionarParametros(NpgsqlCommand command, AuditoriaRegistro registro)
        {
            command.Parameters.AddWithValue("@id",          registro.Id == Guid.Empty ? Guid.NewGuid() : registro.Id);
            command.Parameters.AddWithValue("@entidade",    registro.Entidade);
            command.Parameters.AddWithValue("@idRegistro",  registro.IdRegistro);
            command.Parameters.AddWithValue("@operacao",    registro.Operacao);
            command.Parameters.AddWithValue("@dataHora",    registro.DataHora == default(DateTime) ? DateTime.Now : registro.DataHora);
            command.Parameters.AddWithValue("@usuario",     registro.Usuario ?? "Sistema");
            command.Parameters.AddWithValue("@snapshot",    registro.Snapshot);
        }
    }
}
