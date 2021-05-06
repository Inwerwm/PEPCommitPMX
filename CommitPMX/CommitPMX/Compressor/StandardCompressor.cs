using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitPMX.Compressor
{
    /// <summary>
    /// .NET 標準ライブラリによる圧縮
    /// </summary>
    class StandardCompressor : ICompressor
    {
        public void AddFileToArchive(string filePath, string archivePath)
        {
            using (var archive = ZipFile.Open(archivePath, File.Exists(archivePath) ? ZipArchiveMode.Update : ZipArchiveMode.Create))
                archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath), CompressionLevel.Optimal);
        }
    }
}
