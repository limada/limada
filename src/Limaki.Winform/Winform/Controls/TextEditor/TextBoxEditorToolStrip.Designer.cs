namespace Limaki.Winform.Controls.TextEditor {
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
            this.boldButton = new System.Windows.Forms.ToolStripButton ();
            this.italicButton = new System.Windows.Forms.ToolStripButton();
            this.underlineButton = new System.Windows.Forms.ToolStripButton();
            this.fontSizeComboBox = new System.Windows.Forms.ToolStripComboBox ();
            this.fontComboBox = new System.Windows.Forms.ToolStripComboBox();

            this.boldButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.boldButton.Image = global::Limaki.Winform.Controls.TextEditor.TextBoxEditorResources.FontBoldIcon;
            this.boldButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.boldButton.Name = "boldButton";
            this.boldButton.Size = new System.Drawing.Size(24, 24);
            this.boldButton.Text = "Bold";
            this.boldButton.ToolTipText = "Bold";

            this.italicButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.italicButton.Image = global::Limaki.Winform.Controls.TextEditor.TextBoxEditorResources.FontItalicIcon;
            this.italicButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.italicButton.Name = "italicButton";
            this.italicButton.Size = new System.Drawing.Size(24, 24);
            this.italicButton.Text = "Italic";
            this.italicButton.ToolTipText = "Italic";

            this.underlineButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.underlineButton.Image = global::Limaki.Winform.Controls.TextEditor.TextBoxEditorResources.FontUnderlineIcon;
            this.underlineButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.underlineButton.Name = "underlineButton";
            this.underlineButton.Size = new System.Drawing.Size(24, 24);
            this.underlineButton.Text = "underline";
            this.underlineButton.ToolTipText = "Underline";


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

        public System.Windows.Forms.ToolStripButton boldButton;
        public System.Windows.Forms.ToolStripButton italicButton;
        public System.Windows.Forms.ToolStripButton underlineButton;
        public System.Windows.Forms.ToolStripComboBox fontSizeComboBox;
        public System.Windows.Forms.ToolStripComboBox fontComboBox;
        #endregion
    }
}
