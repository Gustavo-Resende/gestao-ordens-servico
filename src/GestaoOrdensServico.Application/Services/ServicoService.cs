using System;
using System.Collections.Generic;
using GestaoOrdensServico.Domain.Entities;
using GestaoOrdensServico.Infrastructure.Logging;
using GestaoOrdensServico.Infrastructure.Repositories;

namespace GestaoOrdensServico.Application.Services
{
    public class ServicoService
    {
        private readonly ServicoRepository _servicoRepository;
        private readonly Logger _logger;

        public ServicoService(ServicoRepository servicoRepository, Logger logger)
        {
            _servicoRepository = servicoRepository;
            _logger = logger;
        }

        public ResultadoOperacao<Servico> Criar(string nome, decimal valorBase, decimal percentualImposto)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return ResultadoOperacao<Servico>.Falha("Nome é obrigatório.");

            if (valorBase <= 0)
                return ResultadoOperacao<Servico>.Falha("Valor base deve ser maior que zero.");

            if (percentualImposto < 0 || percentualImposto > 100)
                return ResultadoOperacao<Servico>.Falha("Percentual de imposto deve estar entre 0 e 100.");

            var servico = new Servico
            {
                Id                = Guid.NewGuid(),
                Nome              = nome.Trim(),
                ValorBase         = valorBase,
                PercentualImposto = percentualImposto,
                Ativo             = true
            };

            try
            {
                _servicoRepository.Inserir(servico);
                _logger.LogInfo($"Serviço criado: {servico.Id} — {servico.Nome}");
                return ResultadoOperacao<Servico>.Sucesso(servico);
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao criar serviço.", ex);
                return ResultadoOperacao<Servico>.Falha("Erro inesperado ao criar serviço.");
            }
        }

        public ResultadoOperacao<Servico> Atualizar(Guid id, string nome, decimal valorBase,
            decimal percentualImposto, bool ativo)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return ResultadoOperacao<Servico>.Falha("Nome é obrigatório.");

            if (valorBase <= 0)
                return ResultadoOperacao<Servico>.Falha("Valor base deve ser maior que zero.");

            if (percentualImposto < 0 || percentualImposto > 100)
                return ResultadoOperacao<Servico>.Falha("Percentual de imposto deve estar entre 0 e 100.");

            var servico = new Servico
            {
                Id                = id,
                Nome              = nome.Trim(),
                ValorBase         = valorBase,
                PercentualImposto = percentualImposto,
                Ativo             = ativo
            };

            try
            {
                _servicoRepository.Atualizar(servico);
                _logger.LogInfo($"Serviço atualizado: {id}");
                return ResultadoOperacao<Servico>.Sucesso(servico);
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao atualizar serviço.", ex);
                return ResultadoOperacao<Servico>.Falha("Erro inesperado ao atualizar serviço.");
            }
        }

        public ResultadoOperacao<Servico> BuscarPorId(Guid id)
        {
            try
            {
                var servico = _servicoRepository.BuscarPorId(id);
                if (servico == null)
                    return ResultadoOperacao<Servico>.Falha("Serviço não encontrado.");

                return ResultadoOperacao<Servico>.Sucesso(servico);
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao buscar serviço.", ex);
                return ResultadoOperacao<Servico>.Falha("Erro inesperado ao buscar serviço.");
            }
        }

        public ResultadoOperacao<List<Servico>> Listar(bool? ativo = null)
        {
            try
            {
                var lista = _servicoRepository.Listar(ativo);
                return ResultadoOperacao<List<Servico>>.Sucesso(lista);
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao listar serviços.", ex);
                return ResultadoOperacao<List<Servico>>.Falha("Erro inesperado ao listar serviços.");
            }
        }
    }
}
