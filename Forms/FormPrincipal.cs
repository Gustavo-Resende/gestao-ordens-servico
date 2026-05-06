using System.Windows.Forms;
using GestaoOrdensServico.Application.Services;

namespace GestaoOrdensServico.Forms
{
    public partial class FormPrincipal : Form
    {
        private readonly ClienteService _clienteService;
        private readonly ServicoService _servicoService;
        private readonly OrdemServicoService _ordemServicoService;
        private readonly RelatorioService _relatorioService;

        public FormPrincipal(
            ClienteService clienteService,
            ServicoService servicoService,
            OrdemServicoService ordemServicoService,
            RelatorioService relatorioService)
        {
            InitializeComponent();
            _clienteService = clienteService;
            _servicoService = servicoService;
            _ordemServicoService = ordemServicoService;
            _relatorioService = relatorioService;
        }

        private void btnClientes_Click(object sender, System.EventArgs e)
        {
            new FormClientes(_clienteService).ShowDialog();
        }

        private void btnServicos_Click(object sender, System.EventArgs e)
        {
            new FormServicos(_servicoService).ShowDialog();
        }

        private void btnOrdensServico_Click(object sender, System.EventArgs e)
        {
            new FormOrdensServico(_ordemServicoService, _clienteService).ShowDialog();
        }

        private void btnRelatorio_Click(object sender, System.EventArgs e)
        {
            new FormRelatorio(_relatorioService, _clienteService).ShowDialog();
        }
    }
}
