namespace GestaoOrdensServico.Forms
{
    partial class FormPrincipal
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
            this.btnClientes = new System.Windows.Forms.Button();
            this.btnOrdensServico = new System.Windows.Forms.Button();
            this.btnServicos = new System.Windows.Forms.Button();
            this.btnRelatorio = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnClientes
            // 
            this.btnClientes.Location = new System.Drawing.Point(49, 60);
            this.btnClientes.Name = "btnClientes";
            this.btnClientes.Size = new System.Drawing.Size(75, 23);
            this.btnClientes.TabIndex = 0;
            this.btnClientes.Text = "Clientes";
            this.btnClientes.UseVisualStyleBackColor = true;
            this.btnClientes.Click += new System.EventHandler(this.btnClientes_Click);
            // 
            // btnOrdensServico
            // 
            this.btnOrdensServico.Location = new System.Drawing.Point(179, 60);
            this.btnOrdensServico.Name = "btnOrdensServico";
            this.btnOrdensServico.Size = new System.Drawing.Size(121, 23);
            this.btnOrdensServico.TabIndex = 1;
            this.btnOrdensServico.Text = "Ordens de Serviço";
            this.btnOrdensServico.UseVisualStyleBackColor = true;
            this.btnOrdensServico.Click += new System.EventHandler(this.btnOrdensServico_Click);
            // 
            // btnServicos
            // 
            this.btnServicos.Location = new System.Drawing.Point(49, 112);
            this.btnServicos.Name = "btnServicos";
            this.btnServicos.Size = new System.Drawing.Size(75, 23);
            this.btnServicos.TabIndex = 2;
            this.btnServicos.Text = "Serviços";
            this.btnServicos.UseVisualStyleBackColor = true;
            this.btnServicos.Click += new System.EventHandler(this.btnServicos_Click);
            // 
            // btnRelatorio
            // 
            this.btnRelatorio.Location = new System.Drawing.Point(194, 112);
            this.btnRelatorio.Name = "btnRelatorio";
            this.btnRelatorio.Size = new System.Drawing.Size(75, 23);
            this.btnRelatorio.TabIndex = 3;
            this.btnRelatorio.Text = "Relatório";
            this.btnRelatorio.UseVisualStyleBackColor = true;
            this.btnRelatorio.Click += new System.EventHandler(this.btnRelatorio_Click);
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.btnRelatorio);
            this.Controls.Add(this.btnServicos);
            this.Controls.Add(this.btnOrdensServico);
            this.Controls.Add(this.btnClientes);
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestão de Ordens de Serviço";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button btnClientes;
        private System.Windows.Forms.Button btnOrdensServico;
        private System.Windows.Forms.Button btnServicos;
        private System.Windows.Forms.Button btnRelatorio;
    }
}
