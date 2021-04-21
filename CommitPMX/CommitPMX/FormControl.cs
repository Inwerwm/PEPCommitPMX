using PEPlugin;
using PEPlugin.Pmx;
using System;
using System.Windows.Forms;

namespace CommitPMX
{
    public partial class FormControl : Form
    {
        // gitのコメントは50文字以内推奨らしいので
        private readonly int COMMENT_LIMIT = 50;

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

            if (textBoxCommitComment.Text.Length > COMMENT_LIMIT)
                textBoxCommitComment.Text = textBoxCommitComment.Text.Substring(0, COMMENT_LIMIT);
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

        private void textBoxCommitComment_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (buttonCommit.Enabled && e.KeyChar == '\n')
                buttonCommit_Click(sender, e);
        }
    }
}
