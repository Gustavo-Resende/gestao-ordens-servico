using System;
using System.Configuration;
using System.Windows.Forms;
using GestaoOrdensServico.Application.Services;
using GestaoOrdensServico.Forms;
using GestaoOrdensServico.Infrastructure.Connection;
using GestaoOrdensServico.Infrastructure.Logging;
using GestaoOrdensServico.Infrastructure.Repositories;

namespace GestaoOrdensServico
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            string connectionString = ConfigurationManager
                .ConnectionStrings["DefaultConnection"].ConnectionString;

            var factory = new DbConnectionFactory(connectionString);
            var logger  = new Logger();

            var clienteRepo      = new ClienteRepository(factory);
            var servicoRepo      = new ServicoRepository(factory);
            var ordemServicoRepo = new OrdemServicoRepository(factory);
            var auditoriaRepo    = new AuditoriaRepository(factory);

            var clienteService      = new ClienteService(clienteRepo, logger);
            var servicoService      = new ServicoService(servicoRepo, logger);
            var ordemServicoService = new OrdemServicoService(ordemServicoRepo, clienteRepo, servicoRepo, auditoriaRepo, logger);
            var relatorioService    = new RelatorioService(ordemServicoRepo, logger);

            System.Windows.Forms.Application.Run(new FormPrincipal(clienteService, servicoService, ordemServicoService, relatorioService));
        }
    }
}
