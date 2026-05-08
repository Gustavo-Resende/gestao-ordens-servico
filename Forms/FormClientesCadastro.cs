using System.Windows.Forms;
using GestaoOrdensServico.Application;
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

        private void FormClientesCadastro_Load(object sender, System.EventArgs e)
        {
            cmbTipo.Items.AddRange(new object[] { "Fisica", "Juridica" });

            if (_cliente != null)
            {
                txtNome.Text = _cliente.Nome;
                txtDocumento.Text = _cliente.Documento;
                cmbTipo.SelectedItem = _cliente.Tipo;
                txtEmail.Text = _cliente.Email;
                txtTelefone.Text = _cliente.Telefone;
                chkAtivo.Checked = _cliente.Ativo;
            }
            else
            {
                chkAtivo.Checked = true;
            }
        }

        private void btnSalvar_Click(object sender, System.EventArgs e)
        {
            var nome = txtNome.Text.Trim();
            var documento = txtDocumento.Text.Trim();
            var tipo = cmbTipo.SelectedItem?.ToString();
            var email = txtEmail.Text.Trim();
            var telefone = txtTelefone.Text.Trim();
            var ativo = chkAtivo.Checked;

            ResultadoOperacao<Cliente> resultado;

            if (_cliente == null)
            {
                resultado = _clienteService.Criar(nome, documento, tipo, email, telefone);
            }
            else
            {
                resultado = _clienteService.Atualizar(_cliente.Id, nome, documento, tipo, email, telefone, ativo);
            }

            if (!resultado.Sucedeu)
            {
                MessageBox.Show(resultado.Mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
