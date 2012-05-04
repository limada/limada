/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

namespace Limaki.Swf.Backends.Viewers.ToolStrips {
    partial class DisplayToolStrip {
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

            this.selectButton = new System.Windows.Forms.ToolStripButton();
            this.LayoutButton = new System.Windows.Forms.ToolStripButton();
            this.zoomButton = new System.Windows.Forms.ToolStripSplitButton();
            this.zoomMenuFitToScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomMenuFittoWidth = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomMenuFittoHeigth = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomMenuOriginalSize = new System.Windows.Forms.ToolStripMenuItem();
            this.moveButton = new System.Windows.Forms.ToolStripButton();
            this.AddVisualButton = new System.Windows.Forms.ToolStripButton();
            this.connectorButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);            
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // selectButton
            // 
            this.selectButton.Checked = true;
            this.selectButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.selectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.selectButton.Image = global::Limaki.View.Properties.Resources.DrawSelection;
            this.selectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(24, 24);
            this.selectButton.Text = "Select";
            this.selectButton.Click += new System.EventHandler(this.SelectOrMove);
            // 
            // LayoutButton
            // 
            this.LayoutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.LayoutButton.Image = global::Limaki.View.Properties.Resources.ModifyLayout24;
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
            this.zoomButton.Image = global::Limaki.View.Properties.Resources.ZoomToolIcon;
            this.zoomButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomButton.Name = "zoomButton";
            this.zoomButton.Size = new System.Drawing.Size(36, 24);
            this.zoomButton.Text = "Zoom";
            this.zoomButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ZoomInOut);
            // 
            // zoomMenuFitToScreen
            // 
            this.zoomMenuFitToScreen.CheckOnClick = true;
            this.zoomMenuFitToScreen.Name = "zoomMenuFitToScreen";
            this.zoomMenuFitToScreen.Size = new System.Drawing.Size(167, 24);
            this.zoomMenuFitToScreen.Text = "Fit to Screen";
            this.zoomMenuFitToScreen.Click += new System.EventHandler(this.ZoomState);
            // 
            // zoomMenuFittoWidth
            // 
            this.zoomMenuFittoWidth.CheckOnClick = true;
            this.zoomMenuFittoWidth.Name = "zoomMenuFittoWidth";
            this.zoomMenuFittoWidth.Size = new System.Drawing.Size(167, 24);
            this.zoomMenuFittoWidth.Text = "Fit to Width";
            this.zoomMenuFittoWidth.Click += new System.EventHandler(this.ZoomState);
            // 
            // zoomMenuFittoHeigth
            // 
            this.zoomMenuFittoHeigth.CheckOnClick = true;
            this.zoomMenuFittoHeigth.Name = "zoomMenuFittoHeigth";
            this.zoomMenuFittoHeigth.Size = new System.Drawing.Size(167, 24);
            this.zoomMenuFittoHeigth.Text = "Fit to Heigth";
            this.zoomMenuFittoHeigth.Click += new System.EventHandler(this.ZoomState);
            // 
            // zoomMenuOriginalSize
            // 
            this.zoomMenuOriginalSize.Name = "zoomMenuOriginalSize";
            this.zoomMenuOriginalSize.Size = new System.Drawing.Size(167, 24);
            this.zoomMenuOriginalSize.Text = "Original Size";
            this.zoomMenuOriginalSize.Click += new System.EventHandler(this.ZoomState);
            // 
            // moveButton
            // 
            this.moveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveButton.Image = global::Limaki.View.Properties.Resources.MoveShift;
            this.moveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(24, 24);
            this.moveButton.Text = "Move";
            this.moveButton.ToolTipText = "panning";
            this.moveButton.Click += new System.EventHandler(this.SelectOrMove);
            // 
            // AddVisualButton
            // 
            this.AddVisualButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddVisualButton.Image = global::Limaki.View.Properties.Resources.CreateWidget24;
            this.AddVisualButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddVisualButton.Name = "AddVisualButton";
            this.AddVisualButton.Size = new System.Drawing.Size(24, 24);
            this.AddVisualButton.Text = "Add Shape";
            this.AddVisualButton.ToolTipText = "Add Visual";
            this.AddVisualButton.Click += new System.EventHandler(this.SelectOrMove);
            // 
            // connectorButton
            // 
            this.connectorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.connectorButton.Image = global::Limaki.View.Properties.Resources.StraightConnector;
            this.connectorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectorButton.Name = "connectorButton";
            this.connectorButton.Size = new System.Drawing.Size(24, 24);
            this.connectorButton.Text = "Add Link";
            this.connectorButton.ToolTipText = "connect";
            this.connectorButton.Click += new System.EventHandler(this.SelectOrMove);

            this.Dock = System.Windows.Forms.DockStyle.None;
            this.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.selectButton,
                //this.toolStripSeparator2,
                //this.LayoutButton,
                this.zoomButton,
                this.moveButton,
                //this.toolStripSeparator1,
                //this.AddVisualButton,
                //this.connectorButton
            });
            this.Name = "sceneTools";
            this.Size = new System.Drawing.Size(178, 27);
            this.Text = "Scene Tools";
        }

        #endregion

        private System.Windows.Forms.ToolStripButton selectButton;
        private System.Windows.Forms.ToolStripButton LayoutButton;
        private System.Windows.Forms.ToolStripSplitButton zoomButton;
        private System.Windows.Forms.ToolStripButton moveButton;
        private System.Windows.Forms.ToolStripButton AddVisualButton;
        private System.Windows.Forms.ToolStripButton connectorButton;

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        
        private System.Windows.Forms.ToolStripMenuItem zoomMenuFitToScreen;
        private System.Windows.Forms.ToolStripMenuItem zoomMenuFittoWidth;
        private System.Windows.Forms.ToolStripMenuItem zoomMenuFittoHeigth;
        private System.Windows.Forms.ToolStripMenuItem zoomMenuOriginalSize;
    }
}