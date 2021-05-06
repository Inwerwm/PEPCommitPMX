using PEPlugin;
using PEPlugin.Pmx;
using System;
using System.IO;
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
            labelMessage.Text = $"メッセージ({MESSAGE_LIMIT}文字以内)  Ctrl+Enterでコミット";
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

            if(textBoxMessage.Text.Contains(Environment.NewLine))
            {
                // 改行文字分カーソル位置を戻す
                selectionTmp -= 2;
                textBoxMessage.Text = textBoxMessage.Text.Replace(Environment.NewLine, "");
            }

            // 文字キーとエンターキーを同時押しされるとselectionTmpが負になる場合がある
            textBoxMessage.SelectionStart = Math.Max(selectionTmp, 0);
        }

        private void checkBoxAmend_CheckedChanged(object sender, EventArgs e)
        {
            // TODO: 修正処理
        }

        private void buttonCommit_Click(object sender, EventArgs e)
        {
            var compressor = new SevenZipCompressor(Path.Combine(Path.GetDirectoryName(Args.ModulePath), "7z.dll"), SevenZip.OutArchiveFormat.SevenZip);
            new Commit(Args.Host.Connector.Pmx.GetCurrentState(), Args.Host.Connector.Form, textBoxMessage.Text, compressor).Invoke();
            textBoxMessage.Clear();
        }

        private void textBoxMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (buttonCommit.Enabled && e.KeyChar == '\n')
                buttonCommit_Click(sender, e);
        }

        private void FormControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
