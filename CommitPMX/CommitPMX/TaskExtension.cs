using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommitPMX
{
    static class TaskExtension
    {
        public static async Task InvokeAsyncWithExportException(this Task task, string commitDirectory, string exDesc)
        {
            await task.InvokeAsyncWithExportException(commitDirectory, exDesc, null);
        }

        public static async Task InvokeAsyncWithExportException(this Task task, string commitDirectory, string exDesc, Action finallyAction)
        {
            (string Value, bool HasValue) exception = (null, false);
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                exception.Value = $"========================================{Environment.NewLine}" +
                                  $"{DateTime.Now:G}{Environment.NewLine}" +
                                  $"{ex.GetType()}{Environment.NewLine}" +
                                  $"{exDesc}{Environment.NewLine}" +
                                  $"{ex.Message}{Environment.NewLine}" +
                                  $"{ex.StackTrace}{Environment.NewLine}";
                exception.HasValue = true;
                MessageBox.Show($"アーカイブへの追加に失敗しました。{Environment.NewLine}{ex.Message}", "コミットの失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                finallyAction?.Invoke();
            }

            try
            {
                if (exception.HasValue)
                {
                    File.AppendAllText(Path.Combine(commitDirectory, "Exceptions.log"), exception.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"例外履歴の書込に失敗しました。{Environment.NewLine}{ex.Message}", "例外履歴書込の失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
