using PointI=Limaki.Drawing.PointI;
using ToolStripShapeComboBox=Limaki.Winform.Controls.ToolStripShapeComboBox;
using ZoomState=Limaki.Drawing.ZoomState;

namespace Limaki.App {
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
            this.streamViewControler.Dispose();
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
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.styleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileVerticallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleViewsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tiledGraphDocumentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.scrollPosLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.sceneTools = new System.Windows.Forms.ToolStrip();
            this.selectButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.LayoutButton = new System.Windows.Forms.ToolStripButton();
            this.zoomButton = new System.Windows.Forms.ToolStripSplitButton();
            this.zoomMenuFitToScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomMenuFittoWidth = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomMenuFittoHeigth = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomMenuOriginalSize = new System.Windows.Forms.ToolStripMenuItem();
            this.moveButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.AddWidgetButton = new System.Windows.Forms.ToolStripButton();
            this.connectorButton = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.Display1 = new Limaki.Winform.Displays.WidgetDisplay();
            this.Display2 = new Limaki.Winform.Displays.WidgetDisplay();
            this.viewToolStrip = new System.Windows.Forms.ToolStrip();
            this.graphDocViewButton = new System.Windows.Forms.ToolStripButton();
            this.graphGraphViewButton = new System.Windows.Forms.ToolStripButton();
            this.toggleViewButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.goBackButton = new System.Windows.Forms.ToolStripButton();
            this.goForwardButton = new System.Windows.Forms.ToolStripButton();
            this.goHomeButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.newSheetButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.styleTools = new System.Windows.Forms.ToolStrip();
            this.styleComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.fontComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.fontSizeComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.markerStrip = new System.Windows.Forms.ToolStrip();
            this.markerCombo = new System.Windows.Forms.ToolStripComboBox();
            this.layoutTools = new System.Windows.Forms.ToolStrip();
            this.styleSheetCombo = new System.Windows.Forms.ToolStripComboBox();
            this.shapeCombo = new Limaki.Winform.Controls.ToolStripShapeComboBox();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.exportPagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.sceneTools.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.viewToolStrip.SuspendLayout();
            this.styleTools.SuspendLayout();
            this.markerStrip.SuspendLayout();
            this.layoutTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.editToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(632, 27);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            this.FileMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.saveSheetToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(45, 23);
            this.FileMenu.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(194, 24);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(194, 24);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveFile_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(194, 24);
            this.exportToolStripMenuItem.Text = "Save as image...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // saveSheetToolStripMenuItem
            // 
            this.saveSheetToolStripMenuItem.Name = "saveSheetToolStripMenuItem";
            this.saveSheetToolStripMenuItem.Size = new System.Drawing.Size(194, 24);
            this.saveSheetToolStripMenuItem.Text = "Save sheet ...";
            this.saveSheetToolStripMenuItem.Click += new System.EventHandler(this.saveSheetToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(194, 24);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ApplicationExit_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.searchToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(48, 23);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(125, 24);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.Copy_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(125, 24);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.Paste_Click);
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(125, 24);
            this.searchToolStripMenuItem.Text = "Search";
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.layoutToolStripMenuItem,
            this.styleToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(71, 23);
            this.optionsToolStripMenuItem.Text = "Format";
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
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tileVerticallyToolStripMenuItem,
            this.toggleViewsToolStripMenuItem,
            this.tiledGraphDocumentToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(55, 23);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // tileVerticallyToolStripMenuItem
            // 
            this.tileVerticallyToolStripMenuItem.Name = "tileVerticallyToolStripMenuItem";
            this.tileVerticallyToolStripMenuItem.Size = new System.Drawing.Size(209, 24);
            this.tileVerticallyToolStripMenuItem.Text = "Tiled Graph";
            this.tileVerticallyToolStripMenuItem.Click += new System.EventHandler(this.tileGraphGraphToolStripMenuItem_Click);
            // 
            // toggleViewsToolStripMenuItem
            // 
            this.toggleViewsToolStripMenuItem.Name = "toggleViewsToolStripMenuItem";
            this.toggleViewsToolStripMenuItem.Size = new System.Drawing.Size(209, 24);
            this.toggleViewsToolStripMenuItem.Text = "Toggle";
            this.toggleViewsToolStripMenuItem.Click += new System.EventHandler(this.toggleViewsToolStripMenuItem_Click);
            // 
            // tiledGraphDocumentToolStripMenuItem
            // 
            this.tiledGraphDocumentToolStripMenuItem.Name = "tiledGraphDocumentToolStripMenuItem";
            this.tiledGraphDocumentToolStripMenuItem.Size = new System.Drawing.Size(209, 24);
            this.tiledGraphDocumentToolStripMenuItem.Text = "Graph - Document";
            this.tiledGraphDocumentToolStripMenuItem.Click += new System.EventHandler(this.tiledGraphDocumentToolStripMenuItem_Click);
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
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scrollPosLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(632, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // scrollPosLabel
            // 
            this.scrollPosLabel.Font = new System.Drawing.Font("Tahoma", 10F);
            this.scrollPosLabel.Name = "scrollPosLabel";
            this.scrollPosLabel.Size = new System.Drawing.Size(28, 17);
            this.scrollPosLabel.Text = "0,0";
            // 
            // sceneTools
            // 
            this.sceneTools.Dock = System.Windows.Forms.DockStyle.None;
            this.sceneTools.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.sceneTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectButton,
            this.toolStripSeparator2,
            this.LayoutButton,
            this.zoomButton,
            this.moveButton,
            this.toolStripSeparator1,
            this.AddWidgetButton,
            this.connectorButton});
            this.sceneTools.Location = new System.Drawing.Point(3, 27);
            this.sceneTools.Name = "sceneTools";
            this.sceneTools.Size = new System.Drawing.Size(178, 27);
            this.sceneTools.TabIndex = 2;
            this.sceneTools.Text = "Scene Tools";
            // 
            // selectButton
            // 
            this.selectButton.Checked = true;
            this.selectButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.selectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.selectButton.Image = global::Limaki.Properties.Resources.DrawSelection;
            this.selectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(24, 24);
            this.selectButton.Text = "Select";
            this.selectButton.Click += new System.EventHandler(this.selectOrMoveButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // LayoutButton
            // 
            this.LayoutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.LayoutButton.Image = global::Limaki.Properties.Resources.ModifyLayout24;
            this.LayoutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LayoutButton.Name = "LayoutButton";
            this.LayoutButton.Size = new System.Drawing.Size(24, 24);
            this.LayoutButton.Text = "Layout";
            this.LayoutButton.ToolTipText = "arrange";
            this.LayoutButton.Click += new System.EventHandler(this.LayoutButton_Click);
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
            this.zoomButton.Size = new System.Drawing.Size(36, 24);
            this.zoomButton.Text = "Zoom";
            this.zoomButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.zoomButton_MouseDown);
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
            // moveButton
            // 
            this.moveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveButton.Image = global::Limaki.Properties.Resources.MoveShift;
            this.moveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(24, 24);
            this.moveButton.Text = "Move";
            this.moveButton.ToolTipText = "panning";
            this.moveButton.Click += new System.EventHandler(this.selectOrMoveButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // AddWidgetButton
            // 
            this.AddWidgetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddWidgetButton.Image = global::Limaki.Properties.Resources.CreateWidget24;
            this.AddWidgetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddWidgetButton.Name = "AddWidgetButton";
            this.AddWidgetButton.Size = new System.Drawing.Size(24, 24);
            this.AddWidgetButton.Text = "Add Shape";
            this.AddWidgetButton.ToolTipText = "Add Widget";
            this.AddWidgetButton.Click += new System.EventHandler(this.selectOrMoveButton_Click);
            // 
            // connectorButton
            // 
            this.connectorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.connectorButton.Image = global::Limaki.Properties.Resources.StraightConnector;
            this.connectorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectorButton.Name = "connectorButton";
            this.connectorButton.Size = new System.Drawing.Size(24, 24);
            this.connectorButton.Text = "Add Link";
            this.connectorButton.ToolTipText = "connect";
            this.connectorButton.Click += new System.EventHandler(this.selectOrMoveButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.CheckFileExists = false;
            this.openFileDialog.InitialDirectory = "E:\\testdata\\txbProjekt\\Limaki";
            this.openFileDialog.RestoreDirectory = true;
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
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(632, 276);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(632, 406);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.sceneTools);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.viewToolStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.styleTools);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.markerStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.layoutTools);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.Display1);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.Display2);
            this.splitContainer.Panel2MinSize = 0;
            this.splitContainer.Size = new System.Drawing.Size(632, 276);
            this.splitContainer.SplitterDistance = 308;
            this.splitContainer.SplitterWidth = 3;
            this.splitContainer.TabIndex = 1;
            // 
            // Display1
            // 
            this.Display1.AllowDrop = true;
            this.Display1.AutoScroll = true;
            this.Display1.AutoScrollMinSize = new System.Drawing.Size(-2147483648, -2147483648);
            this.Display1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Display1.Location = new System.Drawing.Point(0, 0);
            this.Display1.Name = "Display1";
            this.Display1.SceneId = ((long)(0));
            this.Display1.ScrollPosition = new PointI(0, 0);
            this.Display1.Size = new System.Drawing.Size(308, 276);
            this.Display1.TabIndex = 1;
            this.Display1.ZoomFactor = float.PositiveInfinity;
            this.Display1.ZoomState = Limaki.Drawing.ZoomState.FitToScreen;
            // 
            // Display2
            // 
            this.Display2.AllowDrop = true;
            this.Display2.AutoScroll = true;
            this.Display2.AutoScrollMinSize = new System.Drawing.Size(-2147483648, -2147483648);
            this.Display2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Display2.Location = new System.Drawing.Point(0, 0);
            this.Display2.Margin = new System.Windows.Forms.Padding(2);
            this.Display2.Name = "Display2";
            this.Display2.SceneId = ((long)(0));
            this.Display2.ScrollPosition = new PointI(0, 0);
            this.Display2.Size = new System.Drawing.Size(321, 276);
            this.Display2.TabIndex = 0;
            this.Display2.ZoomFactor = float.PositiveInfinity;
            this.Display2.ZoomState = Limaki.Drawing.ZoomState.FitToScreen;
            // 
            // viewToolStrip
            // 
            this.viewToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.viewToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.viewToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.graphDocViewButton,
            this.graphGraphViewButton,
            this.toggleViewButton,
            this.toolStripSeparator3,
            this.goBackButton,
            this.goForwardButton,
            this.goHomeButton,
            this.toolStripSeparator4,
            this.newSheetButton,
            this.toolStripButton1});
            this.viewToolStrip.Location = new System.Drawing.Point(3, 54);
            this.viewToolStrip.Name = "viewToolStrip";
            this.viewToolStrip.Size = new System.Drawing.Size(214, 27);
            this.viewToolStrip.TabIndex = 6;
            // 
            // graphDocViewButton
            // 
            this.graphDocViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.graphDocViewButton.Image = global::Limaki.Properties.Resources.GraphDocView;
            this.graphDocViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.graphDocViewButton.Name = "graphDocViewButton";
            this.graphDocViewButton.Size = new System.Drawing.Size(24, 24);
            this.graphDocViewButton.Text = "show stream contents";
            this.graphDocViewButton.Click += new System.EventHandler(this.tiledGraphDocumentToolStripMenuItem_Click);
            // 
            // graphGraphViewButton
            // 
            this.graphGraphViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.graphGraphViewButton.Image = global::Limaki.Properties.Resources.GraphGraphView;
            this.graphGraphViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.graphGraphViewButton.Name = "graphGraphViewButton";
            this.graphGraphViewButton.Size = new System.Drawing.Size(24, 24);
            this.graphGraphViewButton.Text = "show tiled graph";
            this.graphGraphViewButton.Click += new System.EventHandler(this.tileGraphGraphToolStripMenuItem_Click);
            // 
            // toggleViewButton
            // 
            this.toggleViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toggleViewButton.Image = global::Limaki.Properties.Resources.ToggleView;
            this.toggleViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toggleViewButton.Name = "toggleViewButton";
            this.toggleViewButton.Size = new System.Drawing.Size(24, 24);
            this.toggleViewButton.Text = "toggle view";
            this.toggleViewButton.Click += new System.EventHandler(this.toggleViewsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // goBackButton
            // 
            this.goBackButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.goBackButton.Image = global::Limaki.Properties.Resources.go_previous;
            this.goBackButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.goBackButton.Name = "goBackButton";
            this.goBackButton.Size = new System.Drawing.Size(24, 24);
            this.goBackButton.Text = "toolStripButton2";
            this.goBackButton.ToolTipText = "navigate back";
            this.goBackButton.Click += new System.EventHandler(this.goBackButton_Click);
            // 
            // goForwardButton
            // 
            this.goForwardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.goForwardButton.Image = global::Limaki.Properties.Resources.go_next;
            this.goForwardButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.goForwardButton.Name = "goForwardButton";
            this.goForwardButton.Size = new System.Drawing.Size(24, 24);
            this.goForwardButton.Text = "toolStripButton2";
            this.goForwardButton.ToolTipText = "navigate forward";
            this.goForwardButton.Click += new System.EventHandler(this.goForwardButton_Click);
            // 
            // goHomeButton
            // 
            this.goHomeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.goHomeButton.Image = global::Limaki.Properties.Resources.gohome;
            this.goHomeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.goHomeButton.Name = "goHomeButton";
            this.goHomeButton.Size = new System.Drawing.Size(24, 24);
            this.goHomeButton.Text = "toolStripButton1";
            this.goHomeButton.ToolTipText = "go to favorites";
            this.goHomeButton.Click += new System.EventHandler(this.goHomeButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
            // 
            // newSheetButton
            // 
            this.newSheetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newSheetButton.Image = global::Limaki.Properties.Resources.document_new;
            this.newSheetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newSheetButton.Name = "newSheetButton";
            this.newSheetButton.Size = new System.Drawing.Size(24, 24);
            this.newSheetButton.Text = "toolStripButton1";
            this.newSheetButton.ToolTipText = "new sheet";
            this.newSheetButton.Click += new System.EventHandler(this.newSheetButton_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::Limaki.Properties.Resources.stream_save;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton1.Text = "saveSheetButton";
            this.toolStripButton1.Click += new System.EventHandler(this.saveSheetToolStripMenuItem_Click);
            // 
            // styleTools
            // 
            this.styleTools.Dock = System.Windows.Forms.DockStyle.None;
            this.styleTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.styleComboBox,
            this.fontComboBox,
            this.fontSizeComboBox});
            this.styleTools.Location = new System.Drawing.Point(293, 79);
            this.styleTools.Name = "styleTools";
            this.styleTools.Size = new System.Drawing.Size(308, 27);
            this.styleTools.TabIndex = 3;
            this.styleTools.Visible = false;
            // 
            // styleComboBox
            // 
            this.styleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.styleComboBox.Items.AddRange(new object[] {
            "Style1",
            "Style2"});
            this.styleComboBox.Name = "styleComboBox";
            this.styleComboBox.Size = new System.Drawing.Size(121, 27);
            this.styleComboBox.SelectedIndexChanged += new System.EventHandler(this.styleComboBox_SelectedIndexChanged);
            // 
            // fontComboBox
            // 
            this.fontComboBox.Name = "fontComboBox";
            this.fontComboBox.Size = new System.Drawing.Size(121, 27);
            this.fontComboBox.SelectedIndexChanged += new System.EventHandler(this.fontComboBox_SelectedIndexChanged);
            // 
            // fontSizeComboBox
            // 
            this.fontSizeComboBox.AutoSize = false;
            this.fontSizeComboBox.DropDownWidth = 50;
            this.fontSizeComboBox.Items.AddRange(new object[] {
            "6",
            "8",
            "10",
            "12",
            "14",
            "16",
            "18",
            "20"});
            this.fontSizeComboBox.Name = "fontSizeComboBox";
            this.fontSizeComboBox.Size = new System.Drawing.Size(50, 27);
            // 
            // markerStrip
            // 
            this.markerStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.markerStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.markerCombo});
            this.markerStrip.Location = new System.Drawing.Point(3, 81);
            this.markerStrip.Name = "markerStrip";
            this.markerStrip.Size = new System.Drawing.Size(133, 27);
            this.markerStrip.TabIndex = 5;
            // 
            // markerCombo
            // 
            this.markerCombo.Name = "markerCombo";
            this.markerCombo.Size = new System.Drawing.Size(121, 27);
            this.markerCombo.SelectedIndexChanged += new System.EventHandler(this.markerCombo_SelectedIndexChanged);
            // 
            // layoutTools
            // 
            this.layoutTools.Dock = System.Windows.Forms.DockStyle.None;
            this.layoutTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.styleSheetCombo,
            this.shapeCombo});
            this.layoutTools.Location = new System.Drawing.Point(136, 81);
            this.layoutTools.Name = "layoutTools";
            this.layoutTools.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.layoutTools.Size = new System.Drawing.Size(213, 27);
            this.layoutTools.TabIndex = 4;
            // 
            // styleSheetCombo
            // 
            this.styleSheetCombo.AutoSize = false;
            this.styleSheetCombo.Name = "styleSheetCombo";
            this.styleSheetCombo.Size = new System.Drawing.Size(121, 27);
            this.styleSheetCombo.ToolTipText = "Stylesheet";
            this.styleSheetCombo.SelectedIndexChanged += new System.EventHandler(this.styleSheetCombo_SelectedIndexChanged);
            this.styleSheetCombo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.styleSheetCombo_KeyDown);
            // 
            // shapeCombo
            // 
            this.shapeCombo.AutoSize = false;
            this.shapeCombo.Margin = new System.Windows.Forms.Padding(0);
            this.shapeCombo.Name = "shapeCombo";
            this.shapeCombo.Size = new System.Drawing.Size(80, 27);
            this.shapeCombo.ToolTipText = "change shape of widget";
            this.shapeCombo.SelectedIndexChanged += new System.EventHandler(this.shapeCombo_SelectedIndexChanged);
            // 
            // exportPagesToolStripMenuItem
            // 
            this.exportPagesToolStripMenuItem.Name = "exportPagesToolStripMenuItem";
            this.exportPagesToolStripMenuItem.Size = new System.Drawing.Size(196, 24);
            this.exportPagesToolStripMenuItem.Text = "Export Pages";
            this.exportPagesToolStripMenuItem.Click += new System.EventHandler(this.exportPagesToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 406);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "limada::concept";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.sceneTools.ResumeLayout(false);
            this.sceneTools.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.viewToolStrip.ResumeLayout(false);
            this.viewToolStrip.PerformLayout();
            this.styleTools.ResumeLayout(false);
            this.styleTools.PerformLayout();
            this.markerStrip.ResumeLayout(false);
            this.markerStrip.PerformLayout();
            this.layoutTools.ResumeLayout(false);
            this.layoutTools.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel scrollPosLabel;
        private System.Windows.Forms.ToolStrip sceneTools;
        private System.Windows.Forms.ToolStripButton moveButton;
        private System.Windows.Forms.ToolStripButton selectButton;
        private System.Windows.Forms.ToolStripSplitButton zoomButton;
        private System.Windows.Forms.ToolStripMenuItem zoomMenuFitToScreen;
        private System.Windows.Forms.ToolStripMenuItem zoomMenuFittoWidth;
        private System.Windows.Forms.ToolStripMenuItem zoomMenuFittoHeigth;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomMenuOriginalSize;
        private System.Windows.Forms.ToolStripButton connectorButton;
        private System.Windows.Forms.ToolStripButton AddWidgetButton;
        private System.Windows.Forms.ToolStripButton LayoutButton;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem styleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showQuadTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem currentProblemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer;
        public Limaki.Winform.Displays.WidgetDisplay Display1;
        public Limaki.Winform.Displays.WidgetDisplay Display2;
        private System.Windows.Forms.ToolStrip styleTools;
        private System.Windows.Forms.ToolStripComboBox styleComboBox;
        private System.Windows.Forms.ToolStripComboBox fontComboBox;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ToolStripComboBox fontSizeComboBox;
        private System.Windows.Forms.ToolStrip layoutTools;
        private System.Windows.Forms.ToolStripComboBox styleSheetCombo;
        private ToolStripShapeComboBox shapeCombo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStrip markerStrip;
        private System.Windows.Forms.ToolStripComboBox markerCombo;
        private System.Windows.Forms.ToolStripMenuItem openTestCaseMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileVerticallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleViewsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tiledGraphDocumentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem schemaFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStrip viewToolStrip;
        private System.Windows.Forms.ToolStripButton graphDocViewButton;
        private System.Windows.Forms.ToolStripButton graphGraphViewButton;
        private System.Windows.Forms.ToolStripButton toggleViewButton;
        private System.Windows.Forms.ToolStripButton goHomeButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton newSheetButton;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton goBackButton;
        private System.Windows.Forms.ToolStripButton goForwardButton;
        private System.Windows.Forms.ToolStripMenuItem exportPagesToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}