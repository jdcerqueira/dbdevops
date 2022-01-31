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

        private void verificaBotaoSalvar()
        {
            btnSalvar.Enabled = acao;
            btnCancelar.Enabled = acao;
        }

        public void carregaConfiguracao()
        {
            // Carrega dados de configuração nos campos
            config = new Configuracao();

            txtBranch.Text = config.branch;
            txtServidor.Text = config.connection;
            txtScriptsParaAplicar.Text = config.aplicaScript;
            txtScriptCompleto.Text = config.scriptCompleto;
            txtScriptsAplicados.Text = config.scriptAplicado;
            txtLog.Text = config.log;
            txtBaseVersionadora.Text = config.baseControladora;
            txtUsuarioBase.Text = config.usuarioBase;
            txtSenhaBase.Text = config.senhaBase;
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
                txtSenhaBase.Text);

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
    }
}
