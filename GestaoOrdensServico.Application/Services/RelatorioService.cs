using System;
using System.Collections.Generic;
using System.Linq;
using GestaoOrdensServico.Application.Relatorios;
using GestaoOrdensServico.Infrastructure.Logging;
using GestaoOrdensServico.Infrastructure.Repositories;

namespace GestaoOrdensServico.Application.Services
{
    public class RelatorioService
    {
        private readonly OrdemServicoRepository _ordemServicoRepository;
        private readonly Logger _logger;

        public RelatorioService(OrdemServicoRepository ordemServicoRepository, Logger logger)
        {
            _ordemServicoRepository = ordemServicoRepository;
            _logger = logger;
        }

        public ResultadoOperacao<List<RelatorioOsItem>> GerarRelatorio(
            DateTime inicio, DateTime fim, Guid? clienteId, string status)
        {
            if (fim < inicio)
                return ResultadoOperacao<List<RelatorioOsItem>>.Falha("Data fim deve ser maior ou igual à data início.");

            try
            {
                var ordens = _ordemServicoRepository.ListarParaRelatorio(inicio, fim, clienteId, status);

                var itens = new List<RelatorioOsItem>();
                foreach (var os in ordens)
                {
                    var itensOs = _ordemServicoRepository.BuscarItens(os.Id);
                    decimal totalImpostos = itensOs.Sum(i =>
                        i.ValorUnitario * i.Quantidade * (i.PercentualImpostoAplicado / 100));

                    itens.Add(new RelatorioOsItem
                    {
                        ClienteNome   = os.ClienteNome,
                        OsId          = os.Id.ToString(),
                        DataAbertura  = os.DataAbertura,
                        Status        = os.Status.ToString(),
                        ValorTotal    = os.ValorTotal,
                        TotalImpostos = Math.Round(totalImpostos, 2)
                    });
                }

                _logger.LogInfo($"Relatório gerado: {itens.Count} OS no período {inicio:d} a {fim:d}");
                return ResultadoOperacao<List<RelatorioOsItem>>.Sucesso(itens);
            }
            catch (Exception ex)
            {
                _logger.LogErro("Erro ao gerar relatório.", ex);
                return ResultadoOperacao<List<RelatorioOsItem>>.Falha("Erro inesperado ao gerar relatório.");
            }
        }
    }
}
