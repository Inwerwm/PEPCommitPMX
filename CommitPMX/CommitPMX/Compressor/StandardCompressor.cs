using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitPMX
{
    /// <summary>
    /// .NET 標準ライブラリによる圧縮
    /// </summary>
    class StandardCompressor : ICompressor
    {
        public string ExtString => ".zip";

        public void AddFileToArchive(string filePath, string archivePath)
        {
            string archiveFullName = archivePath + ExtString;
            using (var archive = ZipFile.Open(archiveFullName, File.Exists(archiveFullName) ? ZipArchiveMode.Update : ZipArchiveMode.Create))
                archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath), CompressionLevel.Optimal);
        }
    }
}
