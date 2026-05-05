using System.Windows.Forms;
using GestaoOrdensServico.Application.Services;
using GestaoOrdensServico.Domain.Entities;

namespace GestaoOrdensServico.Forms
{
    public partial class FormOrdensServicoCadastro : Form
    {
        private readonly OrdemServicoService _ordemServicoService;
        private readonly ClienteService _clienteService;
        private readonly ServicoService _servicoService;
        private readonly OrdemServico _ordemServico;

        public FormOrdensServicoCadastro(
            OrdemServicoService ordemServicoService,
            ClienteService clienteService,
            ServicoService servicoService,
            OrdemServico ordemServico = null)
        {
            InitializeComponent();
            _ordemServicoService = ordemServicoService;
            _clienteService = clienteService;
            _servicoService = servicoService;
            _ordemServico = ordemServico;
        }
    }
}
