using Limaki.Swf.Backends.Viewers.ToolStrips;
namespace Limaki.Swf.Backends.TextEditor {
    partial class TextBoxEditorToolStrip {
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
            components = new System.ComponentModel.Container ();
            this.boldButton = new ToolStripButtonEx ();
            this.italicButton = new ToolStripButtonEx();
            this.underlineButton = new ToolStripButtonEx();
            this.fontSizeComboBox = new System.Windows.Forms.ToolStripComboBox ();
            this.fontComboBox = new System.Windows.Forms.ToolStripComboBox();

            this.boldButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.boldButton.Image = Limaki.View.Properties.Iconery.FontBoldIcon;
            this.boldButton.Name = "boldButton";
            this.boldButton.Size = new System.Drawing.Size(36, 36);
            this.boldButton.Text = "Bold";
            this.boldButton.ToolTipText = "Bold";

            this.italicButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.italicButton.Image = Limaki.View.Properties.Iconery.FontItalicIcon;
            this.italicButton.Name = "italicButton";
            this.italicButton.Size = new System.Drawing.Size(36, 36); 
            this.italicButton.Text = "Italic";
            this.italicButton.ToolTipText = "Italic";

            this.underlineButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.underlineButton.Image = Limaki.View.Properties.Iconery.FontUnderlineIcon;
            this.underlineButton.Name = "underlineButton";
            this.underlineButton.Size = new System.Drawing.Size(36, 36);
            this.underlineButton.Text = "underline";
            this.underlineButton.ToolTipText = "Underline";
            //this.underlineButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.SizeToFit;

            this.fontSizeComboBox.AutoSize = false;
            this.fontSizeComboBox.DropDownWidth = 60;
            this.fontSizeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fontSizeComboBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fontSizeComboBox.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "11",
            "12",
            "14",
            "16",
            "18",
            "20",
            "22",
            "24",
            "28",
            "36"});
            this.fontSizeComboBox.Margin = new System.Windows.Forms.Padding(0);
            this.fontSizeComboBox.Name = "fontSizeComboBox";
            this.fontSizeComboBox.Size = new System.Drawing.Size(60, 25);


            this.fontComboBox.Name = "fontComboBox";
            this.fontComboBox.DropDownWidth = 120;
            this.fontComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fontComboBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fontComboBox.Size = new System.Drawing.Size(120, 25);


            this.Items.AddRange (new System.Windows.Forms.ToolStripItem[] {
            this.fontComboBox,
            this.fontSizeComboBox,
            this.boldButton, 
            this.italicButton,
            this.underlineButton,
            
            });
        }

        public ToolStripButtonEx boldButton;
        public ToolStripButtonEx italicButton;
        public ToolStripButtonEx underlineButton;
        public System.Windows.Forms.ToolStripComboBox fontSizeComboBox;
        public System.Windows.Forms.ToolStripComboBox fontComboBox;
        #endregion
    }
}
