using System;
using System.Collections.Generic;
using GestaoOrdensServico.Domain.Entities;
using GestaoOrdensServico.Infrastructure.Connection;
using Npgsql;

namespace GestaoOrdensServico.Infrastructure.Repositories
{
    public class ServicoRepository
    {
        private readonly DbConnectionFactory _factory;

        public ServicoRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        public void Inserir(Servico servico)
        {
            const string sql = @"
                INSERT INTO servicos (id, nome, valor_base, percentual_imposto, ativo)
                VALUES (@id, @nome, @valorBase, @percentualImposto, @ativo)";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    AdicionarParametros(command, servico);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Atualizar(Servico servico)
        {
            const string sql = @"
                UPDATE servicos
                SET nome = @nome, valor_base = @valorBase,
                    percentual_imposto = @percentualImposto, ativo = @ativo
                WHERE id = @id";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id",                servico.Id);
                    command.Parameters.AddWithValue("@nome",              servico.Nome);
                    command.Parameters.AddWithValue("@valorBase",         servico.ValorBase);
                    command.Parameters.AddWithValue("@percentualImposto", servico.PercentualImposto);
                    command.Parameters.AddWithValue("@ativo",             servico.Ativo);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Servico BuscarPorId(Guid id)
        {
            const string sql = @"
                SELECT id, nome, valor_base, percentual_imposto, ativo
                FROM servicos
                WHERE id = @id";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return MapearServico(reader);
                        return null;
                    }
                }
            }
        }

        public List<Servico> Listar(bool? ativo = null)
        {
            var sql = "SELECT id, nome, valor_base, percentual_imposto, ativo FROM servicos"
                    + (ativo.HasValue ? " WHERE ativo = @ativo" : "")
                    + " ORDER BY nome";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    if (ativo.HasValue)
                        command.Parameters.AddWithValue("@ativo", ativo.Value);

                    var lista = new List<Servico>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            lista.Add(MapearServico(reader));
                    }
                    return lista;
                }
            }
        }

        private static void AdicionarParametros(NpgsqlCommand command, Servico servico)
        {
            command.Parameters.AddWithValue("@id",                servico.Id);
            command.Parameters.AddWithValue("@nome",              servico.Nome);
            command.Parameters.AddWithValue("@valorBase",         servico.ValorBase);
            command.Parameters.AddWithValue("@percentualImposto", servico.PercentualImposto);
            command.Parameters.AddWithValue("@ativo",             servico.Ativo);
        }

        private static Servico MapearServico(NpgsqlDataReader reader)
        {
            return new Servico
            {
                Id                = reader.GetGuid(reader.GetOrdinal("id")),
                Nome              = reader.GetString(reader.GetOrdinal("nome")),
                ValorBase         = reader.GetDecimal(reader.GetOrdinal("valor_base")),
                PercentualImposto = reader.GetDecimal(reader.GetOrdinal("percentual_imposto")),
                Ativo             = reader.GetBoolean(reader.GetOrdinal("ativo"))
            };
        }
    }
}
