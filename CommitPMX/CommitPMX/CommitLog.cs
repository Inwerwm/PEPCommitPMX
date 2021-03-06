using Newtonsoft.Json;
using SevenZip;
using System;

namespace CommitPMX
{
    [JsonObject]
    public struct CommitLog
    {
        public DateTime Date { get; }
        public string Message { get; }
        public string Filename { get; }
        public ArchiveFormat Format { get; }
        public string SavedPath { get; }

        [JsonConstructor]
        public CommitLog(DateTime date, string message, string filename, ArchiveFormat format, string savedPath)
        {
            Date = date;
            Message = message;
            Filename = filename;
            Format = format;
            SavedPath = savedPath;
        }

        public CommitLog(DateTime date, string message, Func<DateTime, string, string> filenameFormatter, ArchiveFormat format, string savedPath)
        {
            Date = date;
            Message = message;
            Filename = filenameFormatter(date, message);
            Format = format;
            SavedPath = savedPath;
        }

        public string ToJson() => JsonConvert.SerializeObject(this, Formatting.None);
        public static CommitLog FromJson(string json) => JsonConvert.DeserializeObject<CommitLog>(json);

        public static ArchiveFormat ConvertFormatEnum(OutArchiveFormat? szFormat)
        {
            if (!szFormat.HasValue)
                return ArchiveFormat.None;

            switch (szFormat)
            {
                case OutArchiveFormat.SevenZip:
                    return ArchiveFormat.SevenZip;
                case OutArchiveFormat.Zip:
                    return ArchiveFormat.Zip;
                default:
                    return ArchiveFormat.None;
            }
        }

        public enum ArchiveFormat
        {
            Zip,
            SevenZip,
            None
        }
    }
}
