using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenZip;

namespace CommitPMX
{
    class SevenZipCompressor : ICompressor
    {
        private string extString;
        private SevenZip.SevenZipCompressor Compressor { get; } = new SevenZip.SevenZipCompressor();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sevenZipLibraryPath"><c>7z.dll</c>へのパス</param>
        /// <param name="format">現状<c>SevenZip</c>と<c>Zip</c>のみ対応</param>
        public SevenZipCompressor(string sevenZipLibraryPath, OutArchiveFormat format)
        {
            switch (format)
            {
                case OutArchiveFormat.SevenZip:
                    Compressor.CompressionMethod = CompressionMethod.Lzma2;
                    extString = ".7z";
                    break;
                case OutArchiveFormat.Zip:
                    Compressor.CompressionMethod = CompressionMethod.Lzma;
                    extString = ".zip";
                    break;
                //case OutArchiveFormat.GZip:
                //    break;
                //case OutArchiveFormat.BZip2:
                //    break;
                //case OutArchiveFormat.Tar:
                //    break;
                //case OutArchiveFormat.XZ:
                //    break;
                default:
                    // どうせ使わんし対応が面倒なので
                    throw new NotImplementedException($"{format}方式の圧縮は未対応です。");
            }

            SevenZipBase.SetLibraryPath(sevenZipLibraryPath);
            Compressor.ArchiveFormat = format;
            Compressor.CompressionLevel = CompressionLevel.Ultra;
        }

        public void AddFileToArchive(string filePath, string archivePath)
        {
            string archiveFullName = archivePath + extString;
            Compressor.CompressionMode = System.IO.File.Exists(archiveFullName) ? CompressionMode.Append : CompressionMode.Create;
            Compressor.CompressFiles(archiveFullName, filePath);
        }
    }
}
