using PEPlugin.Form;
using PEPlugin.Pmx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitPMX
{
    class Comitter
    {
        private IPEFormConnector Connector { get; set; }
        private IPXPmx Model { get; set; }
        private DateTime CommitTime { get; set; }
        private string Comment { get; set; }
        private string DirectoryToCommit { get; set; }

        public void Commit(IPXPmx model, IPEFormConnector connector, string comment)
        {
            Connector = connector;
            Model = model;
            CommitTime = DateTime.Now;

            var modelPath = model.FilePath;
            DirectoryToCommit = Path.Combine(Path.GetDirectoryName(modelPath), "Committed");
            Comment = comment;

            WriteLog();
            WriteModel();
        }

        public void WriteLog()
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(DirectoryToCommit, "CommitLog.csv"), true,Encoding.UTF8))
            {
                // ログを書込
                writer.WriteLine($"{CommitTime:yyyy/MM/dd HH:mm:ss.ff}, {Path.GetFileNameWithoutExtension(Model.FilePath)}, {Comment}");
            }
        }

        public void WriteModel()
        {
            // 上書き保存
            Connector.SavePMXFile(Model.FilePath);

            var modelName = Path.GetFileNameWithoutExtension(Model.FilePath);
            string commitName = $"{CommitTime:yyyy-MM-dd-HH-mm-ss-ff}-{Comment}";

            // フルパスの長さには上限があるので
            // 少し余裕を持ったパス名にする
            if (commitName.Length > 250)
                commitName = commitName.Substring(0, 250 - modelName.Length);

            // コメント付き保存
            Connector.SavePMXFile(Path.Combine(DirectoryToCommit, $"{modelName}_{commitName}.pmx"));
        }
    }
}
