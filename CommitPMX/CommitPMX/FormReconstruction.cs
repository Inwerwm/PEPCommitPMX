using PEPExtensions;
using PEPlugin;
using PEPlugin.Pmx;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommitPMX
{
    internal partial class FormReconstruction : Form
    {
        IPERunArgs Args { get; }
        SevenZipCompressor Compressor { get; }
        LogArchive LogArchive { get; }

        CommitLog? SelectedCommitLog => dataGridViewCommits.RowCount > 0 ? (CommitLog)dataGridViewCommits.SelectedRows[0].DataBoundItem : (CommitLog?)null;

        public FormReconstruction(IPERunArgs args, SevenZipCompressor compressor, LogArchive logArchive)
        {
            InitializeComponent();

            Args = args;
            Compressor = compressor;
            LogArchive = logArchive;
        }

        private void LoadLogs()
        {
            var logs = LogArchive.CommitLogs;

            if (logs.Any())
            {
                dataGridViewCommits.DataSource = logs.Reverse().ToArray();
            }
            else
            {
                MessageBox.Show("履歴が見つかりませんでした。", "復元ファイルの選択", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void ToggleButtons(bool enable)
        {
            buttonExtract.Enabled = enable;
            buttonOverwrite.Enabled = enable;
            buttonRemove.Enabled = enable;
        }

        private void FormReconstruction_Load(object sender, EventArgs e)
        {
            LoadLogs();
            dataGridViewCommits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private static IPXPmx ExtractPmx(in CommitLog selectedLog)
        {
            var extractedPmx = PEStaticBuilder.Pmx.Pmx();
            var path = SevenZipCompressor.Extract(selectedLog.Filename, selectedLog.SavedPath);
            extractedPmx.FromFile(path);

            return extractedPmx;
        }

        private async void buttonExtract_Click(object sender, EventArgs e)
        {
            if (!SelectedCommitLog.HasValue)
                return;
            var selectedLog = SelectedCommitLog.Value;

            ToggleButtons(false);

            var extract = Task.Run(() =>
            {
                if (selectedLog.Format == CommitLog.ArchiveFormat.None)
                {
                    File.Copy(Path.Combine(selectedLog.SavedPath, selectedLog.Filename), Path.Combine(LogArchive.CommitDirectory, selectedLog.Filename));
                }
                else
                {
                    ExtractPmx(in selectedLog);
                }
            });
            await extract.InvokeAsyncWithExportException(LogArchive.CommitDirectory, $"{selectedLog.Filename}の解凍に失敗しました。");

            ToggleButtons(true);
            MessageBox.Show("解凍が完了しました。", "ファイルの解凍");
            Close();
        }

        private async void buttonOverwrite_Click(object sender, EventArgs e)
        {
            if (!SelectedCommitLog.HasValue)
                return;
            var selectedLog = SelectedCommitLog.Value;

            ToggleButtons(false);

            var extract = Task.Run(() =>
            {
                var pmx = PEStaticBuilder.Pmx.Pmx();

                if (selectedLog.Format == CommitLog.ArchiveFormat.None)
                {
                    pmx.FromFile(Path.Combine(selectedLog.SavedPath, selectedLog.Filename));
                }
                else
                {
                    using (var pmxStream = new MemoryStream())
                    {
                        SevenZipCompressor.Extract(selectedLog.Filename, selectedLog.SavedPath, pmxStream);
                        pmxStream.Position = 0;
                        pmx.FromStream(pmxStream);
                    }
                }

                Utility.Update(Args.Host.Connector, pmx);
            });
            await extract.InvokeAsyncWithExportException(LogArchive.CommitDirectory, $"{selectedLog.Filename}の解凍に失敗しました。");

            ToggleButtons(true);
            Close();
        }

        private async void buttonRemove_Click(object sender, EventArgs e)
        {
            if (!SelectedCommitLog.HasValue)
                return;
            var selectedLog = SelectedCommitLog.Value;

            if (MessageBox.Show($"{selectedLog.Filename}を削除します。{Environment.NewLine}削除されたファイルは復元できません。{Environment.NewLine}よろしいですか?", "履歴の削除", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;

            ToggleButtons(false);

            var remove = Task.Run(() => LogArchive.Remove(selectedLog));
            await remove.InvokeAsyncWithExportException(
                LogArchive.CommitDirectory,
                "履歴の削除に失敗しました。",
                () => ToggleButtons(true)
            );

            LoadLogs();
        }
    }
}
