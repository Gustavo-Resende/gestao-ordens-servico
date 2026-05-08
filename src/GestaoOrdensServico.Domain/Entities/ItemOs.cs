using System;

namespace GestaoOrdensServico.Domain.Entities
{
    public class ItemOs
    {
        public Guid Id { get; set; }
        public Guid OrdemServicoId { get; set; }
        public Guid ServicoId { get; set; }
        public string ServicoNome { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal PercentualImpostoAplicado { get; set; }
        public decimal ValorTotalItem { get; set; }
    }
}
