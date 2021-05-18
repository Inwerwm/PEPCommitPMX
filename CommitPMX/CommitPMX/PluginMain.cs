using PEPlugin;
using System;
using System.Windows.Forms;

namespace CommitPMX
{
    public class CommitPMX : IPEPlugin
    {
        private bool disposedValue;

        public string Name => "CommitPMX";

        public string Version => "1.0";

        public string Description => "CommitPMX";

        public IPEPluginOption Option => new PEPluginOption(false, true);

        private FormControl form;

        public void Run(IPERunArgs args)
        {
            try
            {
                if (form == null)
                {
                    form = new FormControl(args);
                    form.Visible = true;
                }
                else
                {
                    form.Visible = !form.Visible;
                    if (form.Visible)
                        form.Reload();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~Class1()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
