﻿using PEPlugin.Form;
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

            Directory.CreateDirectory(DirectoryToCommit);
        }

        public void Invoke()
        {
            WriteLog();
            WriteModel();
        }

        public void WriteLog()
        {
            string pathOfLog = Path.Combine(DirectoryToCommit, "CommitLog.csv");
            var existLogFile = File.Exists(pathOfLog);

            using (StreamWriter writer = new StreamWriter(pathOfLog, true, Encoding.UTF8))
            {
                // 初期作成ファイルにヘッダーを記入
                if (!existLogFile)
                    writer.WriteLine("\"日付\",\"メッセージ\"");

                writer.WriteLine($"\"{CommitTime:yyyy/MM/dd HH:mm:ss.ff}\",\"{Message.Replace("\"", "\"\"")}\"");
            }
        }

        public void WriteModel()
        {
            var modelPath = Model.FilePath;
            var commitPath = Path.Combine(DirectoryToCommit, $"{CommitTime:yyyy-MM-dd-HH-mm-ss-ff}_{Regex.Replace(Message, @"[<>:\/\\|? *""]", "")}");

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
