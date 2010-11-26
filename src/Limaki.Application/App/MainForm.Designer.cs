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
            SplitView.Dispose ();
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
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.styleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphGraphViewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleViewsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphStreamViewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.favoritesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToFavoritesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnOpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTestCaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectorTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showQuadTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentProblemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schemaFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportPagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wCFTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.scrollPosLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.markerStrip = new System.Windows.Forms.ToolStrip();
            this.markerCombo = new System.Windows.Forms.ToolStripComboBox();
            this.layoutTools = new System.Windows.Forms.ToolStrip();
            this.styleSheetCombo = new System.Windows.Forms.ToolStripComboBox();
            this.shapeCombo = new Limaki.Winform.Controls.ToolStripShapeComboBox();
            this.styleTools = new System.Windows.Forms.ToolStrip();
            this.styleComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.fontComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.fontSizeComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.markerStrip.SuspendLayout();
            this.layoutTools.SuspendLayout();
            this.styleTools.SuspendLayout();
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
            this.viewMenuItem,
            this.favoritesMenuItem,
            //this.testToolStripMenuItem,
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
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.printToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.exitToolStripMenuItem
            });
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(45, 23);
            this.FileMenu.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveFile_Click);
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.printPreviewToolStripMenuItem.Text = "Print preview...";
            this.printPreviewToolStripMenuItem.Click += new System.EventHandler(this.printPreviewToolStripMenuItem_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.printToolStripMenuItem.Text = "Print...";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageToolStripMenuItem,
            //this.xMLToolStripMenuItem
            });
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(143, 24);
            this.imageToolStripMenuItem.Text = " as Image ...";
            this.imageToolStripMenuItem.Click += new System.EventHandler(this.imageToolStripMenuItem_Click);
            // 
            // xMLToolStripMenuItem
            // 
            this.xMLToolStripMenuItem.Name = "xMLToolStripMenuItem";
            this.xMLToolStripMenuItem.Size = new System.Drawing.Size(143, 24);
            this.xMLToolStripMenuItem.Text = " Current View ...";
            this.xMLToolStripMenuItem.Click += new System.EventHandler(this.ExportAsMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
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
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.Copy_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.Paste_Click);
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
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
            this.layoutToolStripMenuItem.Click += new System.EventHandler(this.layoutOptionsMenuItem_Click);
            // 
            // styleToolStripMenuItem
            // 
            this.styleToolStripMenuItem.Name = "styleToolStripMenuItem";
            this.styleToolStripMenuItem.Size = new System.Drawing.Size(140, 24);
            this.styleToolStripMenuItem.Text = "Style...";
            this.styleToolStripMenuItem.Click += new System.EventHandler(this.showStyleEditor_Click);
            // 
            // viewMenuItem
            // 
            this.viewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.graphGraphViewMenuItem,
            this.toggleViewsMenuItem,
            this.graphStreamViewMenuItem});
            this.viewMenuItem.Name = "viewMenuItem";
            this.viewMenuItem.Size = new System.Drawing.Size(55, 23);
            this.viewMenuItem.Text = "View";
            // 
            // graphGraphViewMenuItem
            // 
            this.graphGraphViewMenuItem.Name = "graphGraphViewMenuItem";
            this.graphGraphViewMenuItem.Size = new System.Drawing.Size(209, 24);
            this.graphGraphViewMenuItem.Text = "Tiled Graph";
            // 
            // toggleViewsMenuItem
            // 
            this.toggleViewsMenuItem.Name = "toggleViewsMenuItem";
            this.toggleViewsMenuItem.Size = new System.Drawing.Size(209, 24);
            this.toggleViewsMenuItem.Text = "Toggle";
            // 
            // graphStreamViewMenuItem
            // 
            this.graphStreamViewMenuItem.Name = "graphStreamViewMenuItem";
            this.graphStreamViewMenuItem.Size = new System.Drawing.Size(209, 24);
            this.graphStreamViewMenuItem.Text = "Graph - Document";
            // 
            // favoritesMenuItem
            // 
            this.favoritesMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToFavoritesMenuItem,
            this.viewOnOpenMenuItem});
            this.favoritesMenuItem.Name = "favoritesMenuItem";
            this.favoritesMenuItem.Size = new System.Drawing.Size(84, 23);
            this.favoritesMenuItem.Text = "Favorites";
            // 
            // addToFavoritesMenuItem
            // 
            this.addToFavoritesMenuItem.Name = "addToFavoritesMenuItem";
            this.addToFavoritesMenuItem.Size = new System.Drawing.Size(194, 24);
            this.addToFavoritesMenuItem.Text = "Add to Favorites";
            this.addToFavoritesMenuItem.Click += new System.EventHandler(this.addToFavoritesMenuItem_Click);
            // 
            // viewOnOpenMenuItem
            // 
            this.viewOnOpenMenuItem.Name = "viewOnOpenMenuItem";
            this.viewOnOpenMenuItem.Size = new System.Drawing.Size(194, 24);
            this.viewOnOpenMenuItem.Text = "View on open";
            this.viewOnOpenMenuItem.Click += new System.EventHandler(this.viewOnOpenMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openTestCaseMenuItem,
            this.selectorTestToolStripMenuItem,
            this.otherTestToolStripMenuItem,
            this.showQuadTreeToolStripMenuItem,
            this.currentProblemToolStripMenuItem,
            this.schemaFilterToolStripMenuItem,
            this.exportPagesToolStripMenuItem,
            this.wCFTestToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(51, 23);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // openTestCaseMenuItem
            // 
            this.openTestCaseMenuItem.Image = global::Limaki.Properties.Resources.MenuFileOpenIcon;
            this.openTestCaseMenuItem.Name = "openTestCaseMenuItem";
            this.openTestCaseMenuItem.Size = new System.Drawing.Size(196, 24);
            this.openTestCaseMenuItem.Text = "Open Testcase...";
            this.openTestCaseMenuItem.Click += new System.EventHandler(this.ExampleOpen_Click);
            // 
            // selectorTestToolStripMenuItem
            // 
            this.selectorTestToolStripMenuItem.Name = "selectorTestToolStripMenuItem";
            this.selectorTestToolStripMenuItem.Size = new System.Drawing.Size(196, 24);
            this.selectorTestToolStripMenuItem.Text = "SelectorTest";
            this.selectorTestToolStripMenuItem.Click += new System.EventHandler(this.SelectorTest_Click);
            // 
            // otherTestToolStripMenuItem
            // 
            this.otherTestToolStripMenuItem.Name = "otherTestToolStripMenuItem";
            this.otherTestToolStripMenuItem.Size = new System.Drawing.Size(196, 24);
            this.otherTestToolStripMenuItem.Text = "BenchmarkOne";
            this.otherTestToolStripMenuItem.Click += new System.EventHandler(this.BenchmarkOneTest_Click);
            // 
            // showQuadTreeToolStripMenuItem
            // 
            this.showQuadTreeToolStripMenuItem.Name = "showQuadTreeToolStripMenuItem";
            this.showQuadTreeToolStripMenuItem.Size = new System.Drawing.Size(196, 24);
            this.showQuadTreeToolStripMenuItem.Text = "ShowQuadTree";
            this.showQuadTreeToolStripMenuItem.Click += new System.EventHandler(this.ShowQuadTree_Click);
            // 
            // currentProblemToolStripMenuItem
            // 
            this.currentProblemToolStripMenuItem.Name = "currentProblemToolStripMenuItem";
            this.currentProblemToolStripMenuItem.Size = new System.Drawing.Size(196, 24);
            this.currentProblemToolStripMenuItem.Text = "CurrentProblem";
            this.currentProblemToolStripMenuItem.Click += new System.EventHandler(this.currentProblemToolStripMenuItem_Click);
            // 
            // schemaFilterToolStripMenuItem
            // 
            this.schemaFilterToolStripMenuItem.Checked = true;
            this.schemaFilterToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.schemaFilterToolStripMenuItem.Name = "schemaFilterToolStripMenuItem";
            this.schemaFilterToolStripMenuItem.Size = new System.Drawing.Size(196, 24);
            this.schemaFilterToolStripMenuItem.Text = "SchemaFilter";
            this.schemaFilterToolStripMenuItem.Click += new System.EventHandler(this.schemaFilterMenuItem_Click);
            // 
            // exportPagesToolStripMenuItem
            // 
            this.exportPagesToolStripMenuItem.Name = "exportPagesToolStripMenuItem";
            this.exportPagesToolStripMenuItem.Size = new System.Drawing.Size(196, 24);
            this.exportPagesToolStripMenuItem.Text = "Export Pages";
            this.exportPagesToolStripMenuItem.Click += new System.EventHandler(this.exportPagesToolStripMenuItem_Click);
            // 
            // wCFTestToolStripMenuItem
            // 
            this.wCFTestToolStripMenuItem.Name = "wCFTestToolStripMenuItem";
            this.wCFTestToolStripMenuItem.Size = new System.Drawing.Size(196, 24);
            this.wCFTestToolStripMenuItem.Text = "WCFTest";
            this.wCFTestToolStripMenuItem.Click += new System.EventHandler(this.wCFTestToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(64, 23);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 6);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 6);
            // 
            // openFileDialog
            // 
            this.openFileDialog.CheckFileExists = false;
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
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(632, 330);
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
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.layoutTools);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.markerStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.styleTools);
            // 
            // markerStrip
            // 
            this.markerStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.markerStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.markerCombo});
            this.markerStrip.Location = new System.Drawing.Point(3, 27);
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
            this.layoutTools.Location = new System.Drawing.Point(136, 27);
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
            // styleTools
            // 
            this.styleTools.Dock = System.Windows.Forms.DockStyle.None;
            this.styleTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.styleComboBox,
            this.fontComboBox,
            this.fontSizeComboBox});
            this.styleTools.Location = new System.Drawing.Point(291, 108);
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
            // printDialog
            // 
            this.printDialog.UseEXDialog = true;
            // 
            // printPreviewDialog
            // 
            this.printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog.Enabled = true;
            this.printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog.Icon")));
            this.printPreviewDialog.Name = "printPreviewDialog";
            this.printPreviewDialog.Visible = false;
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsFile_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Visible = false;
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
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.markerStrip.ResumeLayout(false);
            this.markerStrip.PerformLayout();
            this.layoutTools.ResumeLayout(false);
            this.layoutTools.PerformLayout();
            this.styleTools.ResumeLayout(false);
            this.styleTools.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel scrollPosLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectorTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otherTestToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripMenuItem viewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphGraphViewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleViewsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphStreamViewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem schemaFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportPagesToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem wCFTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem favoritesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToFavoritesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnOpenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.PrintDialog printDialog;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    }
}