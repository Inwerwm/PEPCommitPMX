using CommitPMX;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.IO;

namespace CommitPmxTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void CommitLogSerializeTest()
        {
            var log = new CommitLog(DateTime.Now, "コミットテスト", "コミットテスト.pmx", CommitLog.ArchiveFormat.SevenZip, @"archive.7z");
            var jsonLog = JsonConvert.SerializeObject(log, Formatting.None);

            var dsLog = JsonConvert.DeserializeObject<CommitLog>(jsonLog);

            Assert.AreEqual(log, dsLog, "シリアライズの復元に失敗しました。");
        }
    }
}
