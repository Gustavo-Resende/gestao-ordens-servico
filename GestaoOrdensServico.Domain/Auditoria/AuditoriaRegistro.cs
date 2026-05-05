using System;

namespace GestaoOrdensServico.Domain.Auditoria
{
    public class AuditoriaRegistro
    {
        public Guid Id { get; set; }
        public string Entidade { get; set; }
        public Guid IdRegistro { get; set; }
        public string Operacao { get; set; }
        public DateTime DataHora { get; set; }
        public string Usuario { get; set; }
        public string Snapshot { get; set; }
    }
}
