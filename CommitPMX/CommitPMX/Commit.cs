using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
        public static string LogFileName => "CommitLog.txt";

        public IPXPmx Model { get; }
        public DateTime CommitTime { get; }
        public string Message { get; }
        private ICompressor Compressor { get; }

        private IPEFormConnector Connector { get; set; }
        private string DirectoryToCommit { get; set; }
        private string LogModelFilename => Path.Combine(DirectoryToCommit, $"{CommitTime:yyyy-MM-dd-HH-mm-ss-ff}_{Regex.Replace(Message, @"[<>:\/\\|? *""]", "")}.pmx");
        private string ArchivePath => Path.Combine(DirectoryToCommit, ArchiveName);

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

            var saveSucceed = WriteModel();
            WriteLog(saveSucceed);
        }

        private void WriteLog(bool saveSucceed)
        {
            string pathOfLog = Path.Combine(DirectoryToCommit, LogFileName);

            string format = saveSucceed ? Compressor.ArchiveFormat.ToString() : "Compression Failed";
            string savedPath = saveSucceed ? ArchivePath + Compressor.ExtString
                             : File.Exists(Path.Combine(DirectoryToCommit, Path.GetFileName(LogModelFilename))) ? DirectoryToCommit : "Unknown";
            
            var log = new CommitLog(CommitTime, Message, Path.GetFileName(LogModelFilename), format, savedPath);
            var jsonLog = JsonConvert.SerializeObject(log, Formatting.None);
            File.AppendAllText(pathOfLog, jsonLog + Environment.NewLine);
        }

        private bool WriteModel()
        {
            bool isSuccess = true;

            var modelPathTmp = Model.FilePath;
            Connector.SavePMXFile(LogModelFilename);
            // コミット保存をした時点でModel.FilePathの値が書き換わるのでもとに戻す
            Model.FilePath = modelPathTmp;
            // 上書き保存
            Connector.SavePMXFile(Model.FilePath);

            // アーカイブに履歴モデルを追加
            (string Value, bool HasValue) exception = (null, false);

            try
            {
                Compressor.AddFileToArchive(LogModelFilename, ArchivePath);
                // 未圧縮ファイルを削除
                File.Delete(LogModelFilename);
            }
            catch (Exception ex)
            {
                exception.Value = $"========================================{Environment.NewLine}" +
                                  $"{DateTime.Now:G}{Environment.NewLine}" +
                                  $"{ex.GetType()}{Environment.NewLine}" +
                                  $"'{ArchivePath + Compressor.ExtString}'に'{LogModelFilename}'を追加するときに例外が発生しました。{Environment.NewLine}" +
                                  $"{ex.Message}{Environment.NewLine}" +
                                  $"{ex.StackTrace}{Environment.NewLine}";
                exception.HasValue = true;
                System.Windows.Forms.MessageBox.Show($"アーカイブへの追加に失敗しました。{Environment.NewLine}{ex.Message}", "コミットの失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                isSuccess = false;
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

            return isSuccess;
        }
    }
}
