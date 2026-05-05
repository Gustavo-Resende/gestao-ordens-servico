using System.Windows.Forms;
using GestaoOrdensServico.Application.Services;

namespace GestaoOrdensServico.Forms
{
    public partial class FormRelatorio : Form
    {
        private readonly RelatorioService _relatorioService;
        private readonly ClienteService _clienteService;

        public FormRelatorio(RelatorioService relatorioService, ClienteService clienteService)
        {
            InitializeComponent();
            _relatorioService = relatorioService;
            _clienteService = clienteService;
        }
    }
}
