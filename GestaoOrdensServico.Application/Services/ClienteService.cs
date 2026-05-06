using System;
using System.Collections.Generic;
using GestaoOrdensServico.Domain.Entities;
using GestaoOrdensServico.Infrastructure.Logging;
using GestaoOrdensServico.Infrastructure.Repositories;
using Newtonsoft.Json;
using Npgsql;

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

        public ResultadoOperacao<Cliente> Criar(string nome, string documento, string tipo,
            string email, string telefone)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return ResultadoOperacao<Cliente>.Falha("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(documento))
                return ResultadoOperacao<Cliente>.Falha("Documento é obrigatório.");

            if (tipo != "Fisica" && tipo != "Juridica")
                return ResultadoOperacao<Cliente>.Falha("Tipo deve ser 'Fisica' ou 'Juridica'.");

            var cliente = new Cliente
            {
                Id           = Guid.NewGuid(),
                Nome         = nome.Trim(),
                Documento    = documento.Trim(),
                Tipo         = tipo,
                Email        = string.IsNullOrWhiteSpace(email)    ? null : email.Trim(),
                Telefone     = string.IsNullOrWhiteSpace(telefone) ? null : telefone.Trim(),
                DataCadastro = DateTime.Now,
                Ativo        = true
            };

            try
            {
                _clienteRepository.Inserir(cliente);
                _logger.LogInfo($"Cliente criado: {cliente.Id} — {cliente.Nome}");
                return ResultadoOperacao<Cliente>.Sucesso(cliente);
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                return ResultadoOperacao<Cliente>.Falha("Já existe um cliente com este documento.");
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao criar cliente.", ex);
                return ResultadoOperacao<Cliente>.Falha("Erro inesperado ao criar cliente.");
            }
        }

        public ResultadoOperacao<Cliente> Atualizar(Guid id, string nome, string documento,
            string tipo, string email, string telefone, bool ativo)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return ResultadoOperacao<Cliente>.Falha("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(documento))
                return ResultadoOperacao<Cliente>.Falha("Documento é obrigatório.");

            if (tipo != "Fisica" && tipo != "Juridica")
                return ResultadoOperacao<Cliente>.Falha("Tipo deve ser 'Fisica' ou 'Juridica'.");

            var cliente = new Cliente
            {
                Id       = id,
                Nome     = nome.Trim(),
                Documento = documento.Trim(),
                Tipo     = tipo,
                Email    = string.IsNullOrWhiteSpace(email)    ? null : email.Trim(),
                Telefone = string.IsNullOrWhiteSpace(telefone) ? null : telefone.Trim(),
                Ativo    = ativo
            };

            try
            {
                _clienteRepository.Atualizar(cliente);
                _logger.LogInfo($"Cliente atualizado: {id}");
                return ResultadoOperacao<Cliente>.Sucesso(cliente);
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                return ResultadoOperacao<Cliente>.Falha("Já existe um cliente com este documento.");
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao atualizar cliente.", ex);
                return ResultadoOperacao<Cliente>.Falha("Erro inesperado ao atualizar cliente.");
            }
        }

        public ResultadoOperacao<bool> Excluir(Guid id)
        {
            if (_clienteRepository.PossuiOsVinculada(id))
                return ResultadoOperacao<bool>.Falha("Não é possível excluir. Existem OS vinculadas a este cliente.");

            try
            {
                _clienteRepository.Excluir(id);
                _logger.LogInfo($"Cliente excluído: {id}");
                return ResultadoOperacao<bool>.Sucesso(true);
            }
            catch (PostgresException ex) when (ex.SqlState == "23503")
            {
                return ResultadoOperacao<bool>.Falha("Não é possível excluir. Existem OS vinculadas a este cliente.");
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao excluir cliente.", ex);
                return ResultadoOperacao<bool>.Falha("Erro inesperado ao excluir cliente.");
            }
        }

        public ResultadoOperacao<Cliente> BuscarPorId(Guid id)
        {
            try
            {
                var cliente = _clienteRepository.BuscarPorId(id);
                if (cliente == null)
                    return ResultadoOperacao<Cliente>.Falha("Cliente não encontrado.");

                return ResultadoOperacao<Cliente>.Sucesso(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao buscar cliente.", ex);
                return ResultadoOperacao<Cliente>.Falha("Erro inesperado ao buscar cliente.");
            }
        }

        public ResultadoOperacao<List<Cliente>> Listar(string nome = null, string documento = null, bool? ativo = null)
        {
            try
            {
                var lista = _clienteRepository.Listar(nome, documento, ativo);
                return ResultadoOperacao<List<Cliente>>.Sucesso(lista);
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao listar clientes.", ex);
                return ResultadoOperacao<List<Cliente>>.Falha("Erro inesperado ao listar clientes.");
            }
        }
    }
}
