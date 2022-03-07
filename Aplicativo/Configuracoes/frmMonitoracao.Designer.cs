
namespace Configuracoes
{
    partial class frmMonitoracao
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbBasesControladas = new System.Windows.Forms.ComboBox();
            this.lblBases = new System.Windows.Forms.Label();
            this.cmbVersaoBase = new System.Windows.Forms.ComboBox();
            this.dgvPendentes = new System.Windows.Forms.DataGridView();
            this.dgvExecutados = new System.Windows.Forms.DataGridView();
            this.dgvBaseVersionadora = new System.Windows.Forms.DataGridView();
            this.lblPendentes = new System.Windows.Forms.Label();
            this.lblExecutados = new System.Windows.Forms.Label();
            this.lblBaseVersionadora = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendentes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExecutados)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaseVersionadora)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbBasesControladas
            // 
            this.cmbBasesControladas.FormattingEnabled = true;
            this.cmbBasesControladas.Location = new System.Drawing.Point(115, 12);
            this.cmbBasesControladas.Name = "cmbBasesControladas";
            this.cmbBasesControladas.Size = new System.Drawing.Size(211, 21);
            this.cmbBasesControladas.TabIndex = 0;
            this.cmbBasesControladas.SelectedIndexChanged += new System.EventHandler(this.cmbBasesControladas_SelectedIndexChanged);
            // 
            // lblBases
            // 
            this.lblBases.AutoSize = true;
            this.lblBases.Location = new System.Drawing.Point(11, 15);
            this.lblBases.Name = "lblBases";
            this.lblBases.Size = new System.Drawing.Size(98, 13);
            this.lblBases.TabIndex = 1;
            this.lblBases.Text = "Bases Controladas:";
            // 
            // cmbVersaoBase
            // 
            this.cmbVersaoBase.FormattingEnabled = true;
            this.cmbVersaoBase.Location = new System.Drawing.Point(332, 12);
            this.cmbVersaoBase.Name = "cmbVersaoBase";
            this.cmbVersaoBase.Size = new System.Drawing.Size(152, 21);
            this.cmbVersaoBase.TabIndex = 2;
            // 
            // dgvPendentes
            // 
            this.dgvPendentes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPendentes.Location = new System.Drawing.Point(12, 63);
            this.dgvPendentes.Name = "dgvPendentes";
            this.dgvPendentes.Size = new System.Drawing.Size(246, 214);
            this.dgvPendentes.TabIndex = 3;
            // 
            // dgvExecutados
            // 
            this.dgvExecutados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExecutados.Location = new System.Drawing.Point(264, 63);
            this.dgvExecutados.Name = "dgvExecutados";
            this.dgvExecutados.Size = new System.Drawing.Size(246, 214);
            this.dgvExecutados.TabIndex = 4;
            // 
            // dgvBaseVersionadora
            // 
            this.dgvBaseVersionadora.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBaseVersionadora.Location = new System.Drawing.Point(516, 63);
            this.dgvBaseVersionadora.Name = "dgvBaseVersionadora";
            this.dgvBaseVersionadora.Size = new System.Drawing.Size(246, 214);
            this.dgvBaseVersionadora.TabIndex = 5;
            // 
            // lblPendentes
            // 
            this.lblPendentes.AutoSize = true;
            this.lblPendentes.Location = new System.Drawing.Point(9, 47);
            this.lblPendentes.Name = "lblPendentes";
            this.lblPendentes.Size = new System.Drawing.Size(35, 13);
            this.lblPendentes.TabIndex = 6;
            this.lblPendentes.Text = "label1";
            // 
            // lblExecutados
            // 
            this.lblExecutados.AutoSize = true;
            this.lblExecutados.Location = new System.Drawing.Point(261, 47);
            this.lblExecutados.Name = "lblExecutados";
            this.lblExecutados.Size = new System.Drawing.Size(35, 13);
            this.lblExecutados.TabIndex = 7;
            this.lblExecutados.Text = "label2";
            // 
            // lblBaseVersionadora
            // 
            this.lblBaseVersionadora.AutoSize = true;
            this.lblBaseVersionadora.Location = new System.Drawing.Point(513, 47);
            this.lblBaseVersionadora.Name = "lblBaseVersionadora";
            this.lblBaseVersionadora.Size = new System.Drawing.Size(35, 13);
            this.lblBaseVersionadora.TabIndex = 8;
            this.lblBaseVersionadora.Text = "label3";
            // 
            // frmMonitoracao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 450);
            this.Controls.Add(this.lblBaseVersionadora);
            this.Controls.Add(this.lblExecutados);
            this.Controls.Add(this.lblPendentes);
            this.Controls.Add(this.dgvBaseVersionadora);
            this.Controls.Add(this.dgvExecutados);
            this.Controls.Add(this.dgvPendentes);
            this.Controls.Add(this.cmbVersaoBase);
            this.Controls.Add(this.lblBases);
            this.Controls.Add(this.cmbBasesControladas);
            this.Name = "frmMonitoracao";
            this.Text = "Monitoração";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendentes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExecutados)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaseVersionadora)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbBasesControladas;
        private System.Windows.Forms.Label lblBases;
        private System.Windows.Forms.ComboBox cmbVersaoBase;
        private System.Windows.Forms.DataGridView dgvPendentes;
        private System.Windows.Forms.DataGridView dgvExecutados;
        private System.Windows.Forms.DataGridView dgvBaseVersionadora;
        private System.Windows.Forms.Label lblPendentes;
        private System.Windows.Forms.Label lblExecutados;
        private System.Windows.Forms.Label lblBaseVersionadora;
    }
}