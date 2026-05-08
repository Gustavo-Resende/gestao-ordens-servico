using System;
using System.Windows.Forms;
using GestaoOrdensServico.Application.Services;
using GestaoOrdensServico.Domain.Entities;

namespace GestaoOrdensServico.Forms
{
    public partial class FormOrdensServico : Form
    {
        private readonly OrdemServicoService _ordemServicoService;
        private readonly ClienteService _clienteService;
        private readonly ServicoService _servicoService;

        private int _paginaAtual = 1;
        private int _totalRegistros = 0;
        private const int TamanhoPagina = 20;

        public FormOrdensServico(OrdemServicoService ordemServicoService, ClienteService clienteService, ServicoService servicoService)
        {
            InitializeComponent();
            _ordemServicoService = ordemServicoService;
            _clienteService = clienteService;
            _servicoService = servicoService;
        }

        private void FormOrdensServico_Load(object sender, System.EventArgs e)
        {
            cmbStatus.Items.Add("");
            cmbStatus.Items.Add("Aberta");
            cmbStatus.Items.Add("EmAndamento");
            cmbStatus.Items.Add("Concluida");
            cmbStatus.Items.Add("Cancelada");
            cmbStatus.SelectedIndex = 0;

            dtpInicio.Value = DateTime.Today.AddMonths(-1);
            dtpFim.Value = DateTime.Today;

            Pesquisar();
        }

        private void btnPesquisar_Click(object sender, System.EventArgs e)
        {
            _paginaAtual = 1;
            Pesquisar();
        }

        private void btnAnterior_Click(object sender, System.EventArgs e)
        {
            if (_paginaAtual > 1)
            {
                _paginaAtual--;
                Pesquisar();
            }
        }

        private void btnProximo_Click(object sender, System.EventArgs e)
        {
            int totalPaginas = (int)Math.Ceiling(_totalRegistros / (double)TamanhoPagina);
            if (_paginaAtual < totalPaginas)
            {
                _paginaAtual++;
                Pesquisar();
            }
        }

        private void btnNova_Click(object sender, System.EventArgs e)
        {
            var form = new FormOrdensServicoCadastro(_ordemServicoService, _clienteService, _servicoService, null);
            if (form.ShowDialog() == DialogResult.OK)
                Pesquisar();
        }

        private void btnAbrir_Click(object sender, System.EventArgs e)
        {
            if (dgvOrdensServico.CurrentRow == null)
            {
                MessageBox.Show("Selecione uma ordem de serviço.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var os = dgvOrdensServico.CurrentRow.DataBoundItem as OrdemServico;
            if (os == null) return;

            var form = new FormOrdensServicoCadastro(_ordemServicoService, _clienteService, _servicoService, os);
            if (form.ShowDialog() == DialogResult.OK)
                Pesquisar();
        }

        private void Pesquisar()
        {
            DateTime inicio = dtpInicio.Value.Date;
            DateTime fim = dtpFim.Value.Date;

            string statusTexto = cmbStatus.SelectedItem?.ToString();
            string status = string.IsNullOrEmpty(statusTexto) ? null : statusTexto;

            var resultado = _ordemServicoService.ListarPaginado(
                inicio, fim, null, status, _paginaAtual, TamanhoPagina, out _totalRegistros);

            if (!resultado.Sucedeu)
            {
                MessageBox.Show(resultado.Mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvOrdensServico.DataSource = resultado.Dados;

            if (dgvOrdensServico.Columns.Count == 0) return;

            void Ocultar(string col) { if (dgvOrdensServico.Columns.Contains(col)) dgvOrdensServico.Columns[col].Visible = false; }
            Ocultar("Id");
            Ocultar("ClienteId");
            Ocultar("DataConclusao");
            Ocultar("Observacao");
            Ocultar("Versao");
            Ocultar("Itens");

            dgvOrdensServico.Columns["ClienteNome"].HeaderText  = "Cliente";
            dgvOrdensServico.Columns["DataAbertura"].HeaderText = "Abertura";
            dgvOrdensServico.Columns["Status"].HeaderText       = "Status";
            dgvOrdensServico.Columns["ValorTotal"].HeaderText   = "Valor Total";

            int totalPaginas = (int)Math.Ceiling(_totalRegistros / (double)TamanhoPagina);
            if (totalPaginas < 1) totalPaginas = 1;

            lblPagina.Text = $"Página {_paginaAtual} de {totalPaginas}";
            btnAnterior.Enabled = _paginaAtual > 1;
            btnProximo.Enabled = _paginaAtual < totalPaginas;
        }
    }
}
