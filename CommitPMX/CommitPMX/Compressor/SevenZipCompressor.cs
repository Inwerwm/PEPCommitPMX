using SevenZip;
using System;
using System.IO;

namespace CommitPMX
{
    class SevenZipCompressor : ICompressor
    {
        public string ExtString
        {
            get
            {
                switch (ArchiveFormat)
                {
                    case OutArchiveFormat.SevenZip:
                        return ".7z";
                    case OutArchiveFormat.Zip:
                        return ".zip";
                    //case OutArchiveFormat.GZip:
                    //    break;
                    //case OutArchiveFormat.BZip2:
                    //    break;
                    //case OutArchiveFormat.Tar:
                    //    break;
                    //case OutArchiveFormat.XZ:
                    //    break;
                    default:
                        throw new NotImplementedException($"{ArchiveFormat}方式の圧縮は未対応です。");
                }
            }
        }
        public OutArchiveFormat ArchiveFormat { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sevenZipLibraryPath"><c>7z.dll</c>へのパス</param>
        /// <param name="format">現状<c>SevenZip</c>と<c>Zip</c>のみ対応</param>
        public SevenZipCompressor(string sevenZipLibraryPath, OutArchiveFormat format)
        {
            SevenZipBase.SetLibraryPath(sevenZipLibraryPath);
            ArchiveFormat = format;
        }

        private SevenZip.SevenZipCompressor CreateCompressor()
        {
            var compressor = new SevenZip.SevenZipCompressor();

            switch (ArchiveFormat)
            {
                case OutArchiveFormat.SevenZip:
                    compressor.CompressionMethod = CompressionMethod.Lzma2;
                    break;
                case OutArchiveFormat.Zip:
                    compressor.CompressionMethod = CompressionMethod.Lzma;
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
                    throw new NotImplementedException($"{ArchiveFormat}方式の圧縮は未対応です。");
            }
            compressor.ArchiveFormat = ArchiveFormat;
            compressor.PreserveDirectoryRoot = false;
            compressor.CompressionLevel = CompressionLevel.Ultra;

            return compressor;
        }

        public void AddFileToArchive(string filePath, string archivePath)
        {
            var compressor = CreateCompressor();
            string archiveFullName = archivePath + ExtString;
            try
            {
                compressor.CompressionMode = File.Exists(archiveFullName) ? CompressionMode.Append : CompressionMode.Create;
                compressor.CompressFiles(archiveFullName, filePath);
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
        /// <param name="progressHandler">プログレス表示用メソッド</param>
        public void ReCompress(string archivePath, Action<object, ProgressEventArgs> progressHandler = null)
        {
            var compressor = CreateCompressor();

            string archivePathWithExt = archivePath + ExtString;
            var archiveDir = Path.GetDirectoryName(archivePathWithExt);
            var extractDir = Path.Combine(archiveDir, ".extracted_archive_tmp");
            if (!(progressHandler is null))
                compressor.Compressing += new EventHandler<ProgressEventArgs>(progressHandler);

            try
            {
                if (Directory.Exists(extractDir))
                {
                    Directory.Delete(extractDir, true);
                }

                using (var extractor = new SevenZipExtractor(archivePathWithExt))
                    extractor.ExtractArchive(extractDir);

                var dirInfo = new DirectoryInfo(extractDir);
                dirInfo.Attributes |= FileAttributes.Hidden;

                compressor.CompressionMode = CompressionMode.Create;
                compressor.CompressDirectory(extractDir, archivePathWithExt);

                Directory.Delete(extractDir, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// アーカイブ内から指定ファイルを解凍する
        /// </summary>
        /// <param name="filename">解凍するファイル名</param>
        /// <param name="archivePath">ファイルのあるアーカイブへのパス</param>
        /// <returns>解凍したファイルパス</returns>
        public static string Extract(string filename, string archivePath)
        {
            string extractPath = Path.Combine(Path.GetDirectoryName(archivePath), filename);
            using (var stream = new FileStream(extractPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                Extract(filename, archivePath, stream);
            return extractPath;
        }

        /// <summary>
        /// アーカイブ内から指定ファイルを解凍する
        /// </summary>
        /// <param name="filename">解凍するファイル名</param>
        /// <param name="archivePath">ファイルのあるアーカイブへのパス</param>
        /// <param name="stream">解凍先ストリーム</param>
        public static void Extract(string filename, string archivePath, Stream stream)
        {
            using (var extractor = new SevenZipExtractor(archivePath))
                extractor.ExtractFile(filename, stream);
        }
    }
}
