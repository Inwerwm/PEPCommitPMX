using PEPlugin.Form;
using PEPlugin.Pmx;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CommitPMX
{
    class Commit
    {
        public IPXPmx Model { get; }
        public DateTime CommitTime { get; }
        public string Comment { get; }

        private IPEFormConnector Connector { get; set; }
        private string DirectoryToCommit { get; set; }

        public Commit(IPXPmx model, IPEFormConnector connector, string comment)
        {
            Connector = connector;
            Model = model;
            CommitTime = DateTime.Now;

            var modelPath = model.FilePath;
            DirectoryToCommit = Path.Combine(Path.GetDirectoryName(modelPath), $"CommitLog_{Path.GetFileNameWithoutExtension(Model.FilePath)}");
            Comment = comment;

            Directory.CreateDirectory(DirectoryToCommit);
        }

        public void Invoke()
        {
            WriteLog();
            WriteModel();
        }

        public void WriteLog()
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(DirectoryToCommit, "CommitLog.csv"), true, Encoding.UTF8))
                writer.WriteLine($"\"{CommitTime:yyyy/MM/dd HH:mm:ss.ff}\",\"{Comment.Replace("\"", "\"\"")}\"");
        }

        public void WriteModel()
        {
            var modelPath = Model.FilePath;
            var commitPath = Path.Combine(DirectoryToCommit, $"{CommitTime:yyyy-MM-dd-HH-mm-ss-ff}_{Regex.Replace(Comment, @"[<>:\/\\|? *""]", "")}");

            // フルパスの長さには上限があるので
            // 少し余裕を持ったパス名にする
            if (commitPath.Length > 250)
                commitPath = commitPath.Substring(0, 250);

            Connector.SavePMXFile($"{commitPath}.pmx");

            // コミット保存をした時点でModel.FilePathの値が書き換わるのでもとに戻す
            Model.FilePath = modelPath;
            Connector.SavePMXFile(Model.FilePath);
        }
    }
}
