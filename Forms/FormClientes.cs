using System.Windows.Forms;
using GestaoOrdensServico.Application.Services;
using GestaoOrdensServico.Domain.Entities;

namespace GestaoOrdensServico.Forms
{
    public partial class FormClientes : Form
    {
        private readonly ClienteService _clienteService;

        public FormClientes(ClienteService clienteService)
        {
            InitializeComponent();
            _clienteService = clienteService;
            btnEditar.Enabled = false;
            btnExcluir.Enabled = false;
        }

        private void dgvClientes_SelectionChanged(object sender, System.EventArgs e)
        {
            bool temSeleção = dgvClientes.CurrentRow != null;
            btnEditar.Enabled = temSeleção;
            btnExcluir.Enabled = temSeleção;
        }

        private void FormClientes_Load(object sender, System.EventArgs e)
        {
            Pesquisar();
        }

        private void btnPesquisar_Click(object sender, System.EventArgs e)
        {
            Pesquisar();
        }

        private void btnNovo_Click(object sender, System.EventArgs e)
        {
            var form = new FormClientesCadastro(_clienteService, null);
            if (form.ShowDialog() == DialogResult.OK)
                Pesquisar();
        }

        private void btnEditar_Click(object sender, System.EventArgs e)
        {
            var cliente = ObterClienteSelecionado();
            if (cliente == null) return;

            var form = new FormClientesCadastro(_clienteService, cliente);
            if (form.ShowDialog() == DialogResult.OK)
                Pesquisar();
        }

        private void btnExcluir_Click(object sender, System.EventArgs e)
        {
            var cliente = ObterClienteSelecionado();
            if (cliente == null) return;

            var confirmacao = MessageBox.Show(
                $"Deseja excluir o cliente \"{cliente.Nome}\"?",
                "Confirmar exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacao != DialogResult.Yes) return;

            var resultado = _clienteService.Excluir(cliente.Id);
            if (resultado.Sucedeu)
            {
                MessageBox.Show("Cliente excluído com sucesso.", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Pesquisar();
            }
            else
            {
                MessageBox.Show(resultado.Mensagem, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Pesquisar()
        {
            string nome      = string.IsNullOrWhiteSpace(txtFiltroNome.Text)      ? null : txtFiltroNome.Text.Trim();
            string documento = string.IsNullOrWhiteSpace(txtFiltroDocumento.Text) ? null : txtFiltroDocumento.Text.Trim();
            bool? ativo      = chkFiltroAtivo.Checked ? (bool?)true : null;

            var resultado = _clienteService.Listar(nome, documento, ativo);
            if (!resultado.Sucedeu)
            {
                MessageBox.Show(resultado.Mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvClientes.AutoGenerateColumns = true;
            dgvClientes.DataSource = resultado.Dados;

            if (dgvClientes.Columns.Contains("Id"))
                dgvClientes.Columns["Id"].Visible = false;
            if (dgvClientes.Columns.Contains("Nome"))
                dgvClientes.Columns["Nome"].HeaderText = "Nome";
            if (dgvClientes.Columns.Contains("Documento"))
                dgvClientes.Columns["Documento"].HeaderText = "Documento";
            if (dgvClientes.Columns.Contains("Tipo"))
                dgvClientes.Columns["Tipo"].HeaderText = "Tipo";
            if (dgvClientes.Columns.Contains("Email"))
                dgvClientes.Columns["Email"].HeaderText = "E-mail";
            if (dgvClientes.Columns.Contains("Telefone"))
                dgvClientes.Columns["Telefone"].HeaderText = "Telefone";
            if (dgvClientes.Columns.Contains("Ativo"))
                dgvClientes.Columns["Ativo"].HeaderText = "Ativo";
            if (dgvClientes.Columns.Contains("DataCadastro"))
                dgvClientes.Columns["DataCadastro"].Visible = false;
        }

        private Cliente ObterClienteSelecionado()
        {
            if (dgvClientes.CurrentRow == null)
            {
                MessageBox.Show("Selecione um cliente.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            return dgvClientes.CurrentRow.DataBoundItem as Cliente;
        }
    }
}
