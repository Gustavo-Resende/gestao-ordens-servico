using System.Windows.Forms;
using GestaoOrdensServico.Application.Services;
using GestaoOrdensServico.Domain.Entities;

namespace GestaoOrdensServico.Forms
{
    public partial class FormServicos : Form
    {
        private readonly ServicoService _servicoService;

        public FormServicos(ServicoService servicoService)
        {
            InitializeComponent();
            _servicoService = servicoService;
            btnEditar.Enabled = false;
            dgvServicos.SelectionChanged += dgvServicos_SelectionChanged;
        }

        private void dgvServicos_SelectionChanged(object sender, System.EventArgs e)
        {
            btnEditar.Enabled = dgvServicos.CurrentRow != null;
        }

        private void FormServicos_Load(object sender, System.EventArgs e)
        {
            Pesquisar();
        }

        private void btnPesquisar_Click(object sender, System.EventArgs e)
        {
            Pesquisar();
        }

        private void btnNovo_Click(object sender, System.EventArgs e)
        {
            var form = new FormServicosCadastro(_servicoService, null);
            if (form.ShowDialog() == DialogResult.OK)
                Pesquisar();
        }

        private void btnEditar_Click(object sender, System.EventArgs e)
        {
            var servico = ObterServicoSelecionado();
            if (servico == null) return;

            var form = new FormServicosCadastro(_servicoService, servico);
            if (form.ShowDialog() == DialogResult.OK)
                Pesquisar();
        }

        private void Pesquisar()
        {
            bool? ativo = chkFiltroAtivo.Checked ? (bool?)true : null;

            var resultado = _servicoService.Listar(ativo);
            if (!resultado.Sucedeu)
            {
                MessageBox.Show(resultado.Mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvServicos.DataSource = resultado.Dados;

            if (dgvServicos.Columns.Count == 0) return;

            dgvServicos.Columns["Id"].Visible                      = false;
            dgvServicos.Columns["Nome"].HeaderText                 = "Nome";
            dgvServicos.Columns["ValorBase"].HeaderText            = "Valor Base";
            dgvServicos.Columns["PercentualImposto"].HeaderText    = "Imposto (%)";
            dgvServicos.Columns["Ativo"].HeaderText                = "Ativo";
        }

        private Servico ObterServicoSelecionado()
        {
            if (dgvServicos.CurrentRow == null)
            {
                MessageBox.Show("Selecione um serviço.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            return dgvServicos.CurrentRow.DataBoundItem as Servico;
        }
    }
}
