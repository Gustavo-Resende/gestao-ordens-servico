using System;
using System.Collections.Generic;
using GestaoOrdensServico.Domain.Entities;
using GestaoOrdensServico.Domain.Enums;
using GestaoOrdensServico.Infrastructure.Connection;
using Npgsql;

namespace GestaoOrdensServico.Infrastructure.Repositories
{
    public class OrdemServicoRepository
    {
        private readonly DbConnectionFactory _factory;

        public OrdemServicoRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        // ─── Ordem de Serviço ────────────────────────────────────────────────

        public void Inserir(OrdemServico os, string snapshotAuditoria)
        {
            const string sql = @"
                INSERT INTO ordens_servico
                    (id, cliente_id, data_abertura, status, observacao, valor_total, versao)
                VALUES
                    (@id, @clienteId, @dataAbertura, @status::status_os, @observacao, @valorTotal, @versao)";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand(sql, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id",           os.Id);
                            command.Parameters.AddWithValue("@clienteId",    os.ClienteId);
                            command.Parameters.AddWithValue("@dataAbertura", os.DataAbertura);
                            command.Parameters.AddWithValue("@status",       os.Status.ToString());
                            command.Parameters.AddWithValue("@observacao",   (object)os.Observacao ?? DBNull.Value);
                            command.Parameters.AddWithValue("@valorTotal",   os.ValorTotal);
                            command.Parameters.AddWithValue("@versao",       os.Versao);
                            command.ExecuteNonQuery();
                        }

                        InserirAuditoriaInterna("OrdemServico", os.Id, "INSERT", snapshotAuditoria, connection, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Atualizar(OrdemServico os)
        {
            const string sql = @"
                UPDATE ordens_servico
                SET observacao = @observacao, versao = versao + 1
                WHERE id = @id AND versao = @versao";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id",         os.Id);
                    command.Parameters.AddWithValue("@observacao", (object)os.Observacao ?? DBNull.Value);
                    command.Parameters.AddWithValue("@versao",     os.Versao);

                    int linhasAfetadas = command.ExecuteNonQuery();
                    if (linhasAfetadas == 0)
                        throw new Exception("A OS foi alterada por outro usuário. Recarregue e tente novamente.");
                }
            }
        }

        public OrdemServico BuscarPorId(Guid id)
        {
            const string sql = @"
                SELECT os.id, os.cliente_id, c.nome AS cliente_nome,
                       os.data_abertura, os.data_conclusao, os.status,
                       os.observacao, os.valor_total, os.versao
                FROM ordens_servico os
                INNER JOIN clientes c ON c.id = os.cliente_id
                WHERE os.id = @id";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return MapearOrdemServico(reader);
                        return null;
                    }
                }
            }
        }

        public List<OrdemServico> ListarPaginado(DateTime? inicio, DateTime? fim, Guid? clienteId, string status,
            int pagina, int tamanhoPagina, out int total)
        {
            var conditions = new List<string>();
            if (inicio.HasValue)              conditions.Add("os.data_abertura >= @inicio");
            if (fim.HasValue)                 conditions.Add("os.data_abertura <= @fim");
            if (clienteId.HasValue)           conditions.Add("os.cliente_id = @clienteId");
            if (!string.IsNullOrEmpty(status)) conditions.Add("os.status = @status::status_os");

            var where = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : "";

            var sqlCount = $@"
                SELECT COUNT(1)
                FROM ordens_servico os
                INNER JOIN clientes c ON c.id = os.cliente_id
                {where}";

            var sqlSelect = $@"
                SELECT os.id, os.cliente_id, c.nome AS cliente_nome,
                       os.data_abertura, os.data_conclusao, os.status,
                       os.observacao, os.valor_total, os.versao
                FROM ordens_servico os
                INNER JOIN clientes c ON c.id = os.cliente_id
                {where}
                ORDER BY os.data_abertura DESC
                LIMIT @limite OFFSET @offset";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();

                using (var cmdCount = new NpgsqlCommand(sqlCount, connection))
                {
                    AdicionarFiltrosListagem(cmdCount, inicio, fim, clienteId, status);
                    total = (int)(long)cmdCount.ExecuteScalar();
                }

                using (var cmdSelect = new NpgsqlCommand(sqlSelect, connection))
                {
                    AdicionarFiltrosListagem(cmdSelect, inicio, fim, clienteId, status);
                    cmdSelect.Parameters.AddWithValue("@limite",  tamanhoPagina);
                    cmdSelect.Parameters.AddWithValue("@offset",  (pagina - 1) * tamanhoPagina);

                    var lista = new List<OrdemServico>();
                    using (var reader = cmdSelect.ExecuteReader())
                    {
                        while (reader.Read())
                            lista.Add(MapearOrdemServico(reader));
                    }
                    return lista;
                }
            }
        }

        public void AlterarStatus(Guid osId, StatusOs novoStatus, int versaoOs,
                                  DateTime? dataConclusao, string snapshotAuditoria)
        {
            const string sql = @"
                UPDATE ordens_servico
                SET status         = @novoStatus::status_os,
                    data_conclusao = @dataConclusao,
                    versao         = versao + 1
                WHERE id = @id AND versao = @versaoAtual";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand(sql, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id",            osId);
                            command.Parameters.AddWithValue("@novoStatus",    novoStatus.ToString());
                            command.Parameters.AddWithValue("@dataConclusao", (object)dataConclusao ?? DBNull.Value);
                            command.Parameters.AddWithValue("@versaoAtual",   versaoOs);

                            int linhasAfetadas = command.ExecuteNonQuery();
                            if (linhasAfetadas == 0)
                                throw new Exception("A OS foi alterada por outro usuário. Recarregue e tente novamente.");
                        }

                        InserirAuditoriaInterna("OrdemServico", osId, "UPDATE", snapshotAuditoria, connection, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // ─── Itens ───────────────────────────────────────────────────────────

        public List<ItemOs> BuscarItens(Guid ordemServicoId)
        {
            const string sql = @"
                SELECT i.id, i.ordem_servico_id, i.servico_id, s.nome AS servico_nome,
                       i.quantidade, i.valor_unitario,
                       i.percentual_imposto_aplicado, i.valor_total_item
                FROM itens_os i
                INNER JOIN servicos s ON s.id = i.servico_id
                WHERE i.ordem_servico_id = @ordemServicoId";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ordemServicoId", ordemServicoId);
                    var lista = new List<ItemOs>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            lista.Add(MapearItemOs(reader));
                    }
                    return lista;
                }
            }
        }

        public void InserirItem(ItemOs item, int versaoOs, string snapshotAuditoria)
        {
            const string sqlItem = @"
                INSERT INTO itens_os
                    (id, ordem_servico_id, servico_id, quantidade,
                     valor_unitario, percentual_imposto_aplicado, valor_total_item)
                VALUES
                    (@id, @ordemServicoId, @servicoId, @quantidade,
                     @valorUnitario, @percentualImpostoAplicado, @valorTotalItem)";

            const string sqlAtualizaTotal = @"
                UPDATE ordens_servico
                SET valor_total = (
                        SELECT COALESCE(SUM(valor_total_item), 0)
                        FROM itens_os
                        WHERE ordem_servico_id = @osId
                    ),
                    versao = versao + 1
                WHERE id = @osId AND versao = @versaoAtual";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand(sqlItem, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id",                        item.Id);
                            command.Parameters.AddWithValue("@ordemServicoId",            item.OrdemServicoId);
                            command.Parameters.AddWithValue("@servicoId",                 item.ServicoId);
                            command.Parameters.AddWithValue("@quantidade",                item.Quantidade);
                            command.Parameters.AddWithValue("@valorUnitario",             item.ValorUnitario);
                            command.Parameters.AddWithValue("@percentualImpostoAplicado", item.PercentualImpostoAplicado);
                            command.Parameters.AddWithValue("@valorTotalItem",            item.ValorTotalItem);
                            command.ExecuteNonQuery();
                        }

                        using (var command = new NpgsqlCommand(sqlAtualizaTotal, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@osId",        item.OrdemServicoId);
                            command.Parameters.AddWithValue("@versaoAtual", versaoOs);

                            int linhasAfetadas = command.ExecuteNonQuery();
                            if (linhasAfetadas == 0)
                                throw new Exception("A OS foi alterada por outro usuário. Recarregue e tente novamente.");
                        }

                        InserirAuditoriaInterna("ItemOs", item.Id, "INSERT", snapshotAuditoria, connection, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void RemoverItem(Guid itemId, Guid ordemServicoId, int versaoOs, string snapshotAuditoria)
        {
            const string sqlDelete = "DELETE FROM itens_os WHERE id = @id";

            const string sqlAtualizaTotal = @"
                UPDATE ordens_servico
                SET valor_total = (
                        SELECT COALESCE(SUM(valor_total_item), 0)
                        FROM itens_os
                        WHERE ordem_servico_id = @osId
                    ),
                    versao = versao + 1
                WHERE id = @osId AND versao = @versaoAtual";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand(sqlDelete, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id", itemId);
                            command.ExecuteNonQuery();
                        }

                        using (var command = new NpgsqlCommand(sqlAtualizaTotal, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@osId",        ordemServicoId);
                            command.Parameters.AddWithValue("@versaoAtual", versaoOs);

                            int linhasAfetadas = command.ExecuteNonQuery();
                            if (linhasAfetadas == 0)
                                throw new Exception("A OS foi alterada por outro usuário. Recarregue e tente novamente.");
                        }

                        InserirAuditoriaInterna("ItemOs", itemId, "DELETE", snapshotAuditoria, connection, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // ─── Relatório ───────────────────────────────────────────────────────

        public List<OrdemServico> ListarParaRelatorio(
            DateTime inicio, DateTime fim, Guid? clienteId, string status)
        {
            var conditions = new List<string>();
            conditions.Add("os.data_abertura >= @inicio");
            conditions.Add("os.data_abertura <= @fim");
            if (clienteId.HasValue)            conditions.Add("os.cliente_id = @clienteId");
            if (!string.IsNullOrEmpty(status)) conditions.Add("os.status = @status::status_os");

            var where = "WHERE " + string.Join(" AND ", conditions);
            var sql   = @"
                SELECT os.id, os.cliente_id, c.nome AS cliente_nome,
                       os.data_abertura, os.data_conclusao, os.status,
                       os.observacao, os.valor_total, os.versao
                FROM ordens_servico os
                INNER JOIN clientes c ON c.id = os.cliente_id "
                + where
                + " ORDER BY c.nome, os.data_abertura";

            using (var connection = _factory.CreateConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@inicio", inicio);
                    command.Parameters.AddWithValue("@fim",    fim);
                    if (clienteId.HasValue)            command.Parameters.AddWithValue("@clienteId", clienteId.Value);
                    if (!string.IsNullOrEmpty(status)) command.Parameters.AddWithValue("@status",    status);

                    var lista = new List<OrdemServico>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            lista.Add(MapearOrdemServico(reader));
                    }
                    return lista;
                }
            }
        }

        // ─── Helpers privados ────────────────────────────────────────────────

        private static void AdicionarFiltrosListagem(
            NpgsqlCommand command, DateTime? inicio, DateTime? fim,
            Guid? clienteId, string status)
        {
            if (inicio.HasValue)              command.Parameters.AddWithValue("@inicio",    inicio.Value);
            if (fim.HasValue)                 command.Parameters.AddWithValue("@fim",       fim.Value);
            if (clienteId.HasValue)           command.Parameters.AddWithValue("@clienteId", clienteId.Value);
            if (!string.IsNullOrEmpty(status)) command.Parameters.AddWithValue("@status",   status);
        }

        private static void InserirAuditoriaInterna(
            string entidade, Guid idRegistro, string operacao, string snapshot,
            NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            const string sql = @"
                INSERT INTO auditoria (id, entidade, id_registro, operacao, data_hora, usuario, snapshot)
                VALUES (@id, @entidade, @idRegistro, @operacao, @dataHora, @usuario, @snapshot)";

            using (var command = new NpgsqlCommand(sql, connection, transaction))
            {
                command.Parameters.AddWithValue("@id",          Guid.NewGuid());
                command.Parameters.AddWithValue("@entidade",    entidade);
                command.Parameters.AddWithValue("@idRegistro",  idRegistro);
                command.Parameters.AddWithValue("@operacao",    operacao);
                command.Parameters.AddWithValue("@dataHora",    DateTime.Now);
                command.Parameters.AddWithValue("@usuario",     "Sistema");
                command.Parameters.AddWithValue("@snapshot",    snapshot);
                command.ExecuteNonQuery();
            }
        }

        private static OrdemServico MapearOrdemServico(NpgsqlDataReader reader)
        {
            return new OrdemServico
            {
                Id            = reader.GetGuid(reader.GetOrdinal("id")),
                ClienteId     = reader.GetGuid(reader.GetOrdinal("cliente_id")),
                ClienteNome   = reader.GetString(reader.GetOrdinal("cliente_nome")),
                DataAbertura  = reader.GetDateTime(reader.GetOrdinal("data_abertura")),
                DataConclusao = reader.IsDBNull(reader.GetOrdinal("data_conclusao"))
                                    ? (DateTime?)null
                                    : reader.GetDateTime(reader.GetOrdinal("data_conclusao")),
                Status        = (StatusOs)Enum.Parse(typeof(StatusOs), reader.GetString(reader.GetOrdinal("status"))),
                Observacao    = reader.IsDBNull(reader.GetOrdinal("observacao"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("observacao")),
                ValorTotal    = reader.GetDecimal(reader.GetOrdinal("valor_total")),
                Versao        = reader.GetInt32(reader.GetOrdinal("versao"))
            };
        }

        private static ItemOs MapearItemOs(NpgsqlDataReader reader)
        {
            return new ItemOs
            {
                Id                        = reader.GetGuid(reader.GetOrdinal("id")),
                OrdemServicoId            = reader.GetGuid(reader.GetOrdinal("ordem_servico_id")),
                ServicoId                 = reader.GetGuid(reader.GetOrdinal("servico_id")),
                ServicoNome               = reader.GetString(reader.GetOrdinal("servico_nome")),
                Quantidade                = reader.GetInt32(reader.GetOrdinal("quantidade")),
                ValorUnitario             = reader.GetDecimal(reader.GetOrdinal("valor_unitario")),
                PercentualImpostoAplicado = reader.GetDecimal(reader.GetOrdinal("percentual_imposto_aplicado")),
                ValorTotalItem            = reader.GetDecimal(reader.GetOrdinal("valor_total_item"))
            };
        }
    }
}
