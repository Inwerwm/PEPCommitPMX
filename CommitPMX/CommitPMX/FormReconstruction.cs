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
        CommitLog? SelectedCommitLog => (CommitLog)dataGridViewCommits.SelectedRows[0].DataBoundItem;
        private string LogFilePath { get; set; }

        public FormReconstruction(IPERunArgs args, SevenZipCompressor compressor)
        {
            InitializeComponent();

            Args = args;
            Compressor = compressor;
        }

        private void ToggleButtons(bool enable)
        {
            buttonExtract.Enabled = enable;
            buttonOverwrite.Enabled = enable;
            buttonRemove.Enabled = enable;
        }

        private void FormReconstruction_Load(object sender, EventArgs e)
        {
            var commitDir = Commit.BuildCommitDirectryPath(Args.Host.Connector.Pmx.CurrentPath);
            LogFilePath = Path.Combine(commitDir, Commit.LogFileName);

            var logLines = File.ReadLines(LogFilePath);
            dataGridViewCommits.DataSource = logLines.Select(JsonConvert.DeserializeObject<CommitLog>).Reverse().ToArray();
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
                await Task.Run(() =>
                {
                    if (selectedLog.Format == CommitLog.ArchiveFormat.None)
                        File.Delete(Path.Combine(selectedLog.SavedPath, selectedLog.Filename));
                    else
                        Compressor.Remove(selectedLog.Filename, selectedLog.SavedPath);
                });
                RemoveLog(selectedLog);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ToggleButtons(true);
            }
        }

        private void RemoveLog(CommitLog targetLog)
        {
            var logs = dataGridViewCommits.DataSource as CommitLog[];
            var removedLogs = logs.Where(log => !log.Equals(targetLog)).ToArray();
            dataGridViewCommits.DataSource = removedLogs;
            var logTexts = removedLogs.Select(log => JsonConvert.SerializeObject(log, Formatting.None)).Reverse();
            File.WriteAllText(LogFilePath, logTexts.Aggregate((sum, elm) => sum + Environment.NewLine + elm) + Environment.NewLine);
        }
    }
}
