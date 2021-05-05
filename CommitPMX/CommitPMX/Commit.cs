using PEPlugin.Form;
using PEPlugin.Pmx;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommitPMX
{
    class Commit
    {
        public IPXPmx Model { get; }
        public DateTime CommitTime { get; }
        public string Message { get; }

        private IPEFormConnector Connector { get; set; }
        private string DirectoryToCommit { get; set; }

        public Commit(IPXPmx model, IPEFormConnector connector, string message)
        {
            Connector = connector;
            Model = model;
            CommitTime = DateTime.Now;

            var modelPath = model.FilePath;
            DirectoryToCommit = Path.Combine(Path.GetDirectoryName(modelPath), $"CommitLog_{Path.GetFileNameWithoutExtension(Model.FilePath)}");
            Message = message;
        }

        public void Invoke()
        {
            // 書込用ディレクトリを作成
            // 既存の場合何もおこらない
            Directory.CreateDirectory(DirectoryToCommit);
            // 上書き保存
            Connector.SavePMXFile(Model.FilePath);
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
            string logModelFilename = $"{CommitTime:yyyy-MM-dd-HH-mm-ss-ff}_{Regex.Replace(Message, @"[<>:\/\\|? *""]", "")}.pmx";

            var modelPathTmp = Model.FilePath;
            Connector.SavePMXFile(logModelFilename);
            // コミット保存をした時点でModel.FilePathの値が書き換わるのでもとに戻す
            Model.FilePath = modelPathTmp;

            // アーカイブに追加する処理は時間がかかる可能性があることも考えて非同期でやる
            Task.Run(() =>
            {
                // アーカイブに履歴モデルを追加
                string archivePath = Path.Combine(DirectoryToCommit, "archive.zip");
                AddFileToArchive(logModelFilename, archivePath);
                // 未圧縮ファイルを削除
                File.Delete(logModelFilename);
            });
        }

        private void AddFileToArchive(string filePath, string archivePath)
        {
            using (var archive = ZipFile.Open(archivePath, File.Exists(archivePath) ? ZipArchiveMode.Update : ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath), CompressionLevel.Optimal);
            }
        }
    }
}
