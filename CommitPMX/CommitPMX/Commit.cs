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
        public CommitLog Log { get; }
        private ICompressor Compressor { get; }
        LogArchive LogArchive { get; }

        private IPEFormConnector Connector { get; set; }

        private string ModelTmpFilename => Path.Combine(LogArchive.CommitDirectory, Log.Filename);

        public Commit(IPXPmx model, IPEFormConnector connector, string message, ICompressor compressor, LogArchive logArchive)
        {
            Compressor = compressor;
            LogArchive = logArchive;

            Connector = connector;
            Model = model;

            DateTime now = DateTime.Now;
            Log = new CommitLog(
                now,
                message,
                (date,msg) => $"{date:yyyy-MM-dd-HH-mm-ss-ff}_{Regex.Replace(msg, @"[<>:\/\\|? *""]", "")}.pmx",
                CommitLog.ConvertFormatEnum(Compressor.ArchiveFormat),
                LogArchive.ArchivePath
            );
        }

        public void Invoke()
        {
            var modelPathTmp = Model.FilePath;
            Connector.SavePMXFile(ModelTmpFilename);
            // コミット保存をした時点でModel.FilePathの値が書き換わるのでもとに戻す
            Model.FilePath = modelPathTmp;
            // 上書き保存
            Connector.SavePMXFile(Model.FilePath);

            LogArchive.Append(Log, ModelTmpFilename);
        }
    }
}
