namespace Limaki.Tests.Display {
    partial class QuadTreeVisualizer {
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent() {
            this.widgetDisplay = new Limaki.Winform.Displays.WidgetDisplay();
            this.SuspendLayout();
            // 
            // widgetDisplay
            // 
            this.widgetDisplay.AllowDrop = true;
            this.widgetDisplay.AutoScroll = true;
            this.widgetDisplay.AutoScrollMinSize = new System.Drawing.Size(-2147483648, -2147483648);
            this.widgetDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.widgetDisplay.Location = new System.Drawing.Point(0, 0);
            this.widgetDisplay.Name = "widgetDisplay";
            this.widgetDisplay.ScrollPosition = new System.Drawing.Point(0, 0);
            this.widgetDisplay.Size = new System.Drawing.Size(292, 266);
            this.widgetDisplay.TabIndex = 0;
            this.widgetDisplay.ZoomFactor = float.PositiveInfinity;
            this.widgetDisplay.ZoomState = Limaki.Actions.ZoomState.FitToScreen;
            this.widgetDisplay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.widgetDisplay_KeyPress);
            // 
            // QuadTreeVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.widgetDisplay);
            this.Name = "QuadTreeVisualizer";
            this.Text = "QuadTreeVisualizer";
            this.ResumeLayout(false);

        }

        #endregion

        private Limaki.Winform.Displays.WidgetDisplay widgetDisplay;
    }
}