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
        [JsonProperty("date")]
        public DateTime Date { get; }
        [JsonProperty("message")]
        public string Message { get; }
        [JsonProperty("filename")]
        public string Filename { get; }
        [JsonProperty("archiveFormat")]
        public string ArchiveFormat { get; }
        [JsonProperty("savedPath")]
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
