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
    }
}
