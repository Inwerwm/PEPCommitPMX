using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace CommitPMX
{
    class LogArchive
    {
        public string CommitDirectory { get; }
        
        public string LogFilename => "CommitLog";
        public string LogFileExt => ".txt";
        public string LogFilePath => Path.Combine(CommitDirectory, LogFilename + LogFileExt);

        public string ArchiveFilename => "archive";
        public string ArchiveExt => Compressor.ExtString;
        public string ArchivePathWitoutExt => Path.Combine(CommitDirectory, ArchiveFilename);
        public string ArchivePath => ArchivePathWitoutExt + ArchiveExt;

        public SevenZipCompressor Compressor { get; }

        private IEnumerable<CommitLog> JsonLog
        {
            get
            {
                return File.Exists(LogFilePath) ?
                       File.ReadLines(LogFilePath).Select(CommitLog.FromJson) :
                       new CommitLog[0];
            }
            set
            {
                if (value.Any())
                {
                    File.WriteAllText(
                        LogFilePath,
                        value.Select(log => log.ToJson())
                            .Aggregate((sum, elm) => sum + Environment.NewLine + elm)
                        + Environment.NewLine
                    );
                }
                else if (File.Exists(LogFilePath))
                {
                    File.Delete(LogFilePath);
                }
            }
        }

        public CommitLog[] CommitLogs => JsonLog.ToArray();

        private static string BuildCommitDirectryPath(string modelPath) =>
            Path.Combine(Path.GetDirectoryName(modelPath), $"CommitLog_{Path.GetFileNameWithoutExtension(modelPath)}");


        public LogArchive(string modelPath, SevenZipCompressor compressor)
        {
            CommitDirectory = BuildCommitDirectryPath(modelPath);
            Compressor = compressor;

            Directory.CreateDirectory(CommitDirectory);
        }

        private void AppendToJsonLog(CommitLog log)
        {
            File.AppendAllText(LogFilePath, log.ToJson() + Environment.NewLine);
        }

        private void RemoveFromJsonLog(CommitLog log)
        {
            JsonLog = JsonLog.Where(l => !l.Equals(log));
        }

        private void AppendToArchive(string filePath)
        {
            Exception occurredEx = null;

            try
            {
                Compressor.AddFileToArchive(filePath, ArchivePathWitoutExt);
            }
            catch (Exception ex)
            {
                occurredEx = ex;
            }

            if (!(occurredEx is null))
            {
                try
                {
                    // ファイルのアーカイブへの追加に失敗したら無圧縮で保存
                    var noCompArchivePath = Path.Combine(CommitDirectory, ArchiveFilename);
                    Directory.CreateDirectory(noCompArchivePath);
                    File.Copy(filePath, Path.Combine(noCompArchivePath, Path.GetFileName(filePath)));
                }
                catch (Exception)
                {
                    throw new SaveLogFailedException($"コミットに失敗しました{Environment.NewLine}アーカイブへのファイルの追加と無圧縮アーカイブへのコピーの両方に失敗しました。", occurredEx);
                }

                throw occurredEx;
            }
        }

        private void RemoveFromArchive(CommitLog log)
        {
            if (log.Format == CommitLog.ArchiveFormat.None)
                File.Delete(Path.Combine(log.SavedPath, log.Filename));
            else
                Compressor.Remove(log.Filename, log.SavedPath);
        }

        public void Append(CommitLog commitLog, string tmpFilePath)
        {
            CommitLog log = commitLog;
            Exception ex = null;

            try
            {
                AppendToArchive(tmpFilePath);
            }
            catch (SaveLogFailedException)
            {
                throw;
            }
            catch (Exception e)
            {
                log = new CommitLog(commitLog.Date, commitLog.Message, commitLog.Filename, CommitLog.ArchiveFormat.None, Path.Combine(CommitDirectory, ArchiveFilename));

                ex = e;
            }
            File.Delete(tmpFilePath);

            try
            {
                AppendToJsonLog(log);
            }
            catch (Exception e)
            {
                throw ex is null ? new SaveLogFailedException("履歴テキストの書込に失敗しました。", e) : new SaveLogFailedException("履歴の保存に失敗しました。", ex);
            }

            if (!(ex is null))
                throw ex;
        }

        public void Remove(CommitLog commitLog)
        {
            try
            {
                RemoveFromArchive(commitLog);
                RemoveFromJsonLog(commitLog);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    [Serializable()]
    public class SaveLogFailedException : Exception
    {
        public SaveLogFailedException()
            : base()
        {
        }

        public SaveLogFailedException(string message)
            : base(message)
        {
        }

        public SaveLogFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected SaveLogFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
