namespace GestaoOrdensServico.Forms
{
    partial class FormOrdensServicoCadastro
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCliente = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtObservacao = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblValorTotal = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtQuantidade = new System.Windows.Forms.TextBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnAlterarStatus = new System.Windows.Forms.Button();
            this.cmbNovoStatus = new System.Windows.Forms.ComboBox();
            this.btnRemoverItem = new System.Windows.Forms.Button();
            this.btnAdicionarItem = new System.Windows.Forms.Button();
            this.cmbServico = new System.Windows.Forms.ComboBox();
            this.dgvItens = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cliente";
            // 
            // cmbCliente
            // 
            this.cmbCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCliente.FormattingEnabled = true;
            this.cmbCliente.Location = new System.Drawing.Point(65, 12);
            this.cmbCliente.Name = "cmbCliente";
            this.cmbCliente.Size = new System.Drawing.Size(200, 21);
            this.cmbCliente.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(280, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Observação";
            // 
            // txtObservacao
            // 
            this.txtObservacao.Location = new System.Drawing.Point(355, 12);
            this.txtObservacao.Name = "txtObservacao";
            this.txtObservacao.Size = new System.Drawing.Size(200, 20);
            this.txtObservacao.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(65, 45);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(53, 16);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Aberta";
            // 
            // lblValorTotal
            // 
            this.lblValorTotal.AutoSize = true;
            this.lblValorTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblValorTotal.Location = new System.Drawing.Point(315, 45);
            this.lblValorTotal.Name = "lblValorTotal";
            this.lblValorTotal.Size = new System.Drawing.Size(58, 16);
            this.lblValorTotal.TabIndex = 5;
            this.lblValorTotal.Text = "R$ 0,00";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cmbCliente);
            this.panel1.Controls.Add(this.lblValorTotal);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtObservacao);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(834, 80);
            this.panel1.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtQuantidade);
            this.panel2.Controls.Add(this.btnCancelar);
            this.panel2.Controls.Add(this.btnSalvar);
            this.panel2.Controls.Add(this.btnAlterarStatus);
            this.panel2.Controls.Add(this.cmbNovoStatus);
            this.panel2.Controls.Add(this.btnRemoverItem);
            this.panel2.Controls.Add(this.btnAdicionarItem);
            this.panel2.Controls.Add(this.cmbServico);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 471);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(834, 90);
            this.panel2.TabIndex = 7;
            // 
            // txtQuantidade
            // 
            this.txtQuantidade.Location = new System.Drawing.Point(248, 9);
            this.txtQuantidade.Name = "txtQuantidade";
            this.txtQuantidade.Size = new System.Drawing.Size(60, 20);
            this.txtQuantidade.TabIndex = 13;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(690, 20);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 32);
            this.btnCancelar.TabIndex = 12;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(580, 20);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(100, 32);
            this.btnSalvar.TabIndex = 11;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // btnAlterarStatus
            // 
            this.btnAlterarStatus.Location = new System.Drawing.Point(240, 43);
            this.btnAlterarStatus.Name = "btnAlterarStatus";
            this.btnAlterarStatus.Size = new System.Drawing.Size(110, 28);
            this.btnAlterarStatus.TabIndex = 10;
            this.btnAlterarStatus.Text = "Alterar Status";
            this.btnAlterarStatus.UseVisualStyleBackColor = true;
            this.btnAlterarStatus.Click += new System.EventHandler(this.btnAlterarStatus_Click);
            // 
            // cmbNovoStatus
            // 
            this.cmbNovoStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNovoStatus.FormattingEnabled = true;
            this.cmbNovoStatus.Location = new System.Drawing.Point(100, 45);
            this.cmbNovoStatus.Name = "cmbNovoStatus";
            this.cmbNovoStatus.Size = new System.Drawing.Size(130, 21);
            this.cmbNovoStatus.TabIndex = 9;
            // 
            // btnRemoverItem
            // 
            this.btnRemoverItem.Enabled = false;
            this.btnRemoverItem.Location = new System.Drawing.Point(420, 7);
            this.btnRemoverItem.Name = "btnRemoverItem";
            this.btnRemoverItem.Size = new System.Drawing.Size(90, 28);
            this.btnRemoverItem.TabIndex = 8;
            this.btnRemoverItem.Text = "Remover";
            this.btnRemoverItem.UseVisualStyleBackColor = true;
            this.btnRemoverItem.Click += new System.EventHandler(this.btnRemoverItem_Click);
            // 
            // btnAdicionarItem
            // 
            this.btnAdicionarItem.Location = new System.Drawing.Point(320, 7);
            this.btnAdicionarItem.Name = "btnAdicionarItem";
            this.btnAdicionarItem.Size = new System.Drawing.Size(90, 28);
            this.btnAdicionarItem.TabIndex = 7;
            this.btnAdicionarItem.Text = "Adicionar";
            this.btnAdicionarItem.UseVisualStyleBackColor = true;
            this.btnAdicionarItem.Click += new System.EventHandler(this.btnAdicionarItem_Click);
            // 
            // cmbServico
            // 
            this.cmbServico.FormattingEnabled = true;
            this.cmbServico.Location = new System.Drawing.Point(60, 9);
            this.cmbServico.Name = "cmbServico";
            this.cmbServico.Size = new System.Drawing.Size(92, 21);
            this.cmbServico.TabIndex = 2;
            // 
            // dgvItens
            // 
            this.dgvItens.AllowUserToAddRows = false;
            this.dgvItens.AllowUserToDeleteRows = false;
            this.dgvItens.BackgroundColor = System.Drawing.Color.White;
            this.dgvItens.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvItens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItens.Location = new System.Drawing.Point(0, 80);
            this.dgvItens.Name = "dgvItens";
            this.dgvItens.ReadOnly = true;
            this.dgvItens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItens.Size = new System.Drawing.Size(834, 391);
            this.dgvItens.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Status:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(280, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Total:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Serviço:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(220, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Qtd:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Alterar Status:";
            // 
            // FormOrdensServicoCadastro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(834, 561);
            this.Controls.Add(this.dgvItens);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FormOrdensServicoCadastro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ordem de Serviço";
            this.Load += new System.EventHandler(this.FormOrdensServicoCadastro_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCliente;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtObservacao;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblValorTotal;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvItens;
        private System.Windows.Forms.ComboBox cmbServico;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnAlterarStatus;
        private System.Windows.Forms.ComboBox cmbNovoStatus;
        private System.Windows.Forms.Button btnRemoverItem;
        private System.Windows.Forms.Button btnAdicionarItem;
        private System.Windows.Forms.TextBox txtQuantidade;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
    }
}
