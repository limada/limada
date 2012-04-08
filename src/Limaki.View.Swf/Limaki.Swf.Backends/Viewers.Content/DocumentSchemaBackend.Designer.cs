using System.Windows.Forms;
using System.Drawing;
using Limaki.View.Display;
using Limaki.View.Swf.Display;
using Limaki.Visuals;

namespace Limaki.View.Swf.Backends {
    partial class DocumentSchemaBackend {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.SuspendLayout();

            WinImageDisplayBackend = new SwfImageDisplayBackend () {Dock = System.Windows.Forms.DockStyle.Fill, BackColor = Color.White};
            
            WinVisualsDisplayBackend = new SwfVisualsDisplayBackend() { Dock = System.Windows.Forms.DockStyle.Right, Width = Controller.GetDefaultWidth()};

            //SplitContainer = new System.Windows.Forms.SplitContainer() { Dock = System.Windows.Forms.DockStyle.Fill };
            //SplitContainer.Panel1.Controls.Add (WinImageDisplay);
            //SplitContainer.Panel2.Controls.Add(WinVisualsDisplay);
            //SplitContainer.SplitterDistance = this.Width - 40;
            //this.Controls.Add(SplitContainer);

            Splitter = new System.Windows.Forms.Splitter {Dock = DockStyle.Right};
            this.Controls.AddRange(new Control[] { WinImageDisplayBackend, Splitter, WinVisualsDisplayBackend });
            
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        public SwfVisualsDisplayBackend WinVisualsDisplayBackend { get; set; }
        public SwfImageDisplayBackend WinImageDisplayBackend { get; set; }
        System.Windows.Forms.SplitContainer SplitContainer { get; set; }
        System.Windows.Forms.Splitter Splitter { get; set; }

        public IGraphSceneDisplay<IVisual, IVisualEdge> GraphSceneDisplay { get { return this.WinVisualsDisplayBackend.Display as IGraphSceneDisplay<IVisual, IVisualEdge>; } }
        public IDisplay<Image> ImageDisplay { get { return this.WinImageDisplayBackend.Display; } }

        #endregion
    }
}
