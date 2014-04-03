namespace Limaki.View.SwfBackend.VidgetBackends {
    partial class TextViewerBackend {
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
            this.innerTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // innerTextBox
            // 
            this.innerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.innerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.innerTextBox.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.innerTextBox.Location = new System.Drawing.Point(0, 0);
            this.innerTextBox.Name = "innerTextBox";
            this.innerTextBox.Size = new System.Drawing.Size(354, 227);
            this.innerTextBox.TabIndex = 0;
            this.innerTextBox.Text = "";
            // 
            // TextViewerBackend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.innerTextBox);
            this.Name = "TextViewerBackend";
            this.Size = new System.Drawing.Size(354, 227);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox innerTextBox;
    }
}
