using System;
using System.Collections.Generic;
using GestaoOrdensServico.Domain.Entities;
using GestaoOrdensServico.Infrastructure.Connection;
using Npgsql;

namespace GestaoOrdensServico.Infrastructure.Repositories
{
    public class ClienteRepository
    {
        private readonly DbConnectionFactory _factory;

        public ClienteRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        public void Inserir(Cliente cliente)
        {
            const string sql = @"
                INSERT INTO clientes (id, nome, documento, tipo, email, telefone, data_cadastro, ativo)
                VALUES (@id, @nome, @documento, @tipo, @email, @telefone, @dataCadastro, @ativo)";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    AdicionarParametros(command, cliente);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Atualizar(Cliente cliente)
        {
            const string sql = @"
                UPDATE clientes
                SET nome = @nome, documento = @documento, tipo = @tipo,
                    email = @email, telefone = @telefone, ativo = @ativo
                WHERE id = @id";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id",       cliente.Id);
                    command.Parameters.AddWithValue("@nome",     cliente.Nome);
                    command.Parameters.AddWithValue("@documento", cliente.Documento);
                    command.Parameters.AddWithValue("@tipo",     cliente.Tipo);
                    command.Parameters.AddWithValue("@email",    (object)cliente.Email    ?? DBNull.Value);
                    command.Parameters.AddWithValue("@telefone", (object)cliente.Telefone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ativo",    cliente.Ativo);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Excluir(Guid id)
        {
            const string sql = "DELETE FROM clientes WHERE id = @id";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Cliente BuscarPorId(Guid id)
        {
            const string sql = @"
                SELECT id, nome, documento, tipo, email, telefone, data_cadastro, ativo
                FROM clientes
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
                            return MapearCliente(reader);
                        return null;
                    }
                }
            }
        }

        public List<Cliente> Listar(string nome = null, string documento = null, bool? ativo = null)
        {
            var conditions = new List<string>();
            if (nome      != null)  conditions.Add("nome ILIKE @nome");
            if (documento != null)  conditions.Add("documento = @documento");
            if (ativo.HasValue)     conditions.Add("ativo = @ativo");

            var where = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : "";
            var sql   = "SELECT id, nome, documento, tipo, email, telefone, data_cadastro, ativo "
                      + "FROM clientes " + where + " ORDER BY nome";

            System.Diagnostics.Debug.WriteLine($"[ClienteRepository.Listar] SQL: {sql}");
            System.Diagnostics.Debug.WriteLine($"[ClienteRepository.Listar] Params — nome={nome}, documento={documento}, ativo={ativo?.ToString() ?? "null"}");

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    if (nome      != null)  command.Parameters.AddWithValue("@nome",      "%" + nome + "%");
                    if (documento != null)  command.Parameters.AddWithValue("@documento", documento);
                    if (ativo.HasValue)     command.Parameters.AddWithValue("@ativo",     ativo.Value);

                    var lista = new List<Cliente>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            lista.Add(MapearCliente(reader));
                    }

                    System.Diagnostics.Debug.WriteLine($"[ClienteRepository.Listar] Linhas retornadas: {lista.Count}");
                    return lista;
                }
            }
        }

        public bool PossuiOsVinculada(Guid clienteId)
        {
            const string sql = "SELECT COUNT(1) FROM ordens_servico WHERE cliente_id = @clienteId";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@clienteId", clienteId);
                    return (long)command.ExecuteScalar() > 0;
                }
            }
        }

        private static void AdicionarParametros(NpgsqlCommand command, Cliente cliente)
        {
            command.Parameters.AddWithValue("@id",           cliente.Id);
            command.Parameters.AddWithValue("@nome",         cliente.Nome);
            command.Parameters.AddWithValue("@documento",    cliente.Documento);
            command.Parameters.AddWithValue("@tipo",         cliente.Tipo);
            command.Parameters.AddWithValue("@email",        (object)cliente.Email    ?? DBNull.Value);
            command.Parameters.AddWithValue("@telefone",     (object)cliente.Telefone ?? DBNull.Value);
            command.Parameters.AddWithValue("@dataCadastro", cliente.DataCadastro);
            command.Parameters.AddWithValue("@ativo",        cliente.Ativo);
        }

        private static Cliente MapearCliente(NpgsqlDataReader reader)
        {
            return new Cliente
            {
                Id           = reader.GetGuid(reader.GetOrdinal("id")),
                Nome         = reader.GetString(reader.GetOrdinal("nome")),
                Documento    = reader.GetString(reader.GetOrdinal("documento")),
                Tipo         = reader.GetString(reader.GetOrdinal("tipo")),
                Email        = reader.IsDBNull(reader.GetOrdinal("email"))
                                   ? null : reader.GetString(reader.GetOrdinal("email")),
                Telefone     = reader.IsDBNull(reader.GetOrdinal("telefone"))
                                   ? null : reader.GetString(reader.GetOrdinal("telefone")),
                DataCadastro = reader.GetDateTime(reader.GetOrdinal("data_cadastro")),
                Ativo        = reader.GetBoolean(reader.GetOrdinal("ativo"))
            };
        }
    }
}
