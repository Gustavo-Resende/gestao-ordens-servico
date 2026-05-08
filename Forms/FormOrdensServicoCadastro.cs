using System;
using System.Windows.Forms;
using GestaoOrdensServico.Application;
using GestaoOrdensServico.Application.Services;
using GestaoOrdensServico.Domain.Entities;
using GestaoOrdensServico.Domain.Enums;

namespace GestaoOrdensServico.Forms
{
    public partial class FormOrdensServicoCadastro : Form
    {
        private readonly OrdemServicoService _ordemServicoService;
        private readonly ClienteService _clienteService;
        private readonly ServicoService _servicoService;
        private readonly OrdemServico _ordemServico;

        private OrdemServico _osAtual;

        public FormOrdensServicoCadastro(
            OrdemServicoService ordemServicoService,
            ClienteService clienteService,
            ServicoService servicoService,
            OrdemServico ordemServico = null)
        {
            InitializeComponent();
            _ordemServicoService = ordemServicoService;
            _clienteService = clienteService;
            _servicoService = servicoService;
            _ordemServico = ordemServico;
        }

        private void FormOrdensServicoCadastro_Load(object sender, System.EventArgs e)
        {
            var clientes = _clienteService.Listar(null, null, true);
            if (clientes.Sucedeu)
            {
                cmbCliente.DataSource = clientes.Dados;
                cmbCliente.DisplayMember = "Nome";
                cmbCliente.ValueMember = "Id";
            }

            var servicos = _servicoService.Listar(true);
            if (servicos.Sucedeu)
            {
                cmbServico.DataSource = servicos.Dados;
                cmbServico.DisplayMember = "Nome";
                cmbServico.ValueMember = "Id";
            }

            cmbNovoStatus.Items.Add("EmAndamento");
            cmbNovoStatus.Items.Add("Concluida");
            cmbNovoStatus.Items.Add("Cancelada");
            cmbNovoStatus.SelectedIndex = 0;

            if (_ordemServico != null)
            {
                var resultado = _ordemServicoService.BuscarPorId(_ordemServico.Id);
                if (!resultado.Sucedeu)
                {
                    MessageBox.Show(resultado.Mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _osAtual = resultado.Dados;
                cmbCliente.SelectedValue = _osAtual.ClienteId;
                txtObservacao.Text = _osAtual.Observacao;
                lblStatus.Text = _osAtual.Status.ToString();
                lblValorTotal.Text = _osAtual.ValorTotal.ToString("C");

                dgvItens.DataSource = null;
                dgvItens.DataSource = _osAtual.Itens;
                ConfigurarColunasItens();

                cmbCliente.Enabled = false;
            }
            else
            {
                _osAtual = null;
                btnAdicionarItem.Enabled = false;
                btnRemoverItem.Enabled = false;
                btnAlterarStatus.Enabled = false;
            }
        }

        private void btnSalvar_Click(object sender, System.EventArgs e)
        {
            if (_osAtual == null)
            {
                if (cmbCliente.SelectedValue == null)
                {
                    MessageBox.Show("Selecione um cliente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var clienteId = (Guid)cmbCliente.SelectedValue;
                var observacao = txtObservacao.Text.Trim();

                var resultado = _ordemServicoService.Criar(clienteId, observacao);
                if (!resultado.Sucedeu)
                {
                    MessageBox.Show(resultado.Mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _osAtual = resultado.Dados;
                lblStatus.Text = _osAtual.Status.ToString();
                lblValorTotal.Text = _osAtual.ValorTotal.ToString("C");

                btnAdicionarItem.Enabled = true;
                btnRemoverItem.Enabled = true;
                btnAlterarStatus.Enabled = true;
                cmbCliente.Enabled = false;
            }
            else
            {
                var observacao = txtObservacao.Text.Trim();

                var resultado = _ordemServicoService.Atualizar(_osAtual.Id, observacao, _osAtual.Versao);
                if (!resultado.Sucedeu)
                {
                    MessageBox.Show(resultado.Mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("OS atualizada com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAdicionarItem_Click(object sender, System.EventArgs e)
        {
            if (cmbServico.SelectedValue == null)
            {
                MessageBox.Show("Selecione um serviço.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtQuantidade.Text.Trim(), out int quantidade) || quantidade < 1)
            {
                MessageBox.Show("Quantidade inválida. Informe um número inteiro maior que zero.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var servicoId = (Guid)cmbServico.SelectedValue;
            var resultado = _ordemServicoService.AdicionarItem(_osAtual.Id, servicoId, quantidade);

            if (!resultado.Sucedeu)
            {
                MessageBox.Show(resultado.Mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AtualizarTela();
        }

        private void btnRemoverItem_Click(object sender, System.EventArgs e)
        {
            if (dgvItens.CurrentRow == null)
            {
                MessageBox.Show("Selecione um item.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var item = dgvItens.CurrentRow.DataBoundItem as ItemOs;
            if (item == null) return;

            var confirmacao = MessageBox.Show(
                $"Deseja remover o item \"{item.ServicoNome}\"?",
                "Confirmar remoção",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacao != DialogResult.Yes) return;

            var resultado = _ordemServicoService.RemoverItem(item.Id, _osAtual.Id);
            if (!resultado.Sucedeu)
            {
                MessageBox.Show(resultado.Mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AtualizarTela();
        }

        private void btnAlterarStatus_Click(object sender, System.EventArgs e)
        {
            var statusTexto = cmbNovoStatus.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(statusTexto)) return;

            var novoStatus = (StatusOs)Enum.Parse(typeof(StatusOs), statusTexto);

            var resultado = _ordemServicoService.AlterarStatus(_osAtual.Id, novoStatus);
            if (!resultado.Sucedeu)
            {
                MessageBox.Show(resultado.Mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AtualizarTela();
            DialogResult = DialogResult.OK;
        }

        private void btnCancelar_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void AtualizarTela()
        {
            var resultado = _ordemServicoService.BuscarPorId(_osAtual.Id);
            if (!resultado.Sucedeu) return;

            _osAtual = resultado.Dados;

            lblStatus.Text = _osAtual.Status.ToString();
            lblValorTotal.Text = _osAtual.ValorTotal.ToString("C");

            dgvItens.DataSource = null;
            dgvItens.DataSource = _osAtual.Itens;
            ConfigurarColunasItens();
        }

        private void ConfigurarColunasItens()
        {
            if (dgvItens.Columns.Count == 0) return;

            dgvItens.Columns["Id"].Visible                        = false;
            dgvItens.Columns["OrdemServicoId"].Visible            = false;
            dgvItens.Columns["ServicoId"].Visible                 = false;
            dgvItens.Columns["PercentualImpostoAplicado"].Visible = false;

            dgvItens.Columns["ServicoNome"].HeaderText    = "Serviço";
            dgvItens.Columns["Quantidade"].HeaderText     = "Qtd";
            dgvItens.Columns["ValorUnitario"].HeaderText  = "Valor Unit.";
            dgvItens.Columns["ValorTotalItem"].HeaderText = "Total";
        }
    }
}
