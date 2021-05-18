﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenZip;

namespace CommitPMX
{
    class SevenZipCompressor : ICompressor
    {
        private SevenZip.SevenZipCompressor Compressor { get; } = new SevenZip.SevenZipCompressor();
        public string ExtString { get; }

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
                    ExtString = ".7z";
                    break;
                case OutArchiveFormat.Zip:
                    Compressor.CompressionMethod = CompressionMethod.Lzma;
                    ExtString = ".zip";
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
            string archiveFullName = archivePath + ExtString;
            try
            {
                Compressor.CompressionMode = File.Exists(archiveFullName) ? CompressionMode.Append : CompressionMode.Create;
                Compressor.CompressFiles(archiveFullName, filePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 対象アーカイブを再圧縮する
        /// 1つずつ追加されたアーカイブをまとめて圧縮することで圧縮率が向上する
        /// </summary>
        /// <param name="archivePath">対象アーカイブへのパス アーカイブ名に拡張子はいらない</param>
        public void ReCompress(string archivePath)
        {
            string archivePathWithExt = archivePath + ExtString;
            var archiveDir = System.IO.Path.GetDirectoryName(archivePathWithExt);
            var extractDir = System.IO.Path.Combine(archiveDir, "tmp");

            var extractor = new SevenZipExtractor(archivePathWithExt);
            extractor.ExtractArchive(extractDir);

            Compressor.CompressionMode = CompressionMode.Create;
            Compressor.CompressDirectory(extractDir, archivePathWithExt);

            System.IO.Directory.Delete(extractDir, true);
        }
    }
}
