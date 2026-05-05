using System;
using System.Collections.Generic;
using GestaoOrdensServico.Domain.Enums;

namespace GestaoOrdensServico.Domain.Entities
{
    public class OrdemServico
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public string ClienteNome { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime? DataConclusao { get; set; }
        public StatusOs Status { get; set; }
        public string Observacao { get; set; }
        public decimal ValorTotal { get; set; }
        public int Versao { get; set; }
        public List<ItemOs> Itens { get; set; } = new List<ItemOs>();
    }
}
