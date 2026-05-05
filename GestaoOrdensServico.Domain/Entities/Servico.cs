using System;

namespace GestaoOrdensServico.Domain.Entities
{
    public class Servico
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal ValorBase { get; set; }
        public decimal PercentualImposto { get; set; }
        public bool Ativo { get; set; }
    }
}
