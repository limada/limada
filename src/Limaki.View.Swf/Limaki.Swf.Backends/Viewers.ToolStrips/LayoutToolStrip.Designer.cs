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
    partial class LayoutToolStrip {
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
            this.StyleSheetCombo = new System.Windows.Forms.ToolStripComboBox();
            this.ShapeCombo = new ToolStripShapeComboBox();
            this.StyleDialogButton= new ToolStripButtonEx();
            // 
            // styleSheetCombo
            // 
            this.StyleSheetCombo.AutoSize = false;
            this.StyleSheetCombo.Name = "styleSheetCombo";
            this.StyleSheetCombo.Size = new System.Drawing.Size(121, 27);
            this.StyleSheetCombo.ToolTipText = "Stylesheet";
            this.StyleSheetCombo.SelectedIndexChanged += new System.EventHandler(this.StyleSheetSelectedIndexChanged);
            this.StyleSheetCombo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StyleSheetKeyDown);
            // 
            // shapeCombo
            // 
            this.ShapeCombo.AutoSize = false;
            this.ShapeCombo.Margin = new System.Windows.Forms.Padding(0);
            this.ShapeCombo.Name = "shapeCombo";
            this.ShapeCombo.Size = new System.Drawing.Size(80, 27);
            this.ShapeCombo.ToolTipText = "change shape of visual";
            this.ShapeCombo.SelectedIndexChanged += new System.EventHandler(this.ShapeSelectedIndexChanged);
            // 
            // StyleDialogButton
            //
            this.StyleDialogButton.Checked = false;
            this.StyleDialogButton.CheckState = System.Windows.Forms.CheckState.Unchecked;
            this.StyleDialogButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StyleDialogButton.Image = global::Limaki.View.Properties.Iconery.StyleItem;
            this.StyleDialogButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StyleDialogButton.Name = "StyleDialogButton";
            this.StyleDialogButton.Size = new System.Drawing.Size(24, 24);
            this.StyleDialogButton.ToolTipText = "set style for selected items";
            this.StyleDialogButton.Text = "SetStyle";
            this.StyleDialogButton.Click += new System.EventHandler(ChangeStyle);

            this.Dock = System.Windows.Forms.DockStyle.None;
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StyleSheetCombo,
            this.ShapeCombo,
            this.StyleDialogButton});
            this.Name = "layoutTools";
            this.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.Size = new System.Drawing.Size(213, 27);
            this.TabIndex = 4;
        }

        public System.Windows.Forms.ToolStripComboBox StyleSheetCombo;
        public ToolStripShapeComboBox ShapeCombo;
        public ToolStripButtonEx StyleDialogButton;

        #endregion
    }
}