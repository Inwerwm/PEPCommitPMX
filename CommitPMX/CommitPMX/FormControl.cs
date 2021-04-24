using PEPlugin;
using PEPlugin.Pmx;
using System;
using System.Windows.Forms;

namespace CommitPMX
{
    public partial class FormControl : Form
    {
        // gitのコメントは50文字以内推奨らしいので
        private readonly int MESSAGE_LIMIT = 50;

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

        private void textBoxMessage_TextChanged(object sender, EventArgs e)
        {
            // テキストの内容をプログラム側で操作するとカーソル位置が最初に戻ってしまう
            // それを戻すためにカーソル位置を保存して最後に戻す処理を行う
            var selectionTmp = textBoxMessage.SelectionStart;

            buttonCommit.Enabled = !string.IsNullOrEmpty(textBoxMessage.Text);
            
            if (textBoxMessage.Text.Length > MESSAGE_LIMIT)
                textBoxMessage.Text = textBoxMessage.Text.Substring(0, MESSAGE_LIMIT);

            textBoxMessage.SelectionStart = selectionTmp;
        }

        private void checkBoxAmend_CheckedChanged(object sender, EventArgs e)
        {
            // TODO: 修正処理
        }

        private void buttonCommit_Click(object sender, EventArgs e)
        {
            new Commit(Args.Host.Connector.Pmx.GetCurrentState(), Args.Host.Connector.Form, textBoxMessage.Text).Invoke();
            textBoxMessage.Clear();
        }

        private void textBoxMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (buttonCommit.Enabled && e.KeyChar == '\n')
                buttonCommit_Click(sender, e);
        }
    }
}
