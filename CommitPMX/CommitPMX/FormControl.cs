using PEPExtensions;
using PEPlugin;
using PEPlugin.Pmx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommitPMX
{
    public partial class FormControl : Form
    {
        IPERunArgs Args { get; }
        IPXPmx Pmx { get; set; }

        public FormControl(IPERunArgs args)
        {
            Args = args;

            InitializeComponent();
            Reload();
        }

        internal void Reload()
        {
            //Pmx = Args.Host.Connector.Pmx.GetCurrentState();
        }

        private void textBoxCommitComment_TextChanged(object sender, EventArgs e)
        {
            buttonCommit.Enabled = !string.IsNullOrEmpty(textBoxCommitComment.Text);

            // パス文字数制限があるのでとりあえずコメントは144文字制限にしておく
            if (textBoxCommitComment.Text.Length > 144)
                textBoxCommitComment.Text = textBoxCommitComment.Text.Substring(0, 144);
        }

        private void checkBoxAmend_CheckedChanged(object sender, EventArgs e)
        {
            // TODO: 修正処理
        }

        private void buttonCommit_Click(object sender, EventArgs e)
        {
            new Commit(Args.Host.Connector.Pmx.GetCurrentState(), Args.Host.Connector.Form, textBoxCommitComment.Text).Invoke();
            textBoxCommitComment.Clear();
        }
    }
}
