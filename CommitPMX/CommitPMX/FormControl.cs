using PEPlugin;
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
        SevenZip.OutArchiveFormat ArchiveFormat
        {
            get => Properties.Settings.Default.ArchiveFormat;
            set
            {
                Properties.Settings.Default.ArchiveFormat = value;
                Properties.Settings.Default.Save();
                SyncFormatSelection();
            }
        }

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

        private void SyncFormatSelection()
        {
            RadioButton correspondingButton = null;

            switch (ArchiveFormat)
            {
                case SevenZip.OutArchiveFormat.SevenZip:
                    correspondingButton = radioButton7z;
                    break;
                case SevenZip.OutArchiveFormat.Zip:
                    correspondingButton = radioButtonZip;
                    break;
                //case SevenZip.OutArchiveFormat.GZip:
                //    break;
                //case SevenZip.OutArchiveFormat.BZip2:
                //    break;
                //case SevenZip.OutArchiveFormat.Tar:
                //    break;
                //case SevenZip.OutArchiveFormat.XZ:
                //    break;
                default:
                    ArchiveFormat = SevenZip.OutArchiveFormat.SevenZip;
                    new NotImplementedException($"対応していない圧縮形式が選択されました。{Environment.NewLine}規定の方式に変更しました。");
                    break;
            }

            if (!correspondingButton.Checked)
                correspondingButton.Checked = true;
        }

        private void FormControl_Load(object sender, EventArgs e)
        {
            SyncFormatSelection();
        }

        private void textBoxMessage_TextChanged(object sender, EventArgs e)
        {
            // テキストの内容をプログラム側で操作するとカーソル位置が最初に戻ってしまう
            // それを戻すためにカーソル位置を保存して最後に戻す処理を行う
            var selectionTmp = textBoxMessage.SelectionStart;

            buttonCommit.Enabled = !string.IsNullOrEmpty(textBoxMessage.Text);

            if (textBoxMessage.Text.Length > MESSAGE_LIMIT)
                textBoxMessage.Text = textBoxMessage.Text.Substring(0, MESSAGE_LIMIT);

            if (textBoxMessage.Text.Contains(Environment.NewLine))
            {
                // 改行文字分カーソル位置を戻す
                selectionTmp -= 2;
                textBoxMessage.Text = textBoxMessage.Text.Replace(Environment.NewLine, "");
            }

            // 文字キーとエンターキーを同時押しされるとselectionTmpが負になる場合がある
            textBoxMessage.SelectionStart = Math.Max(selectionTmp, 0);
        }

        private void buttonCommit_Click(object sender, EventArgs e)
        {
            var compressor = new SevenZipCompressor(Path.Combine(Path.GetDirectoryName(Args.ModulePath), "7z.dll"), ArchiveFormat);
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

        private void buttonReCompress_Click(object sender, EventArgs e)
        {
            string commitDir = Commit.BuildCommitDirectryPath(Args.Host.Connector.Pmx.CurrentPath);
            string archivePath = Path.Combine(commitDir, Commit.ArchiveName);
            var compressor = new SevenZipCompressor(Path.Combine(Path.GetDirectoryName(Args.ModulePath), "7z.dll"), ArchiveFormat);

            if (File.Exists(archivePath + compressor.ExtString))
            {
                compressor.ReCompress(archivePath);
            }
            else
            {
                MessageBox.Show($"アーカイブファイルが見つかりませんでした。{Environment.NewLine}期待されたパス:{archivePath + compressor.ExtString}");
            }
        }

        private void radioButton7z_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                ArchiveFormat = SevenZip.OutArchiveFormat.SevenZip;
        }

        private void radioButtonZip_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                ArchiveFormat = SevenZip.OutArchiveFormat.Zip;
        }
    }
}
