﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitPMX
{
    interface ICompressor
    {
        /// <summary>
        /// アーカイブにファイルを追加
        /// </summary>
        /// <param name="filePath">追加されるファイルへのパス</param>
        /// <param name="archivePath">追加対象アーカイブへのパス</param>
        void AddFileToArchive(string filePath, string archivePath);
    }
}