using Newtonsoft.Json;
using SevenZip;
using System;

namespace CommitPMX
{
    [JsonObject]
    struct CommitLog
    {
        public DateTime Date { get; }
        public string Message { get; }
        public string Filename { get; }
        public ArchiveFormat Format { get; }
        public string SavedPath { get; }

        public CommitLog(DateTime date, string message, string fileName, ArchiveFormat format, string savedPath)
        {
            Date = date;
            Message = message;
            Filename = fileName;
            Format = format;
            SavedPath = savedPath;
        }

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
