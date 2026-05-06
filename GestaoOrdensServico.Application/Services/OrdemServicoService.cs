using System;
using System.Collections.Generic;
using GestaoOrdensServico.Domain.Entities;
using GestaoOrdensServico.Domain.Enums;
using GestaoOrdensServico.Infrastructure.Logging;
using GestaoOrdensServico.Infrastructure.Repositories;
using Newtonsoft.Json;

namespace GestaoOrdensServico.Application.Services
{
    public class OrdemServicoService
    {
        private readonly OrdemServicoRepository _ordemServicoRepository;
        private readonly ClienteRepository _clienteRepository;
        private readonly ServicoRepository _servicoRepository;
        private readonly AuditoriaRepository _auditoriaRepository;
        private readonly Logger _logger;

        public OrdemServicoService(
            OrdemServicoRepository ordemServicoRepository,
            ClienteRepository clienteRepository,
            ServicoRepository servicoRepository,
            AuditoriaRepository auditoriaRepository,
            Logger logger)
        {
            _ordemServicoRepository = ordemServicoRepository;
            _clienteRepository      = clienteRepository;
            _servicoRepository      = servicoRepository;
            _auditoriaRepository    = auditoriaRepository;
            _logger                 = logger;
        }

        public ResultadoOperacao<OrdemServico> Criar(Guid clienteId, string observacao)
        {
            var cliente = _clienteRepository.BuscarPorId(clienteId);
            if (cliente == null)
                return ResultadoOperacao<OrdemServico>.Falha("Cliente não encontrado.");

            if (!cliente.Ativo)
                return ResultadoOperacao<OrdemServico>.Falha("Não é possível abrir OS para cliente inativo.");

            var os = new OrdemServico
            {
                Id           = Guid.NewGuid(),
                ClienteId    = clienteId,
                DataAbertura = DateTime.Now,
                Status       = StatusOs.Aberta,
                Observacao   = string.IsNullOrWhiteSpace(observacao) ? null : observacao.Trim(),
                ValorTotal   = 0,
                Versao       = 1
            };

            var snapshot = JsonConvert.SerializeObject(os);

            try
            {
                _ordemServicoRepository.Inserir(os, snapshot);
                _logger.LogInfo($"OS criada: {os.Id} — Cliente: {clienteId}");
                return ResultadoOperacao<OrdemServico>.Sucesso(os);
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao criar OS.", ex);
                return ResultadoOperacao<OrdemServico>.Falha("Erro inesperado ao criar OS.");
            }
        }

        public ResultadoOperacao<ItemOs> AdicionarItem(Guid osId, Guid servicoId, int quantidade)
        {
            if (quantidade < 1)
                return ResultadoOperacao<ItemOs>.Falha("Quantidade deve ser maior que zero.");

            var os = _ordemServicoRepository.BuscarPorId(osId);
            if (os == null)
                return ResultadoOperacao<ItemOs>.Falha("OS não encontrada.");

            if (os.Status == StatusOs.Concluida || os.Status == StatusOs.Cancelada)
                return ResultadoOperacao<ItemOs>.Falha("Não é possível adicionar itens em OS Concluída ou Cancelada.");

            var servico = _servicoRepository.BuscarPorId(servicoId);
            if (servico == null)
                return ResultadoOperacao<ItemOs>.Falha("Serviço não encontrado.");

            if (!servico.Ativo)
                return ResultadoOperacao<ItemOs>.Falha("Não é possível adicionar serviço inativo.");

            decimal valorTotalItem = (quantidade * servico.ValorBase) * (1 + servico.PercentualImposto / 100);

            var item = new ItemOs
            {
                Id                        = Guid.NewGuid(),
                OrdemServicoId            = osId,
                ServicoId                 = servicoId,
                ServicoNome               = servico.Nome,
                Quantidade                = quantidade,
                ValorUnitario             = servico.ValorBase,
                PercentualImpostoAplicado = servico.PercentualImposto,
                ValorTotalItem            = Math.Round(valorTotalItem, 2)
            };

            var snapshot = JsonConvert.SerializeObject(os);

            try
            {
                _ordemServicoRepository.InserirItem(item, os.Versao, snapshot);
                _logger.LogInfo($"Item adicionado na OS {osId}: serviço {servicoId}, qtd {quantidade}");
                return ResultadoOperacao<ItemOs>.Sucesso(item);
            }
            catch (Exception ex) when (ex.Message.Contains("alterada por outro usuário"))
            {
                return ResultadoOperacao<ItemOs>.Falha(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao adicionar item na OS.", ex);
                return ResultadoOperacao<ItemOs>.Falha("Erro inesperado ao adicionar item.");
            }
        }

        public ResultadoOperacao<bool> RemoverItem(Guid itemId, Guid osId)
        {
            var os = _ordemServicoRepository.BuscarPorId(osId);
            if (os == null)
                return ResultadoOperacao<bool>.Falha("OS não encontrada.");

            if (os.Status == StatusOs.Concluida || os.Status == StatusOs.Cancelada)
                return ResultadoOperacao<bool>.Falha("Não é possível remover itens de OS Concluída ou Cancelada.");

            var snapshot = JsonConvert.SerializeObject(os);

            try
            {
                _ordemServicoRepository.RemoverItem(itemId, osId, os.Versao, snapshot);
                _logger.LogInfo($"Item {itemId} removido da OS {osId}");
                return ResultadoOperacao<bool>.Sucesso(true);
            }
            catch (Exception ex) when (ex.Message.Contains("alterada por outro usuário"))
            {
                return ResultadoOperacao<bool>.Falha(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao remover item da OS.", ex);
                return ResultadoOperacao<bool>.Falha("Erro inesperado ao remover item.");
            }
        }

        public ResultadoOperacao<bool> AlterarStatus(Guid osId, StatusOs novoStatus)
        {
            var os = _ordemServicoRepository.BuscarPorId(osId);
            if (os == null)
                return ResultadoOperacao<bool>.Falha("OS não encontrada.");

            if (!TransicaoValida(os.Status, novoStatus))
                return ResultadoOperacao<bool>.Falha(
                    $"Transição de '{os.Status}' para '{novoStatus}' não é permitida.");

            if (novoStatus == StatusOs.Concluida && os.ValorTotal == 0)
                return ResultadoOperacao<bool>.Falha("Não é possível concluir uma OS com valor total zero.");

            DateTime? dataConclusao = novoStatus == StatusOs.Concluida ? DateTime.Now : (DateTime?)null;

            var snapshot = JsonConvert.SerializeObject(os);

            try
            {
                _ordemServicoRepository.AlterarStatus(osId, novoStatus, os.Versao, dataConclusao, snapshot);
                _logger.LogInfo($"OS {osId}: status alterado de {os.Status} para {novoStatus}");
                return ResultadoOperacao<bool>.Sucesso(true);
            }
            catch (Exception ex) when (ex.Message.Contains("alterada por outro usuário"))
            {
                return ResultadoOperacao<bool>.Falha(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao alterar status da OS.", ex);
                return ResultadoOperacao<bool>.Falha("Erro inesperado ao alterar status.");
            }
        }

        public ResultadoOperacao<bool> Concluir(Guid osId)
        {
            return AlterarStatus(osId, StatusOs.Concluida);
        }

        public ResultadoOperacao<OrdemServico> BuscarPorId(Guid id)
        {
            try
            {
                var os = _ordemServicoRepository.BuscarPorId(id);
                if (os == null)
                    return ResultadoOperacao<OrdemServico>.Falha("OS não encontrada.");

                os.Itens = _ordemServicoRepository.BuscarItens(id);
                return ResultadoOperacao<OrdemServico>.Sucesso(os);
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao buscar OS.", ex);
                return ResultadoOperacao<OrdemServico>.Falha("Erro inesperado ao buscar OS.");
            }
        }

        public ResultadoOperacao<List<OrdemServico>> ListarPaginado(
            DateTime? inicio, DateTime? fim, Guid? clienteId, string status,
            int pagina, int tamanhoPagina, out int total)
        {
            total = 0;
            try
            {
                var lista = _ordemServicoRepository.ListarPaginado(
                    inicio, fim, clienteId, status, pagina, tamanhoPagina, out total);
                return ResultadoOperacao<List<OrdemServico>>.Sucesso(lista);
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao listar OS.", ex);
                return ResultadoOperacao<List<OrdemServico>>.Falha("Erro inesperado ao listar OS.");
            }
        }

        private static bool TransicaoValida(StatusOs atual, StatusOs novo)
        {
            switch (atual)
            {
                case StatusOs.Aberta:
                    return novo == StatusOs.EmAndamento || novo == StatusOs.Cancelada;
                case StatusOs.EmAndamento:
                    return novo == StatusOs.Concluida || novo == StatusOs.Cancelada;
                default:
                    return false;
            }
        }
    }
}
