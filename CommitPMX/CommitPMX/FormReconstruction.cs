using Newtonsoft.Json;
using PEPExtensions;
using PEPlugin;
using PEPlugin.Pmx;
using System;
using System.Data;
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
                dataGridViewCommits.DataSource = logs;
            else
                MessageBox.Show("履歴が見つかりませんでした。", "復元ファイルの選択", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
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

        private void buttonExtract_Click(object sender, EventArgs e)
        {
            if (!SelectedCommitLog.HasValue)
                return;
            var selectedLog = SelectedCommitLog.Value;

            if (selectedLog.Format == CommitLog.ArchiveFormat.None)
                return;

            try
            {
                ExtractPmx(in selectedLog);
            }
            catch (Exception)
            {
                throw;
            }

            MessageBox.Show("解凍が完了しました。", "ファイルの解凍");
            Close();
        }

        private void buttonOverwrite_Click(object sender, EventArgs e)
        {
            if (!SelectedCommitLog.HasValue)
                return;
            var selectedLog = SelectedCommitLog.Value;

            var pmx = PEStaticBuilder.Pmx.Pmx();

            if (selectedLog.Format != CommitLog.ArchiveFormat.None)
            {
                using (var pmxStream = new MemoryStream())
                {
                    SevenZipCompressor.Extract(selectedLog.Filename, selectedLog.SavedPath, pmxStream);
                    pmxStream.Position = 0;
                    pmx.FromStream(pmxStream);
                }
            }
            else
            {
                pmx.FromFile(Path.Combine(selectedLog.SavedPath, selectedLog.Filename));
            }

            Utility.Update(Args.Host.Connector, pmx);
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

            try
            {
                await Task.Run(() => LogArchive.Remove(selectedLog));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"履歴の削除に失敗しました。{Environment.NewLine}{ex.Message}", "履歴の削除", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ToggleButtons(true);
            }

            LoadLogs();
        }
    }
}
