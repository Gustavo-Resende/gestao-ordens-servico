using System.Windows.Forms;
using GestaoOrdensServico.Application.Services;

namespace GestaoOrdensServico.Forms
{
    public partial class FormOrdensServico : Form
    {
        private readonly OrdemServicoService _ordemServicoService;
        private readonly ClienteService _clienteService;

        public FormOrdensServico(OrdemServicoService ordemServicoService, ClienteService clienteService)
        {
            InitializeComponent();
            _ordemServicoService = ordemServicoService;
            _clienteService = clienteService;
        }
    }
}
