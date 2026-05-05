using System;

namespace GestaoOrdensServico.Application.Relatorios
{
    public class RelatorioOsItem
    {
        public string ClienteNome { get; set; }
        public string OsId { get; set; }
        public DateTime DataAbertura { get; set; }
        public string Status { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal TotalImpostos { get; set; }
    }
}
