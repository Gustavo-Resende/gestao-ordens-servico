using System.Windows.Forms;
using GestaoOrdensServico.Application.Services;

namespace GestaoOrdensServico.Forms
{
    public partial class FormServicos : Form
    {
        private readonly ServicoService _servicoService;

        public FormServicos(ServicoService servicoService)
        {
            InitializeComponent();
            _servicoService = servicoService;
        }
    }
}
