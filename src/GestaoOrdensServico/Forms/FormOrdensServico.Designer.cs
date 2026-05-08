namespace GestaoOrdensServico.Forms
{
    partial class FormOrdensServico
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPesquisar = new System.Windows.Forms.Button();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.dtpFim = new System.Windows.Forms.DateTimePicker();
            this.dtpInicio = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnProximo = new System.Windows.Forms.Button();
            this.lblPagina = new System.Windows.Forms.Label();
            this.btnAnterior = new System.Windows.Forms.Button();
            this.btnAbrir = new System.Windows.Forms.Button();
            this.btnNova = new System.Windows.Forms.Button();
            this.dgvOrdensServico = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdensServico)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnPesquisar);
            this.panel1.Controls.Add(this.cmbStatus);
            this.panel1.Controls.Add(this.dtpFim);
            this.panel1.Controls.Add(this.dtpInicio);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(884, 55);
            this.panel1.TabIndex = 0;
            // 
            // btnPesquisar
            // 
            this.btnPesquisar.Location = new System.Drawing.Point(550, 13);
            this.btnPesquisar.Name = "btnPesquisar";
            this.btnPesquisar.Size = new System.Drawing.Size(100, 28);
            this.btnPesquisar.TabIndex = 3;
            this.btnPesquisar.Text = "Pesquisar";
            this.btnPesquisar.UseVisualStyleBackColor = true;
            this.btnPesquisar.Click += new System.EventHandler(this.btnPesquisar_Click);
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(405, 15);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(121, 21);
            this.cmbStatus.TabIndex = 2;
            // 
            // dtpFim
            // 
            this.dtpFim.Location = new System.Drawing.Point(210, 15);
            this.dtpFim.Name = "dtpFim";
            this.dtpFim.Size = new System.Drawing.Size(140, 20);
            this.dtpFim.TabIndex = 1;
            // 
            // dtpInicio
            // 
            this.dtpInicio.Location = new System.Drawing.Point(35, 15);
            this.dtpInicio.Name = "dtpInicio";
            this.dtpInicio.Size = new System.Drawing.Size(140, 20);
            this.dtpInicio.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnProximo);
            this.panel2.Controls.Add(this.lblPagina);
            this.panel2.Controls.Add(this.btnAnterior);
            this.panel2.Controls.Add(this.btnAbrir);
            this.panel2.Controls.Add(this.btnNova);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 411);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(884, 50);
            this.panel2.TabIndex = 1;
            // 
            // btnProximo
            // 
            this.btnProximo.Location = new System.Drawing.Point(620, 10);
            this.btnProximo.Name = "btnProximo";
            this.btnProximo.Size = new System.Drawing.Size(90, 30);
            this.btnProximo.TabIndex = 4;
            this.btnProximo.Text = "Próximo >";
            this.btnProximo.UseVisualStyleBackColor = true;
            this.btnProximo.Click += new System.EventHandler(this.btnProximo_Click);
            // 
            // lblPagina
            // 
            this.lblPagina.AutoSize = true;
            this.lblPagina.Location = new System.Drawing.Point(500, 15);
            this.lblPagina.Name = "lblPagina";
            this.lblPagina.Size = new System.Drawing.Size(49, 13);
            this.lblPagina.TabIndex = 3;
            this.lblPagina.Text = "Página 1";
            // 
            // btnAnterior
            // 
            this.btnAnterior.Location = new System.Drawing.Point(400, 10);
            this.btnAnterior.Name = "btnAnterior";
            this.btnAnterior.Size = new System.Drawing.Size(90, 30);
            this.btnAnterior.TabIndex = 2;
            this.btnAnterior.Text = "< Anterior";
            this.btnAnterior.UseVisualStyleBackColor = true;
            this.btnAnterior.Click += new System.EventHandler(this.btnAnterior_Click);
            // 
            // btnAbrir
            // 
            this.btnAbrir.Location = new System.Drawing.Point(112, 10);
            this.btnAbrir.Name = "btnAbrir";
            this.btnAbrir.Size = new System.Drawing.Size(90, 30);
            this.btnAbrir.TabIndex = 1;
            this.btnAbrir.Text = "Abrir";
            this.btnAbrir.UseVisualStyleBackColor = true;
            this.btnAbrir.Click += new System.EventHandler(this.btnAbrir_Click);
            // 
            // btnNova
            // 
            this.btnNova.Location = new System.Drawing.Point(12, 10);
            this.btnNova.Name = "btnNova";
            this.btnNova.Size = new System.Drawing.Size(90, 30);
            this.btnNova.TabIndex = 0;
            this.btnNova.Text = "Nova OS";
            this.btnNova.UseVisualStyleBackColor = true;
            this.btnNova.Click += new System.EventHandler(this.btnNova_Click);
            // 
            // dgvOrdensServico
            // 
            this.dgvOrdensServico.AllowUserToAddRows = false;
            this.dgvOrdensServico.AllowUserToDeleteRows = false;
            this.dgvOrdensServico.BackgroundColor = System.Drawing.Color.White;
            this.dgvOrdensServico.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvOrdensServico.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrdensServico.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrdensServico.Location = new System.Drawing.Point(0, 55);
            this.dgvOrdensServico.Name = "dgvOrdensServico";
            this.dgvOrdensServico.ReadOnly = true;
            this.dgvOrdensServico.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrdensServico.Size = new System.Drawing.Size(884, 356);
            this.dgvOrdensServico.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "De:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Até:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(360, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Status:";
            // 
            // FormOrdensServico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.dgvOrdensServico);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FormOrdensServico";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ordens de Serviço";
            this.Load += new System.EventHandler(this.FormOrdensServico_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdensServico)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPesquisar;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.DateTimePicker dtpFim;
        private System.Windows.Forms.DateTimePicker dtpInicio;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvOrdensServico;
        private System.Windows.Forms.Button btnProximo;
        private System.Windows.Forms.Label lblPagina;
        private System.Windows.Forms.Button btnAnterior;
        private System.Windows.Forms.Button btnAbrir;
        private System.Windows.Forms.Button btnNova;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
