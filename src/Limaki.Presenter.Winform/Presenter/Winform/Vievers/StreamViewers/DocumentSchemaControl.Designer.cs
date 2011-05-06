using Limaki.Presenter.Widgets;
using Limaki.Presenter.Winform.Display;
using System.Windows.Forms;
using System.Drawing;

using Limaki.Widgets;
using Limaki.Presenter.Display;
namespace Limaki.Presenter.Winform.Controls {
    partial class DocumentSchemaControl {
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

            WinImageDisplay = new WinformImageDisplay () {Dock = System.Windows.Forms.DockStyle.Fill, BackColor = Color.White};
            SplitContainer = new System.Windows.Forms.SplitContainer() { Dock = System.Windows.Forms.DockStyle.Fill };
            WidgetDisplay = new WinformWidgetDisplay() { Dock = System.Windows.Forms.DockStyle.Right, Width = Controller.GetDefaultWidth()};

            //SplitContainer.Panel1.Controls.Add (ImageDisplay);
            //SplitContainer.Panel2.Controls.Add(WidgetDisplay);
            //SplitContainer.SplitterDistance = this.Width - 40;
            //this.Controls.Add(SplitContainer);

            Splitter = new System.Windows.Forms.Splitter {Dock = DockStyle.Right};
            this.Controls.AddRange(new Control[] { Splitter, WidgetDisplay, WinImageDisplay });
            
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        public WinformWidgetDisplay WidgetDisplay { get; set; }
        public WinformImageDisplay WinImageDisplay { get; set; }
        System.Windows.Forms.SplitContainer SplitContainer { get; set; }
        System.Windows.Forms.Splitter Splitter { get; set; }

        public IGraphSceneDisplay<IWidget, IEdgeWidget> GraphSceneDisplay { get { return this.WidgetDisplay.Display as IGraphSceneDisplay<IWidget, IEdgeWidget>; } }
        public IDisplay<Image> ImageDisplay { get { return this.WinImageDisplay.Display; } }

        #endregion
    }
}
