using System.Windows.Forms;
using GestaoOrdensServico.Application.Services;
using GestaoOrdensServico.Domain.Entities;

namespace GestaoOrdensServico.Forms
{
    public partial class FormClientesCadastro : Form
    {
        private readonly ClienteService _clienteService;
        private readonly Cliente _cliente;

        public FormClientesCadastro(ClienteService clienteService, Cliente cliente = null)
        {
            InitializeComponent();
            _clienteService = clienteService;
            _cliente = cliente;
        }
    }
}
