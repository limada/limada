using Limaki.Model.Content;
using Limaki.Model.Content.IO;
using Limaki.Viewers;
using System;
using System.IO;

namespace Limaki.Usecases {

    public class IoUiManager  {

        public FileDialogMemento OpenFileDialog { get; set; }
        public FileDialogMemento SaveFileDialog { get; set; }

        public Func<string, string, MessageBoxButtons, DialogResult> MessageBoxShow { get; set; }
        public Func<FileDialogMemento, bool, DialogResult> FileDialogShow { get; set; }

        public void DefaultDialogValues (FileDialogMemento dialog, string filter) {
            dialog.Filter = filter + "All Files|*.*";
            dialog.DefaultExt = "";
            dialog.AddExtension = true;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.AutoUpgradeEnabled = true;
            dialog.SupportMultiDottedExtensions = true;
            // Important! under windows this adds the fileextension!
            dialog.ValidateNames = true;

        }

        public virtual Action<string, int, int> Progress { get; set; }
        protected virtual void OnProgress (string m, int i, int count) {
            if (Progress != null)
                Progress(m, i, count);
        }
    }
}