using Limaki.Common;
using Limaki.Contents;
using Limaki.Contents.IO;
using Limaki.View.Vidgets;
using System;
using System.IO;

namespace Limaki.Usecases {

    public class IoUiManager:IProgress  {

        public FileDialogMemento OpenFileDialog { get; set; }
        public FileDialogMemento SaveFileDialog { get; set; }

        public Func<string, string, MessageBoxButtons, DialogResult> MessageBoxShow { get; set; }
        /// <summary>
        /// shows the file dialog; set true if Dialog should be a OpenFileDialog 
        /// </summary>
        public Func<FileDialogMemento, bool, DialogResult> FileDialogShow { get; set; }

        public void DefaultDialogValues (FileDialogMemento dialog, string filter) {
            dialog.Filter = filter + "All Files|*.*";
            dialog.DefaultExt = "";
            dialog.OverwritePrompt = true;
        }

        public virtual Action<string, int, int> Progress { get; set; }
        protected virtual void OnProgress (string m, int i, int count) {
            Progress?.Invoke (m, i, count);
        }
    }
}