using System.Windows.Forms;
using GestaoOrdensServico.Application.Services;

namespace GestaoOrdensServico.Forms
{
    public partial class FormClientes : Form
    {
        private readonly ClienteService _clienteService;

        public FormClientes(ClienteService clienteService)
        {
            InitializeComponent();
            _clienteService = clienteService;
        }
    }
}
