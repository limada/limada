using ZoomState=Limaki.Actions.ZoomState;

namespace Limaki.Examples {
    partial class MainForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FileOpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.framePerSecondToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.framePerSecondHalfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.framePerSecondQuarterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawModeComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.selectorTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.styleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.mousePosLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.scrollPosLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.moveButton = new System.Windows.Forms.ToolStripButton();
            this.selectButton = new System.Windows.Forms.ToolStripButton();
            this.connectorButton = new System.Windows.Forms.ToolStripButton();
            this.AddWidgetButton = new System.Windows.Forms.ToolStripButton();
            this.zoomButton = new System.Windows.Forms.ToolStripSplitButton();
            this.zoomMenuFitToScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomMenuFittoWidth = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomMenuFittoHeigth = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomMenuOriginalSize = new System.Windows.Forms.ToolStripMenuItem();
            this.LayoutButton = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.display = new Limaki.Displays.WidgetDisplay();
            this.menuStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.testToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(384, 27);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            this.FileMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileOpenMenuItem,
            this.exitToolStripMenuItem});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(45, 23);
            this.FileMenu.Text = "File";
            // 
            // FileOpenMenuItem
            // 
            this.FileOpenMenuItem.Image = global::Limaki.Properties.Resources.MenuFileOpenIcon;
            this.FileOpenMenuItem.Name = "FileOpenMenuItem";
            this.FileOpenMenuItem.Size = new System.Drawing.Size(152, 24);
            this.FileOpenMenuItem.Text = "Open...";
            this.FileOpenMenuItem.Click += new System.EventHandler(this.fileOpenMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.framePerSecondToolStripMenuItem,
            this.framePerSecondHalfToolStripMenuItem,
            this.framePerSecondQuarterToolStripMenuItem,
            this.drawModeComboBox,
            this.selectorTestToolStripMenuItem,
            this.otherTestToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(51, 23);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // framePerSecondToolStripMenuItem
            // 
            this.framePerSecondToolStripMenuItem.Name = "framePerSecondToolStripMenuItem";
            this.framePerSecondToolStripMenuItem.Size = new System.Drawing.Size(273, 24);
            this.framePerSecondToolStripMenuItem.Text = "FramePerSecond (Full)";
            this.framePerSecondToolStripMenuItem.Click += new System.EventHandler(this.framePerSecondToolStripMenuItem_Click);
            // 
            // framePerSecondHalfToolStripMenuItem
            // 
            this.framePerSecondHalfToolStripMenuItem.Name = "framePerSecondHalfToolStripMenuItem";
            this.framePerSecondHalfToolStripMenuItem.Size = new System.Drawing.Size(273, 24);
            this.framePerSecondHalfToolStripMenuItem.Text = "FramePerSecond (Half)";
            this.framePerSecondHalfToolStripMenuItem.Click += new System.EventHandler(this.framePerSecondToolStripMenuItem_Click);
            // 
            // framePerSecondQuarterToolStripMenuItem
            // 
            this.framePerSecondQuarterToolStripMenuItem.Name = "framePerSecondQuarterToolStripMenuItem";
            this.framePerSecondQuarterToolStripMenuItem.Size = new System.Drawing.Size(273, 24);
            this.framePerSecondQuarterToolStripMenuItem.Text = "FramePerSecond (Quarter))";
            this.framePerSecondQuarterToolStripMenuItem.Click += new System.EventHandler(this.framePerSecondToolStripMenuItem_Click);
            // 
            // drawModeComboBox
            // 
            this.drawModeComboBox.Name = "drawModeComboBox";
            this.drawModeComboBox.Size = new System.Drawing.Size(121, 27);
            // 
            // selectorTestToolStripMenuItem
            // 
            this.selectorTestToolStripMenuItem.Name = "selectorTestToolStripMenuItem";
            this.selectorTestToolStripMenuItem.Size = new System.Drawing.Size(273, 24);
            this.selectorTestToolStripMenuItem.Text = "SelectorTest";
            this.selectorTestToolStripMenuItem.Click += new System.EventHandler(this.selectorTestToolStripMenuItem_Click);
            // 
            // otherTestToolStripMenuItem
            // 
            this.otherTestToolStripMenuItem.Name = "otherTestToolStripMenuItem";
            this.otherTestToolStripMenuItem.Size = new System.Drawing.Size(273, 24);
            this.otherTestToolStripMenuItem.Text = "OtherTest";
            this.otherTestToolStripMenuItem.Click += new System.EventHandler(this.otherTestToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.layoutToolStripMenuItem,
            this.styleToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(76, 23);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // layoutToolStripMenuItem
            // 
            this.layoutToolStripMenuItem.Name = "layoutToolStripMenuItem";
            this.layoutToolStripMenuItem.Size = new System.Drawing.Size(140, 24);
            this.layoutToolStripMenuItem.Text = "Layout...";
            this.layoutToolStripMenuItem.Click += new System.EventHandler(this.layoutToolStripMenuItem_Click);
            // 
            // styleToolStripMenuItem
            // 
            this.styleToolStripMenuItem.Name = "styleToolStripMenuItem";
            this.styleToolStripMenuItem.Size = new System.Drawing.Size(140, 24);
            this.styleToolStripMenuItem.Text = "Style...";
            this.styleToolStripMenuItem.Click += new System.EventHandler(this.styleToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(64, 23);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mousePosLabel,
            this.scrollPosLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(384, 24);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // mousePosLabel
            // 
            this.mousePosLabel.Name = "mousePosLabel";
            this.mousePosLabel.Size = new System.Drawing.Size(32, 19);
            this.mousePosLabel.Text = "0,0";
            // 
            // scrollPosLabel
            // 
            this.scrollPosLabel.Name = "scrollPosLabel";
            this.scrollPosLabel.Size = new System.Drawing.Size(32, 19);
            this.scrollPosLabel.Text = "0,0";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveButton,
            this.selectButton,
            this.connectorButton,
            this.AddWidgetButton,
            this.zoomButton,
            this.LayoutButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 27);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(157, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // moveButton
            // 
            this.moveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveButton.Image = global::Limaki.Properties.Resources.MoveToolIcon;
            this.moveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(23, 22);
            this.moveButton.Text = "Move";
            this.moveButton.Click += new System.EventHandler(this.selectOrMoveButton_Click);
            // 
            // selectButton
            // 
            this.selectButton.Checked = true;
            this.selectButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.selectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.selectButton.Image = ((System.Drawing.Image)(resources.GetObject("selectButton.Image")));
            this.selectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(23, 22);
            this.selectButton.Text = "Select";
            this.selectButton.Click += new System.EventHandler(this.selectOrMoveButton_Click);
            // 
            // connectorButton
            // 
            this.connectorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.connectorButton.Image = global::Limaki.Properties.Resources.ConnectorToolIcon;
            this.connectorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectorButton.Name = "connectorButton";
            this.connectorButton.Size = new System.Drawing.Size(23, 22);
            this.connectorButton.Text = "Add Link";
            this.connectorButton.Click += new System.EventHandler(this.selectOrMoveButton_Click);
            // 
            // AddWidgetButton
            // 
            this.AddWidgetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddWidgetButton.Image = global::Limaki.Properties.Resources.Shape;
            this.AddWidgetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddWidgetButton.Name = "AddWidgetButton";
            this.AddWidgetButton.Size = new System.Drawing.Size(23, 22);
            this.AddWidgetButton.Text = "Add Shape";
            this.AddWidgetButton.Click += new System.EventHandler(this.selectOrMoveButton_Click);
            // 
            // zoomButton
            // 
            this.zoomButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.zoomButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomMenuFitToScreen,
            this.zoomMenuFittoWidth,
            this.zoomMenuFittoHeigth,
            this.zoomMenuOriginalSize});
            this.zoomButton.Image = global::Limaki.Properties.Resources.ZoomToolIcon;
            this.zoomButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomButton.Name = "zoomButton";
            this.zoomButton.Size = new System.Drawing.Size(32, 22);
            this.zoomButton.Text = "Zoom";
            this.zoomButton.ButtonClick += new System.EventHandler(this.zoomButton_ButtonClick);
            // 
            // zoomMenuFitToScreen
            // 
            this.zoomMenuFitToScreen.CheckOnClick = true;
            this.zoomMenuFitToScreen.Name = "zoomMenuFitToScreen";
            this.zoomMenuFitToScreen.Size = new System.Drawing.Size(167, 24);
            this.zoomMenuFitToScreen.Text = "Fit to Screen";
            this.zoomMenuFitToScreen.Click += new System.EventHandler(this.zoomButton_DropDownItemClicked);
            // 
            // zoomMenuFittoWidth
            // 
            this.zoomMenuFittoWidth.CheckOnClick = true;
            this.zoomMenuFittoWidth.Name = "zoomMenuFittoWidth";
            this.zoomMenuFittoWidth.Size = new System.Drawing.Size(167, 24);
            this.zoomMenuFittoWidth.Text = "Fit to Width";
            this.zoomMenuFittoWidth.Click += new System.EventHandler(this.zoomButton_DropDownItemClicked);
            // 
            // zoomMenuFittoHeigth
            // 
            this.zoomMenuFittoHeigth.CheckOnClick = true;
            this.zoomMenuFittoHeigth.Name = "zoomMenuFittoHeigth";
            this.zoomMenuFittoHeigth.Size = new System.Drawing.Size(167, 24);
            this.zoomMenuFittoHeigth.Text = "Fit to Heigth";
            this.zoomMenuFittoHeigth.Click += new System.EventHandler(this.zoomButton_DropDownItemClicked);
            // 
            // zoomMenuOriginalSize
            // 
            this.zoomMenuOriginalSize.Name = "zoomMenuOriginalSize";
            this.zoomMenuOriginalSize.Size = new System.Drawing.Size(167, 24);
            this.zoomMenuOriginalSize.Text = "Original Size";
            this.zoomMenuOriginalSize.Click += new System.EventHandler(this.zoomButton_DropDownItemClicked);
            // 
            // LayoutButton
            // 
            this.LayoutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.LayoutButton.Image = ((System.Drawing.Image)(resources.GetObject("LayoutButton.Image")));
            this.LayoutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LayoutButton.Name = "LayoutButton";
            this.LayoutButton.Size = new System.Drawing.Size(23, 22);
            this.LayoutButton.Text = "Layout";
            this.LayoutButton.Click += new System.EventHandler(this.LayoutButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.display);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(384, 231);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(384, 307);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // display
            // 
            this.display.AllowDrop = true;
            this.display.AutoScroll = true;
            this.display.AutoScrollMinSize = new System.Drawing.Size(-2147483648, -2147483648);
            this.display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.display.Location = new System.Drawing.Point(0, 0);
            this.display.Name = "display";
            this.display.ScrollPosition = new System.Drawing.Point(0, 0);
            this.display.Size = new System.Drawing.Size(384, 231);
            this.display.TabIndex = 0;
            this.display.ZoomFactor = float.PositiveInfinity;
            this.display.ZoomState = Limaki.Actions.ZoomState.FitToScreen;
            this.display.MouseMove += new System.Windows.Forms.MouseEventHandler(this.display_MouseMove);
            this.display.Paint += new System.Windows.Forms.PaintEventHandler(this.display_Paint);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 307);
            this.Controls.Add(this.toolStripContainer1);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Limaki.Graphics Examples";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
#if Widget
        private Limaki.Displays.WidgetDisplay display;
#else
        private Limaki.Displays.ImageDisplay display;
#endif
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel mousePosLabel;
        private System.Windows.Forms.ToolStripStatusLabel scrollPosLabel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton moveButton;
        private System.Windows.Forms.ToolStripButton selectButton;
        private System.Windows.Forms.ToolStripSplitButton zoomButton;
        private System.Windows.Forms.ToolStripMenuItem zoomMenuFitToScreen;
        private System.Windows.Forms.ToolStripMenuItem zoomMenuFittoWidth;
        private System.Windows.Forms.ToolStripMenuItem zoomMenuFittoHeigth;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem FileOpenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem framePerSecondToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem framePerSecondHalfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem framePerSecondQuarterToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox drawModeComboBox;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectorTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomMenuOriginalSize;
        private System.Windows.Forms.ToolStripButton connectorButton;
        private System.Windows.Forms.ToolStripMenuItem otherTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton AddWidgetButton;
        private System.Windows.Forms.ToolStripButton LayoutButton;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem styleToolStripMenuItem;
    }
}