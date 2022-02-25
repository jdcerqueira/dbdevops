
namespace Configuracoes
{
    partial class frmConfiguracao
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPastaBaseVersionadora = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSenhaBase = new System.Windows.Forms.TextBox();
            this.txtUsuarioBase = new System.Windows.Forms.TextBox();
            this.txtBaseVersionadora = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.txtBranch = new System.Windows.Forms.TextBox();
            this.txtServidor = new System.Windows.Forms.TextBox();
            this.txtScriptsParaAplicar = new System.Windows.Forms.TextBox();
            this.txtScriptCompleto = new System.Windows.Forms.TextBox();
            this.txtScriptsAplicados = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabControleScripts = new System.Windows.Forms.TabControl();
            this.tabScriptsAplicar = new System.Windows.Forms.TabPage();
            this.dgvScriptsParaAplicar = new System.Windows.Forms.DataGridView();
            this.tabScriptsJaAplicados = new System.Windows.Forms.TabPage();
            this.dgvScriptsJaAplicados = new System.Windows.Forms.DataGridView();
            this.tabScriptCompleto = new System.Windows.Forms.TabPage();
            this.dgvScriptCompleto = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControleScripts.SuspendLayout();
            this.tabScriptsAplicar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScriptsParaAplicar)).BeginInit();
            this.tabScriptsJaAplicados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScriptsJaAplicados)).BeginInit();
            this.tabScriptCompleto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScriptCompleto)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtPastaBaseVersionadora);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtSenhaBase);
            this.groupBox1.Controls.Add(this.txtUsuarioBase);
            this.groupBox1.Controls.Add(this.txtBaseVersionadora);
            this.groupBox1.Controls.Add(this.txtLog);
            this.groupBox1.Controls.Add(this.txtBranch);
            this.groupBox1.Controls.Add(this.txtServidor);
            this.groupBox1.Controls.Add(this.txtScriptsParaAplicar);
            this.groupBox1.Controls.Add(this.txtScriptCompleto);
            this.groupBox1.Controls.Add(this.txtScriptsAplicados);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(374, 251);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtPastaBaseVersionadora
            // 
            this.txtPastaBaseVersionadora.Location = new System.Drawing.Point(109, 168);
            this.txtPastaBaseVersionadora.Name = "txtPastaBaseVersionadora";
            this.txtPastaBaseVersionadora.Size = new System.Drawing.Size(233, 20);
            this.txtPastaBaseVersionadora.TabIndex = 6;
            this.txtPastaBaseVersionadora.TextChanged += new System.EventHandler(this.txtPastaBaseVersionadora_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 171);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "Pasta da Base";
            // 
            // txtSenhaBase
            // 
            this.txtSenhaBase.Location = new System.Drawing.Point(109, 213);
            this.txtSenhaBase.Name = "txtSenhaBase";
            this.txtSenhaBase.PasswordChar = '*';
            this.txtSenhaBase.Size = new System.Drawing.Size(233, 20);
            this.txtSenhaBase.TabIndex = 8;
            this.txtSenhaBase.TextChanged += new System.EventHandler(this.txtSenhaBase_TextChanged);
            // 
            // txtUsuarioBase
            // 
            this.txtUsuarioBase.Location = new System.Drawing.Point(109, 191);
            this.txtUsuarioBase.Name = "txtUsuarioBase";
            this.txtUsuarioBase.Size = new System.Drawing.Size(233, 20);
            this.txtUsuarioBase.TabIndex = 7;
            this.txtUsuarioBase.TextChanged += new System.EventHandler(this.txtUsuarioBase_TextChanged);
            // 
            // txtBaseVersionadora
            // 
            this.txtBaseVersionadora.Location = new System.Drawing.Point(109, 145);
            this.txtBaseVersionadora.Name = "txtBaseVersionadora";
            this.txtBaseVersionadora.ReadOnly = true;
            this.txtBaseVersionadora.Size = new System.Drawing.Size(233, 20);
            this.txtBaseVersionadora.TabIndex = 6;
            this.txtBaseVersionadora.TabStop = false;
            this.txtBaseVersionadora.TextChanged += new System.EventHandler(this.txtBaseVersionadora_TextChanged);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(109, 123);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(233, 20);
            this.txtLog.TabIndex = 5;
            this.txtLog.TextChanged += new System.EventHandler(this.txtLog_TextChanged);
            // 
            // txtBranch
            // 
            this.txtBranch.Location = new System.Drawing.Point(109, 13);
            this.txtBranch.Name = "txtBranch";
            this.txtBranch.Size = new System.Drawing.Size(233, 20);
            this.txtBranch.TabIndex = 0;
            this.txtBranch.TextChanged += new System.EventHandler(this.txtBranch_TextChanged);
            // 
            // txtServidor
            // 
            this.txtServidor.Location = new System.Drawing.Point(109, 35);
            this.txtServidor.Name = "txtServidor";
            this.txtServidor.Size = new System.Drawing.Size(233, 20);
            this.txtServidor.TabIndex = 1;
            this.txtServidor.TextChanged += new System.EventHandler(this.txtServidor_TextChanged);
            // 
            // txtScriptsParaAplicar
            // 
            this.txtScriptsParaAplicar.Location = new System.Drawing.Point(109, 57);
            this.txtScriptsParaAplicar.Name = "txtScriptsParaAplicar";
            this.txtScriptsParaAplicar.Size = new System.Drawing.Size(233, 20);
            this.txtScriptsParaAplicar.TabIndex = 2;
            this.txtScriptsParaAplicar.TextChanged += new System.EventHandler(this.txtScriptsParaAplicar_TextChanged);
            // 
            // txtScriptCompleto
            // 
            this.txtScriptCompleto.Location = new System.Drawing.Point(109, 79);
            this.txtScriptCompleto.Name = "txtScriptCompleto";
            this.txtScriptCompleto.Size = new System.Drawing.Size(233, 20);
            this.txtScriptCompleto.TabIndex = 3;
            this.txtScriptCompleto.TextChanged += new System.EventHandler(this.txtScriptCompleto_TextChanged);
            // 
            // txtScriptsAplicados
            // 
            this.txtScriptsAplicados.Location = new System.Drawing.Point(109, 101);
            this.txtScriptsAplicados.Name = "txtScriptsAplicados";
            this.txtScriptsAplicados.Size = new System.Drawing.Size(233, 20);
            this.txtScriptsAplicados.TabIndex = 4;
            this.txtScriptsAplicados.TextChanged += new System.EventHandler(this.txtScriptsAplicados_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 216);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Senha base";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Usuário base";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Base versionadora";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Log";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 104);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Scripts já aplicados";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Script completo";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Scripts para aplicar";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Servidor";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Branch";
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(628, 270);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(75, 23);
            this.btnSalvar.TabIndex = 9;
            this.btnSalvar.Text = "&Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(713, 270);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 10;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tabControleScripts);
            this.groupBox2.Location = new System.Drawing.Point(414, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(374, 251);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // tabControleScripts
            // 
            this.tabControleScripts.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControleScripts.Controls.Add(this.tabScriptsAplicar);
            this.tabControleScripts.Controls.Add(this.tabScriptsJaAplicados);
            this.tabControleScripts.Controls.Add(this.tabScriptCompleto);
            this.tabControleScripts.Location = new System.Drawing.Point(6, 12);
            this.tabControleScripts.Multiline = true;
            this.tabControleScripts.Name = "tabControleScripts";
            this.tabControleScripts.SelectedIndex = 0;
            this.tabControleScripts.Size = new System.Drawing.Size(362, 233);
            this.tabControleScripts.TabIndex = 0;
            // 
            // tabScriptsAplicar
            // 
            this.tabScriptsAplicar.Controls.Add(this.dgvScriptsParaAplicar);
            this.tabScriptsAplicar.Location = new System.Drawing.Point(4, 4);
            this.tabScriptsAplicar.Name = "tabScriptsAplicar";
            this.tabScriptsAplicar.Padding = new System.Windows.Forms.Padding(3);
            this.tabScriptsAplicar.Size = new System.Drawing.Size(354, 207);
            this.tabScriptsAplicar.TabIndex = 0;
            this.tabScriptsAplicar.Text = "Scripts para Aplicar";
            this.tabScriptsAplicar.UseVisualStyleBackColor = true;
            // 
            // dgvScriptsParaAplicar
            // 
            this.dgvScriptsParaAplicar.AllowUserToAddRows = false;
            this.dgvScriptsParaAplicar.AllowUserToDeleteRows = false;
            this.dgvScriptsParaAplicar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScriptsParaAplicar.Location = new System.Drawing.Point(6, 6);
            this.dgvScriptsParaAplicar.Name = "dgvScriptsParaAplicar";
            this.dgvScriptsParaAplicar.ReadOnly = true;
            this.dgvScriptsParaAplicar.Size = new System.Drawing.Size(342, 195);
            this.dgvScriptsParaAplicar.TabIndex = 1;
            // 
            // tabScriptsJaAplicados
            // 
            this.tabScriptsJaAplicados.Controls.Add(this.dgvScriptsJaAplicados);
            this.tabScriptsJaAplicados.Location = new System.Drawing.Point(4, 4);
            this.tabScriptsJaAplicados.Name = "tabScriptsJaAplicados";
            this.tabScriptsJaAplicados.Padding = new System.Windows.Forms.Padding(3);
            this.tabScriptsJaAplicados.Size = new System.Drawing.Size(354, 207);
            this.tabScriptsJaAplicados.TabIndex = 2;
            this.tabScriptsJaAplicados.Text = "Scripts já Aplicados";
            this.tabScriptsJaAplicados.UseVisualStyleBackColor = true;
            // 
            // dgvScriptsJaAplicados
            // 
            this.dgvScriptsJaAplicados.AllowUserToAddRows = false;
            this.dgvScriptsJaAplicados.AllowUserToDeleteRows = false;
            this.dgvScriptsJaAplicados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScriptsJaAplicados.Location = new System.Drawing.Point(6, 6);
            this.dgvScriptsJaAplicados.Name = "dgvScriptsJaAplicados";
            this.dgvScriptsJaAplicados.ReadOnly = true;
            this.dgvScriptsJaAplicados.Size = new System.Drawing.Size(342, 195);
            this.dgvScriptsJaAplicados.TabIndex = 1;
            // 
            // tabScriptCompleto
            // 
            this.tabScriptCompleto.Controls.Add(this.dgvScriptCompleto);
            this.tabScriptCompleto.Location = new System.Drawing.Point(4, 4);
            this.tabScriptCompleto.Name = "tabScriptCompleto";
            this.tabScriptCompleto.Padding = new System.Windows.Forms.Padding(3);
            this.tabScriptCompleto.Size = new System.Drawing.Size(354, 207);
            this.tabScriptCompleto.TabIndex = 1;
            this.tabScriptCompleto.Text = "Script Completo";
            this.tabScriptCompleto.UseVisualStyleBackColor = true;
            // 
            // dgvScriptCompleto
            // 
            this.dgvScriptCompleto.AllowUserToAddRows = false;
            this.dgvScriptCompleto.AllowUserToDeleteRows = false;
            this.dgvScriptCompleto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScriptCompleto.Location = new System.Drawing.Point(6, 6);
            this.dgvScriptCompleto.Name = "dgvScriptCompleto";
            this.dgvScriptCompleto.ReadOnly = true;
            this.dgvScriptCompleto.Size = new System.Drawing.Size(342, 195);
            this.dgvScriptCompleto.TabIndex = 0;
            // 
            // frmConfiguracao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 302);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmConfiguracao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuração";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabControleScripts.ResumeLayout(false);
            this.tabScriptsAplicar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvScriptsParaAplicar)).EndInit();
            this.tabScriptsJaAplicados.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvScriptsJaAplicados)).EndInit();
            this.tabScriptCompleto.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvScriptCompleto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtSenhaBase;
        private System.Windows.Forms.TextBox txtUsuarioBase;
        private System.Windows.Forms.TextBox txtBaseVersionadora;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.TextBox txtBranch;
        private System.Windows.Forms.TextBox txtServidor;
        private System.Windows.Forms.TextBox txtScriptsParaAplicar;
        private System.Windows.Forms.TextBox txtScriptCompleto;
        private System.Windows.Forms.TextBox txtScriptsAplicados;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControleScripts;
        private System.Windows.Forms.TabPage tabScriptsAplicar;
        private System.Windows.Forms.TabPage tabScriptCompleto;
        private System.Windows.Forms.TabPage tabScriptsJaAplicados;
        private System.Windows.Forms.DataGridView dgvScriptsParaAplicar;
        private System.Windows.Forms.DataGridView dgvScriptCompleto;
        private System.Windows.Forms.DataGridView dgvScriptsJaAplicados;
        private System.Windows.Forms.TextBox txtPastaBaseVersionadora;
        private System.Windows.Forms.Label label10;
    }
}

