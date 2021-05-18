using Newtonsoft.Json;
using PEPlugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommitPMX
{
    internal partial class FormReconstruction : Form
    {
        IPERunArgs Args { get; }
        SevenZipCompressor Compressor { get; }
        CommitLog? SelectedCommitLog => (CommitLog)dataGridViewCommits.SelectedRows[0].DataBoundItem;

        public FormReconstruction(IPERunArgs args, SevenZipCompressor compressor)
        {
            InitializeComponent();

            Args = args;
            Compressor = compressor;
        }

        private void FormReconstruction_Load(object sender, EventArgs e)
        {
            var commitDir = Commit.BuildCommitDirectryPath(Args.Host.Connector.Pmx.CurrentPath);

            var logFile = File.ReadLines(Path.Combine(commitDir, Commit.LogFileName));
            dataGridViewCommits.DataSource = logFile.Select(JsonConvert.DeserializeObject<CommitLog>).Reverse().ToArray();
            dataGridViewCommits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void buttonExtract_Click(object sender, EventArgs e)
        {

        }

        private void buttonOverwrite_Click(object sender, EventArgs e)
        {
        }
    }
}
