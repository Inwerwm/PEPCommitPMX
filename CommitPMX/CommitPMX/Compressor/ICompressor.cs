using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitPMX
{
    interface ICompressor
    {
        /// <summary>
        /// アーカイブの拡張子
        /// </summary>
        string ExtString { get; }

        SevenZip.OutArchiveFormat ArchiveFormat { get; }

        /// <summary>
        /// アーカイブにファイルを追加
        /// アーカイブが存在しなければ作る
        /// </summary>
        /// <param name="filePath">追加されるファイルへのパス</param>
        /// <param name="archivePath">追加対象アーカイブへのパス 拡張子は無しで渡すこと</param>
        void AddFileToArchive(string filePath, string archivePath);
    }
}
