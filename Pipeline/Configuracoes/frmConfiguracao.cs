using pipeline_config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Configuracoes
{
    public partial class frmConfiguracao : Form
    {

        public Boolean acao;
        Configuracao config;

        public frmConfiguracao()
        {
            InitializeComponent();
            carregaConfiguracao();
            acao = false;
            verificaBotaoSalvar();
        }

        private void carregaDataGridViewScriptsJaAplicados()
        {
            dgvScriptsJaAplicados.ColumnCount = 1;
            dgvScriptsJaAplicados.Rows.Clear();
            dgvScriptsJaAplicados.Columns[0].HeaderText = "Scripts";
            dgvScriptsJaAplicados.Columns[0].Name = "Scripts";
            dgvScriptsJaAplicados.AllowUserToAddRows = false;
            dgvScriptsJaAplicados.ReadOnly = true;

            //foreach (Scripts item in config.arquivosScriptsAplicados.scripts)
            //    dgvScriptsJaAplicados.Rows.Add(item.nomeArquivo);

            dgvScriptsJaAplicados.Refresh();
        }

        private void carregaDataGridViewScriptsParaAplicar()
        {
            dgvScriptsParaAplicar.ColumnCount = 1;
            dgvScriptsParaAplicar.Rows.Clear();
            dgvScriptsParaAplicar.Columns[0].HeaderText = "Scripts";
            dgvScriptsParaAplicar.Columns[0].Name = "Scripts";
            dgvScriptsParaAplicar.AllowUserToAddRows = false;
            dgvScriptsParaAplicar.ReadOnly = true;

            //foreach (Scripts item in config.arquivosScriptsParaAplicar.scripts)
            //    dgvScriptsParaAplicar.Rows.Add(item.nomeArquivo);

            dgvScriptsParaAplicar.Refresh();
        }

        private void carregaDataGridViewScriptCompleto()
        {
            dgvScriptCompleto.ColumnCount = 1;
            dgvScriptCompleto.Rows.Clear();
            dgvScriptCompleto.Columns[0].HeaderText = "Scripts";
            dgvScriptCompleto.Columns[0].Name = "Scripts";
            dgvScriptCompleto.AllowUserToAddRows = false;
            dgvScriptCompleto.ReadOnly = true;

            //foreach (Scripts item in config.arquivoScriptCompleto.scripts)
            //    dgvScriptCompleto.Rows.Add(item.nomeArquivo);

            dgvScriptCompleto.Refresh();
        }

        private void verificaBotaoSalvar()
        {
            btnSalvar.Enabled = acao;
            btnCancelar.Enabled = acao;
        }

        public void carregaConfiguracao()
        {
            // Carrega dados de configuração nos campos
            try
            {
                config = new Configuracao();

                txtBranch.Text = config.branch;
                txtServidor.Text = config.connection;
                txtScriptsParaAplicar.Text = config.aplicaScript;
                txtScriptCompleto.Text = config.scriptCompleto;
                txtScriptsAplicados.Text = config.scriptAplicado;
                txtLog.Text = config.log;
                txtBaseVersionadora.Text = Configuracao.baseControladora;
                txtUsuarioBase.Text = config.usuarioBase;
                txtSenhaBase.Text = config.senhaBase;
                txtPastaBaseVersionadora.Text = config.pastaBaseVersionadora;

                carregaDataGridViewScriptsJaAplicados();
                carregaDataGridViewScriptsParaAplicar();
                carregaDataGridViewScriptCompleto();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao carregar as configurações." + ex.Message);
            }
            
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Configuracao.salvarConfiguracao
                (txtBranch.Text,
                txtServidor.Text,
                txtScriptsParaAplicar.Text,
                txtScriptCompleto.Text,
                txtScriptsAplicados.Text,
                txtLog.Text,
                txtBaseVersionadora.Text,
                txtUsuarioBase.Text,
                txtSenhaBase.Text,
                txtPastaBaseVersionadora.Text);

            carregaConfiguracao();
            acao = false;
            verificaBotaoSalvar();

            MessageBox.Show("Registro atualizado com sucesso.","Salvar configuração",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void txtBranch_TextChanged(object sender, EventArgs e)
        {
            acao = true;
            verificaBotaoSalvar();
        }

        private void txtServidor_TextChanged(object sender, EventArgs e)
        {
            acao = true;
            verificaBotaoSalvar();
        }

        private void txtScriptsParaAplicar_TextChanged(object sender, EventArgs e)
        {
            acao = true;
            verificaBotaoSalvar();
        }

        private void txtScriptCompleto_TextChanged(object sender, EventArgs e)
        {
            acao = true;
            verificaBotaoSalvar();
        }

        private void txtScriptsAplicados_TextChanged(object sender, EventArgs e)
        {
            acao = true;
            verificaBotaoSalvar();
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            acao = true;
            verificaBotaoSalvar();
        }

        private void txtBaseVersionadora_TextChanged(object sender, EventArgs e)
        {
            acao = true;
            verificaBotaoSalvar();
        }

        private void txtUsuarioBase_TextChanged(object sender, EventArgs e)
        {
            acao = true;
            verificaBotaoSalvar();
        }

        private void txtSenhaBase_TextChanged(object sender, EventArgs e)
        {
            acao = true;
            verificaBotaoSalvar();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            carregaConfiguracao();
            acao = false;
            verificaBotaoSalvar();
        }

        private void txtPastaBaseVersionadora_TextChanged(object sender, EventArgs e)
        {
            acao = true;
            verificaBotaoSalvar();
        }
    }
}
