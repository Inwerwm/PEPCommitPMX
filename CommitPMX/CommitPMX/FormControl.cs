using PEPlugin;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommitPMX
{
    public partial class FormControl : Form
    {
        // gitのメッセージは50文字以内推奨らしいので
        private readonly int MESSAGE_LIMIT = 50;

        IPERunArgs Args { get; }

        SevenZipCompressor Compressor { get; set; }
        LogArchive LogArchive { get; set; }

        SevenZip.OutArchiveFormat ArchiveFormat
        {
            get => Properties.Settings.Default.ArchiveFormat;
            set
            {
                Properties.Settings.Default.ArchiveFormat = value;
                Properties.Settings.Default.Save();
                Reload();
                SyncFormatSelection();
            }
        }

        private string DefaultDescription { get; }

        public FormControl(IPERunArgs args)
        {
            Args = args;

            InitializeComponent();
            labelMessage.Text = $"メッセージ({MESSAGE_LIMIT}文字以内)  Ctrl+Enterでコミット";
            DefaultDescription = textBoxDescription.Text;
            Reload();
        }

        private void SetControlesEnable(bool enable, Button runningButton)
        {
            textBoxMessage.ReadOnly = !enable;
            buttonCommit.Enabled = enable && !string.IsNullOrEmpty(textBoxMessage.Text);
            buttonReCompress.Enabled = enable;
            buttonReconstruction.Enabled = enable;

            if (runningButton is null)
                return;

            const string progressText = "中...";
            runningButton.Text = enable ?
                                 runningButton.Text.Replace(progressText, "") :
                                 runningButton.Text + progressText;
        }

        internal void Reload()
        {
            Compressor = new SevenZipCompressor(Path.Combine(Path.GetDirectoryName(Args.ModulePath), "7z.cdll"), ArchiveFormat);
            LogArchive = new LogArchive(Args.Host.Connector.Pmx.CurrentPath, Compressor);
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

            buttonCommit.Enabled = !(string.IsNullOrEmpty(textBoxMessage.Text) || textBoxMessage.ReadOnly);

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

        private async void buttonCommit_Click(object sender, EventArgs e)
        {
            SetControlesEnable(false, sender as Button);

            Commit commit = new Commit(
                Args.Host.Connector.Pmx.GetCurrentState(),
                Args.Host.Connector.Form,
                textBoxMessage.Text,
                Compressor,
                LogArchive
            );

            var commitTask = Task.Run(commit.Invoke);
            await commitTask.InvokeAsyncWithExportException(
                LogArchive.CommitDirectory,
                $"'{commit.Log.SavedPath}'に'{commit.Log.Filename}'を追加するときに例外が発生しました。"
            );

            textBoxMessage.Clear();
            SetControlesEnable(true, sender as Button);
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

        private async void buttonReCompress_Click(object sender, EventArgs e)
        {
            var msgTmp = textBoxMessage.Text;
            SetControlesEnable(false, sender as Button);

            var recompTask = Task.Run(() =>
            {
                if (File.Exists(LogArchive.ArchivePath))
                    Compressor.ReCompress(
                        LogArchive.ArchivePathWitoutExt,
                        (_, dpe, state) =>
                            {
                                var completedRatio = (float)dpe.AmountCompleted / dpe.TotalAmount;
                                textBoxMessage.Text = $"{state}: {completedRatio * 100: #.#}%";
                            },
                        (state) => textBoxMessage.Text = state
                    );
                else
                    MessageBox.Show($"アーカイブファイルが見つかりませんでした。{Environment.NewLine}期待されたパス:{LogArchive.ArchivePath}");
            });
            await recompTask.InvokeAsyncWithExportException(
                LogArchive.CommitDirectory,
                $"'{LogArchive.ArchivePath}'を再圧縮するときに例外が発生しました。"
            );

            textBoxMessage.Text = msgTmp;
            SetControlesEnable(true, sender as Button);
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

        private void radioButton7z_MouseEnter(object sender, EventArgs e)
        {
            textBoxDescription.Text = $"高い圧縮率{Environment.NewLine}解凍には7zipが必要です。";
        }

        private void radioButtonZip_MouseHover(object sender, EventArgs e)
        {
            textBoxDescription.Text = $"低い圧縮率{Environment.NewLine}Windows標準機能で解凍できます。";
        }

        private void ResetDexcription()
        {
            textBoxDescription.Text = DefaultDescription;
        }

        private void radioButton7z_MouseLeave(object sender, EventArgs e)
        {
            ResetDexcription();
        }

        private void radioButtonZip_MouseLeave(object sender, EventArgs e)
        {
            ResetDexcription();
        }

        private void buttonReconstruction_Click(object sender, EventArgs e)
        {
            var recForm = new FormReconstruction(Args, Compressor, LogArchive);
            recForm.ShowDialog(this);
        }
    }
}
