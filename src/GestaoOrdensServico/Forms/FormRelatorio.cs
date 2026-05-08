using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using GestaoOrdensServico.Application.Relatorios;
using GestaoOrdensServico.Application.Services;
using GestaoOrdensServico.Domain.Entities;
using Microsoft.Reporting.WinForms;

namespace GestaoOrdensServico.Forms
{
    public partial class FormRelatorio : Form
    {
        private readonly RelatorioService _relatorioService;
        private readonly ClienteService _clienteService;

        public FormRelatorio(RelatorioService relatorioService, ClienteService clienteService)
        {
            InitializeComponent();
            _relatorioService = relatorioService;
            _clienteService = clienteService;
            reportViewer1.LocalReport.ReportEmbeddedResource = "GestaoOrdensServico.Forms.RelatorioOs.rdlc";
            reportViewer1.LocalReport.SubreportProcessing += (s, ev) => { };
        }

        private void FormRelatorio_Load(object sender, System.EventArgs e)
        {
            dtpInicio.Value = DateTime.Today.AddMonths(-1);
            dtpFim.Value = DateTime.Today;

            var resultado = _clienteService.Listar();
            if (resultado.Sucedeu)
            {
                var clientes = resultado.Dados;
                clientes.Insert(0, new Cliente { Id = Guid.Empty, Nome = "Todos" });
                cmbCliente.DataSource = clientes;
                cmbCliente.DisplayMember = "Nome";
                cmbCliente.ValueMember = "Id";
            }

            cmbStatus.Items.Add("Todos");
            cmbStatus.Items.Add("Aberta");
            cmbStatus.Items.Add("EmAndamento");
            cmbStatus.Items.Add("Concluida");
            cmbStatus.Items.Add("Cancelada");
            cmbStatus.SelectedIndex = 0;
        }

        private List<RelatorioOsItem> _dadosRelatorio = null;

        private void btnGerar_Click(object sender, System.EventArgs e)
        {
            try
            {
                DateTime inicio = dtpInicio.Value.Date;
                DateTime fim = dtpFim.Value.Date.AddDays(1).AddSeconds(-1);

                Guid? clienteId = null;
                if (cmbCliente.SelectedValue is Guid id && id != Guid.Empty)
                    clienteId = id;

                string status = cmbStatus.SelectedItem?.ToString() == "Todos"
                    ? null : cmbStatus.SelectedItem?.ToString();

                var resultado = _relatorioService.GerarRelatorio(inicio, fim, clienteId, status);
                if (!resultado.Sucedeu)
                {
                    MessageBox.Show(resultado.Mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (resultado.Dados.Count == 0)
                {
                    MessageBox.Show("Nenhuma OS encontrada no período.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _dadosRelatorio = resultado.Dados;
                MessageBox.Show($"{resultado.Dados.Count} OS encontradas. Clique em Exportar PDF para gerar o relatório.",
                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportarPdf_Click(object sender, System.EventArgs e)
        {
            if (_dadosRelatorio == null || _dadosRelatorio.Count == 0)
            {
                MessageBox.Show("Clique em Gerar primeiro para buscar os dados.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var dataTable = ConverterParaDataTable(_dadosRelatorio);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;
                reportViewer1.LocalReport.ReportEmbeddedResource = "GestaoOrdensServico.Forms.RelatorioOs.rdlc";
                reportViewer1.LocalReport.DataSources.Add(
                    new Microsoft.Reporting.WinForms.ReportDataSource("DataSetRelatorio", dataTable));

                byte[] bytes = reportViewer1.LocalReport.Render("PDF");

                using (var dialog = new SaveFileDialog())
                {
                    dialog.Filter = "PDF|*.pdf";
                    dialog.FileName = $"relatorio_{DateTime.Now:yyyy-MM-dd}.pdf";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.File.WriteAllBytes(dialog.FileName, bytes);
                        MessageBox.Show("PDF exportado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar PDF:\n\n" + ex.Message + "\n\n" + ex.InnerException?.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable ConverterParaDataTable(List<RelatorioOsItem> itens)
        {
            var dt = new DataTable();
            dt.Columns.Add("ClienteNome", typeof(string));
            dt.Columns.Add("OsId", typeof(string));
            dt.Columns.Add("DataAbertura", typeof(DateTime));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("ValorTotal", typeof(decimal));
            dt.Columns.Add("TotalImpostos", typeof(decimal));

            foreach (var item in itens)
            {
                dt.Rows.Add(
                    item.ClienteNome,
                    item.OsId,
                    item.DataAbertura,
                    item.Status,
                    item.ValorTotal,
                    item.TotalImpostos);
            }

            return dt;
        }
    }
}
