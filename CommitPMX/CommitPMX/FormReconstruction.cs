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

        public FormReconstruction(IPERunArgs args, SevenZipCompressor compressor)
        {
            InitializeComponent();

            Args = args;
            Compressor = compressor;
        }

        private void FormReconstruction_Load(object sender, EventArgs e)
        {
            var commitDir = Commit.BuildCommitDirectryPath(Args.Host.Connector.Pmx.CurrentPath);

        }
    }
}
