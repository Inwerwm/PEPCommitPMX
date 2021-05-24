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

        private string ModelTmpFilename => Path.Combine(LogArchive.LogDirectory, Log.Filename);

        public static string BuildCommitDirectryPath(string modelPath) =>
            Path.Combine(Path.GetDirectoryName(modelPath), $"CommitLog_{Path.GetFileNameWithoutExtension(modelPath)}");

        public Commit(IPXPmx model, IPEFormConnector connector, string message, ICompressor compressor, LogArchive logArchive)
        {
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

            Compressor = compressor;
            LogArchive = logArchive;

            var modelPath = model.FilePath;
        }

        public void Invoke()
        {
            var modelPathTmp = Model.FilePath;
            Connector.SavePMXFile(ModelTmpFilename);
            // コミット保存をした時点でModel.FilePathの値が書き換わるのでもとに戻す
            Model.FilePath = modelPathTmp;
            // 上書き保存
            Connector.SavePMXFile(Model.FilePath);


            (string Value, bool HasValue) exception = (null, false);
            try
            {
                LogArchive.Append(Log, ModelTmpFilename);
            }
            catch (Exception ex)
            {
                exception.Value = $"========================================{Environment.NewLine}" +
                                  $"{DateTime.Now:G}{Environment.NewLine}" +
                                  $"{ex.GetType()}{Environment.NewLine}" +
                                  $"'{Log.SavedPath}'に'{ModelTmpFilename}'を追加するときに例外が発生しました。{Environment.NewLine}" +
                                  $"{ex.Message}{Environment.NewLine}" +
                                  $"{ex.StackTrace}{Environment.NewLine}";
                exception.HasValue = true;
                System.Windows.Forms.MessageBox.Show($"アーカイブへの追加に失敗しました。{Environment.NewLine}{ex.Message}", "コミットの失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            try
            {
                if (exception.HasValue)
                {
                    File.AppendAllText(Path.Combine(LogArchive.LogDirectory, "Exceptions.log"), exception.Value);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"例外履歴の書込に失敗しました。{Environment.NewLine}{ex.Message}", "例外履歴書込の失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
    }
}
