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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnClientes
            // 
            this.btnClientes.Location = new System.Drawing.Point(75, 134);
            this.btnClientes.Name = "btnClientes";
            this.btnClientes.Size = new System.Drawing.Size(160, 60);
            this.btnClientes.TabIndex = 0;
            this.btnClientes.Text = "Clientes";
            this.btnClientes.UseVisualStyleBackColor = true;
            this.btnClientes.Click += new System.EventHandler(this.btnClientes_Click);
            // 
            // btnOrdensServico
            // 
            this.btnOrdensServico.Location = new System.Drawing.Point(241, 134);
            this.btnOrdensServico.Name = "btnOrdensServico";
            this.btnOrdensServico.Size = new System.Drawing.Size(160, 60);
            this.btnOrdensServico.TabIndex = 1;
            this.btnOrdensServico.Text = "Ordens de Serviço";
            this.btnOrdensServico.UseVisualStyleBackColor = true;
            this.btnOrdensServico.Click += new System.EventHandler(this.btnOrdensServico_Click);
            // 
            // btnServicos
            // 
            this.btnServicos.Location = new System.Drawing.Point(75, 200);
            this.btnServicos.Name = "btnServicos";
            this.btnServicos.Size = new System.Drawing.Size(160, 60);
            this.btnServicos.TabIndex = 2;
            this.btnServicos.Text = "Serviços";
            this.btnServicos.UseVisualStyleBackColor = true;
            this.btnServicos.Click += new System.EventHandler(this.btnServicos_Click);
            // 
            // btnRelatorio
            // 
            this.btnRelatorio.Location = new System.Drawing.Point(241, 200);
            this.btnRelatorio.Name = "btnRelatorio";
            this.btnRelatorio.Size = new System.Drawing.Size(160, 60);
            this.btnRelatorio.TabIndex = 3;
            this.btnRelatorio.Text = "Relatório";
            this.btnRelatorio.UseVisualStyleBackColor = true;
            this.btnRelatorio.Click += new System.EventHandler(this.btnRelatorio_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(79, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(322, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Gestão de Ordens de Serviço";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label2.Location = new System.Drawing.Point(131, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Selecione um módulo para começar";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRelatorio);
            this.Controls.Add(this.btnServicos);
            this.Controls.Add(this.btnOrdensServico);
            this.Controls.Add(this.btnClientes);
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema de Gestão de Ordens de Serviço";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnClientes;
        private System.Windows.Forms.Button btnOrdensServico;
        private System.Windows.Forms.Button btnServicos;
        private System.Windows.Forms.Button btnRelatorio;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
