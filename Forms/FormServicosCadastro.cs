using System.Windows.Forms;
using GestaoOrdensServico.Application.Services;
using GestaoOrdensServico.Domain.Entities;

namespace GestaoOrdensServico.Forms
{
    public partial class FormServicosCadastro : Form
    {
        private readonly ServicoService _servicoService;
        private readonly Servico _servico;

        public FormServicosCadastro(ServicoService servicoService, Servico servico = null)
        {
            InitializeComponent();
            _servicoService = servicoService;
            _servico = servico;
        }
    }
}
