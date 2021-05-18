using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitPMX
{
    [JsonObject]
    struct CommitLog
    {
        public DateTime Date { get; }
        public string Message { get; }
        public string Filename { get; }
        public string ArchiveFormat { get; }
        public string SavedPath { get; }

        public CommitLog(DateTime date, string message, string fileName, string archiveFormat, string savedPath)
        {
            Date = date;
            Message = message;
            Filename = fileName;
            ArchiveFormat = archiveFormat;
            SavedPath = savedPath;
        }
    }
}
