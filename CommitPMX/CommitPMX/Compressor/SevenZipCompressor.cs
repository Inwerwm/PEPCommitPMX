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
            Compressor.PreserveDirectoryRoot = false;
            Compressor.CompressionLevel = CompressionLevel.Ultra;
        }

        public void AddFileToArchive(string filePath, string archivePath)
        {
            string archiveFullName = archivePath + extString;
            Compressor.CompressionMode = System.IO.File.Exists(archiveFullName) ? CompressionMode.Append : CompressionMode.Create;
            Compressor.CompressFiles(archiveFullName, filePath);
        }

        /// <summary>
        /// 対象アーカイブを再圧縮する
        /// 1つずつ追加されたアーカイブをまとめて圧縮することで圧縮率が向上する
        /// </summary>
        /// <param name="archivePath">対象アーカイブへのパス</param>
        public void ReCompress(string archivePath)
        {
            var archiveDir = System.IO.Path.GetDirectoryName(archivePath);
            var extractDir = System.IO.Path.Combine(archiveDir, "tmp");

            var extractor = new SevenZipExtractor(archivePath);
            extractor.ExtractArchive(extractDir);

            Compressor.CompressionMode = CompressionMode.Create;
            Compressor.CompressDirectory(extractDir, archivePath);

            System.IO.Directory.Delete(extractDir, true);
        }
    }
}
