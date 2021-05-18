using PEPlugin.Form;
using PEPlugin.Pmx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommitPMX
{
    class Commit
    {
        public static string ArchiveName => "archive";

        public IPXPmx Model { get; }
        public DateTime CommitTime { get; }
        public string Message { get; }
        private ICompressor Compressor { get; }

        private IPEFormConnector Connector { get; set; }
        private string DirectoryToCommit { get; set; }

        public static string BuildCommitDirectryPath(string modelPath) =>
            Path.Combine(Path.GetDirectoryName(modelPath), $"CommitLog_{Path.GetFileNameWithoutExtension(modelPath)}");

        public Commit(IPXPmx model, IPEFormConnector connector, string message, ICompressor compressor)
        {
            Connector = connector;
            Model = model;
            CommitTime = DateTime.Now;
            Compressor = compressor;

            var modelPath = model.FilePath;
            DirectoryToCommit = BuildCommitDirectryPath(modelPath);
            Message = message;
        }

        public void Invoke()
        {
            // 書込用ディレクトリを作成
            // 既存の場合何もおこらない
            Directory.CreateDirectory(DirectoryToCommit);
            // 別に時間がかかる処理でもないがなんとなく非同期でやる
            Task.Run(WriteLog);
            WriteModel();
        }

        private void WriteLog()
        {
            string pathOfLog = Path.Combine(DirectoryToCommit, "CommitLog.csv");
            var existLogFile = File.Exists(pathOfLog);

            var logTexts = new List<string>(2);
            if (!existLogFile)
                logTexts.Add("\"Date\",\"Message\"");
            logTexts.Add($"\"{CommitTime:yyyy/MM/dd HH:mm:ss.ff}\",\"{Message.Replace("\"", "\"\"")}\"");

            File.AppendAllLines(pathOfLog, logTexts);
        }

        private void WriteModel()
        {
            string logModelFilename = Path.Combine(DirectoryToCommit, $"{CommitTime:yyyy-MM-dd-HH-mm-ss-ff}_{Regex.Replace(Message, @"[<>:\/\\|? *""]", "")}.pmx");

            var modelPathTmp = Model.FilePath;
            Connector.SavePMXFile(logModelFilename);
            // コミット保存をした時点でModel.FilePathの値が書き換わるのでもとに戻す
            Model.FilePath = modelPathTmp;
            // 上書き保存
            Connector.SavePMXFile(Model.FilePath);

            // アーカイブに追加する処理は時間がかかる可能性があることも考えて非同期でやる
            Task.Run(() =>
            {
                (string Value, bool HasValue) exception = (null, false);
                // アーカイブに履歴モデルを追加
                string archivePath = Path.Combine(DirectoryToCommit, ArchiveName);

                try
                {
                    Compressor.AddFileToArchive(logModelFilename, archivePath);
                    // 未圧縮ファイルを削除
                    File.Delete(logModelFilename);
                }
                catch (Exception ex)
                {
                    exception.Value = $"========================================{Environment.NewLine}" +
                                      $"{DateTime.Now:G}{Environment.NewLine}" +
                                      $"{ex.GetType()}{Environment.NewLine}" +
                                      $"'{archivePath + Compressor.ExtString}'に'{logModelFilename}'を追加するときに例外が発生しました。{Environment.NewLine}" +
                                      $"{ex.Message}{Environment.NewLine}" +
                                      $"{ex.StackTrace}{Environment.NewLine}";
                    exception.HasValue = true;
                    System.Windows.Forms.MessageBox.Show($"アーカイブへの追加に失敗しました。{Environment.NewLine}{ex.Message}", "コミットの失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                try
                {
                    if (exception.HasValue)
                    {
                        File.AppendAllText(DirectoryToCommit + "Exceptions.log", exception.Value);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show($"例外履歴の書込に失敗しました。{Environment.NewLine}{ex.Message}", "例外履歴書込の失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            });
        }
    }
}
