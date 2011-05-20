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
 * http://limada.sourceforge.net
 * 
 */

namespace Limaki.UseCases.Winform.Viewers.ToolStrips {
    partial class SplitViewToolStrip {
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
            
            this.graphStreamViewButton = new System.Windows.Forms.ToolStripButton();
            this.graphGraphViewButton = new System.Windows.Forms.ToolStripButton();
            this.toggleViewButton = new System.Windows.Forms.ToolStripButton();
            
            this.goBackButton = new System.Windows.Forms.ToolStripButton();
            this.goForwardButton = new System.Windows.Forms.ToolStripButton();
            this.goHomeButton = new System.Windows.Forms.ToolStripButton();
            
            this.newSheetButton = new System.Windows.Forms.ToolStripButton();
            this.newNoteButton = new System.Windows.Forms.ToolStripButton();
            this.saveSheetButton = new System.Windows.Forms.ToolStripButton();

            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();

            // 
            // viewToolStrip
            // 
            this.Dock = System.Windows.Forms.DockStyle.None;
            this.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                                                             this.graphStreamViewButton,
                                                                             this.graphGraphViewButton,
                                                                             this.toggleViewButton,
                                                                             this.toolStripSeparator3,
                                                                             this.goBackButton,
                                                                             this.goForwardButton,
                                                                             this.goHomeButton,
                                                                             this.toolStripSeparator4,
                                                                             this.newSheetButton,
                                                                             this.newNoteButton,
                                                                             this.saveSheetButton});

            this.Name = "viewToolStrip";
            this.Size = new System.Drawing.Size(238, 27);
            this.TabIndex = 6;
            // 
            // graphStreamViewButton
            // 
            this.graphStreamViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.graphStreamViewButton.Image = global::Limaki.Presenter.Properties.Resources.GraphDocView;
            this.graphStreamViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.graphStreamViewButton.Name = "graphStreamViewButton";
            this.graphStreamViewButton.Size = new System.Drawing.Size(24, 24);
            this.graphStreamViewButton.Text = "show stream contents";
            this.graphStreamViewButton.Click += new System.EventHandler(this.GraphStreamView);
            // 
            // graphGraphViewButton
            // 
            this.graphGraphViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.graphGraphViewButton.Image = global::Limaki.Presenter.Properties.Resources.GraphGraphView;
            this.graphGraphViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.graphGraphViewButton.Name = "graphGraphViewButton";
            this.graphGraphViewButton.Size = new System.Drawing.Size(24, 24);
            this.graphGraphViewButton.Text = "show tiled graph";
            this.graphGraphViewButton.Click += new System.EventHandler(this.GraphGraphView);
            // 
            // toggleViewButton
            // 
            this.toggleViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toggleViewButton.Image = global::Limaki.Presenter.Properties.Resources.ToggleView;
            this.toggleViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toggleViewButton.Name = "toggleViewButton";
            this.toggleViewButton.Size = new System.Drawing.Size(24, 24);
            this.toggleViewButton.Text = "toggle view";
            this.toggleViewButton.Click += new System.EventHandler(this.ToggleView);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // goBackButton
            // 
            this.goBackButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.goBackButton.Image = global::Limaki.Presenter.Properties.Resources.go_previous;
            this.goBackButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.goBackButton.Name = "goBackButton";
            this.goBackButton.Size = new System.Drawing.Size(24, 24);
            this.goBackButton.Text = "toolStripButton2";
            this.goBackButton.ToolTipText = "navigate back";
            this.goBackButton.Click += new System.EventHandler(this.GoBack);
            // 
            // goForwardButton
            // 
            this.goForwardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.goForwardButton.Image = global::Limaki.Presenter.Properties.Resources.go_next;
            this.goForwardButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.goForwardButton.Name = "goForwardButton";
            this.goForwardButton.Size = new System.Drawing.Size(24, 24);
            this.goForwardButton.Text = "toolStripButton2";
            this.goForwardButton.ToolTipText = "navigate forward";
            this.goForwardButton.Click += new System.EventHandler(this.GoForward);
            // 
            // goHomeButton
            // 
            this.goHomeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.goHomeButton.Image = global::Limaki.Presenter.Properties.Resources.gohome;
            this.goHomeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.goHomeButton.Name = "goHomeButton";
            this.goHomeButton.Size = new System.Drawing.Size(24, 24);
            this.goHomeButton.Text = "toolStripButton1";
            this.goHomeButton.ToolTipText = "go to favorites";
            this.goHomeButton.Click += new System.EventHandler(this.GoHome);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
            // 
            // newSheetButton
            // 
            this.newSheetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newSheetButton.Image = global::Limaki.Presenter.Properties.Resources.document_new;
            this.newSheetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newSheetButton.Name = "newSheetButton";
            this.newSheetButton.Size = new System.Drawing.Size(24, 24);
            this.newSheetButton.Text = "toolStripButton1";
            this.newSheetButton.ToolTipText = "new sheet";
            this.newSheetButton.Click += new System.EventHandler(this.NewSheet);
            // 
            // newNoteButton
            // 
            this.newNoteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newNoteButton.Image = global::Limaki.Presenter.Properties.Resources.notes;
            this.newNoteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newNoteButton.Name = "newNoteButton";
            this.newNoteButton.Size = new System.Drawing.Size(24, 24);
            this.newNoteButton.Text = "toolStripButton2";
            this.newNoteButton.ToolTipText = "new Note";
            this.newNoteButton.Click += new System.EventHandler(this.NewNote);
            // 
            // saveSheetButton
            // 
            this.saveSheetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveSheetButton.Image = global::Limaki.Presenter.Properties.Resources.stream_save;
            this.saveSheetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveSheetButton.Name = "saveSheetButton";
            this.saveSheetButton.Size = new System.Drawing.Size(24, 24);
            this.saveSheetButton.Text = "saveSheetButton";
            this.saveSheetButton.Click += new System.EventHandler(this.SaveDocument);

        }

        #endregion


        private System.Windows.Forms.ToolStripButton graphStreamViewButton;
        private System.Windows.Forms.ToolStripButton graphGraphViewButton;
        private System.Windows.Forms.ToolStripButton toggleViewButton;
        private System.Windows.Forms.ToolStripButton goHomeButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton newSheetButton;
        private System.Windows.Forms.ToolStripButton saveSheetButton;
        private System.Windows.Forms.ToolStripButton goBackButton;
        private System.Windows.Forms.ToolStripButton goForwardButton;
        private System.Windows.Forms.ToolStripButton newNoteButton;
    }
}