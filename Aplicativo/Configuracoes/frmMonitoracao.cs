using System;
using System.Windows.Forms;
using pipeline_core;
using pipeline_core_ControlDBDevops;

namespace Configuracoes
{
    public partial class frmMonitoracao : Form
    {
        Configuracao configuracao = new Configuracao();


        public frmMonitoracao()
        {
            InitializeComponent();

            // Inicia controles
            atualizaComboBasesControladas();
            lblPendentes.Text = configuracao.aplicaScript;
            lblExecutados.Text = configuracao.scriptAplicado;
            lblBaseVersionadora.Text = pipeline_core_ControlDBDevops.Constantes.baseControladora;
        }

        private void atualizaDgvPendentes(String baseDados)
        {
            dgvPendentes.Rows.Clear();
            dgvPendentes.AllowUserToAddRows = false;
            dgvPendentes.ReadOnly = true;
            dgvPendentes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPendentes.ColumnCount = 3;
            
            dgvPendentes.Columns[0].Name = "Versao";
            dgvPendentes.Columns[1].Name = "Script";
            dgvPendentes.Columns[2].Name = "Caminho";

            dgvPendentes.Columns[0].HeaderText = "Versão";
            dgvPendentes.Columns[1].HeaderText = "Script";
            dgvPendentes.Columns[2].HeaderText = "Caminho";

            foreach (Scripts script in Scripts.GetScripts(new String[] { configuracao.aplicaScript }, baseDados))
                dgvPendentes.Rows.Add
                    (
                        script.info.versao,
                        script.nomeArquivo,
                        script.caminhoArquivo
                    );

            dgvPendentes.Refresh();
        }

        private void atualizaDgvExecutados(String baseDados)
        {
            dgvExecutados.Rows.Clear();
            dgvExecutados.AllowUserToAddRows = false;
            dgvExecutados.ReadOnly = true;
            dgvExecutados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvExecutados.ColumnCount = 3;

            dgvExecutados.Columns[0].Name = "Versao";
            dgvExecutados.Columns[1].Name = "Script";
            dgvExecutados.Columns[2].Name = "Caminho";

            dgvExecutados.Columns[0].HeaderText = "Versão";
            dgvExecutados.Columns[1].HeaderText = "Script";
            dgvExecutados.Columns[2].HeaderText = "Caminho";

            foreach (Scripts script in Scripts.GetScripts(new String[] { configuracao.scriptAplicado }, baseDados))
                dgvExecutados.Rows.Add
                    (
                        script.info.versao,
                        script.nomeArquivo,
                        script.caminhoArquivo
                    );

            dgvExecutados.Refresh();
        }

        private void atualizaDgvBaseVersionadora(String baseDados)
        {
            dgvBaseVersionadora.Rows.Clear();
            dgvBaseVersionadora.AllowUserToAddRows = false;
            dgvBaseVersionadora.ReadOnly = true;
            dgvBaseVersionadora.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBaseVersionadora.ColumnCount = 3;

            dgvBaseVersionadora.Columns[0].Name = "Versao";
            dgvBaseVersionadora.Columns[1].Name = "Script";
            dgvBaseVersionadora.Columns[2].Name = "Data_Hora";

            dgvBaseVersionadora.Columns[0].HeaderText = "Versão";
            dgvBaseVersionadora.Columns[1].HeaderText = "Script";
            dgvBaseVersionadora.Columns[2].HeaderText = "Data/Hora";

            foreach (ControlDBDevops.Info info in ControlDBDevops.Info.versoesPorBase(baseDados))
                dgvBaseVersionadora.Rows.Add
                    (
                        info.nuVersion,
                        info.nmFile,
                        info.dateTimeExecute.ToString()
                    );

            dgvBaseVersionadora.Refresh();
        }

        private void atualizaComboBasesControladas()
        {
            try
            {
                cmbBasesControladas.Items.Clear();
                cmbBasesControladas.DataSource = new BindingSource(Scripts.GetBases(new String[] { configuracao.scriptAplicado, configuracao.aplicaScript }), null);
                atualizaComboVersaoBasesControladas(cmbBasesControladas.SelectedValue.ToString());
                atualizaDgvPendentes(cmbBasesControladas.SelectedItem.ToString());
                atualizaDgvExecutados(cmbBasesControladas.SelectedItem.ToString());
                atualizaDgvBaseVersionadora(cmbBasesControladas.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERR: " + ex.Message, "Erro na Execução!",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void atualizaComboVersaoBasesControladas(String Indice)
        {
            cmbVersaoBase.Items.Clear();
            int[] versoes = Scripts.GetVersoesBases(Scripts.GetScripts(new String[] { configuracao.scriptAplicado, configuracao.aplicaScript }, Indice));
            for (int i = 0; i < versoes.Length; i++)
                cmbVersaoBase.Items.Add(versoes[i]);

            cmbVersaoBase.SelectedIndex = 0;
        }

        private void cmbBasesControladas_SelectedIndexChanged(object sender, EventArgs e)
        {
            atualizaComboVersaoBasesControladas(cmbBasesControladas.SelectedValue.ToString());
            atualizaDgvPendentes(cmbBasesControladas.SelectedItem.ToString());
            atualizaDgvExecutados(cmbBasesControladas.SelectedItem.ToString());
            atualizaDgvBaseVersionadora(cmbBasesControladas.SelectedItem.ToString());
        }
    }
}
