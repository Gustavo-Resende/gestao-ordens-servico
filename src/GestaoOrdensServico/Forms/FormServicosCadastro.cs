using System;
using System.Windows.Forms;
using GestaoOrdensServico.Application;
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

        private void FormServicosCadastro_Load(object sender, System.EventArgs e)
        {
            if (_servico != null)
            {
                txtNome.Text = _servico.Nome;
                txtValorBase.Text = _servico.ValorBase.ToString("F2");
                txtPercentualImposto.Text = _servico.PercentualImposto.ToString("F2");
                chkAtivo.Checked = _servico.Ativo;
            }
            else
            {
                chkAtivo.Checked = true;
            }
        }

        private void btnSalvar_Click(object sender, System.EventArgs e)
        {
            var nome = txtNome.Text.Trim();

            if (!decimal.TryParse(txtValorBase.Text.Trim(), out decimal valorBase))
            {
                MessageBox.Show("Valor Base inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtPercentualImposto.Text.Trim(), out decimal percentualImposto))
            {
                MessageBox.Show("Percentual de Imposto inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var ativo = chkAtivo.Checked;

            ResultadoOperacao<Servico> resultado;

            if (_servico == null)
            {
                resultado = _servicoService.Criar(nome, valorBase, percentualImposto);
            }
            else
            {
                resultado = _servicoService.Atualizar(_servico.Id, nome, valorBase, percentualImposto, ativo);
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
