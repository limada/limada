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
    partial class SplitViewToolStripBackend {
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

            this.sheetCombo = new System.Windows.Forms.ToolStripComboBox();

            // 
            // viewToolStrip
            // 
            this.Dock = System.Windows.Forms.DockStyle.None;
            this.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Name = "viewToolStrip";
            this.Size = new System.Drawing.Size(238, 27);
            this.TabIndex = 6;

            // 
            // markerCombo
            // 
            this.sheetCombo.Name = "markerCombo";
            this.sheetCombo.Size = new System.Drawing.Size(121, 27);
            
            this.Dock = System.Windows.Forms.DockStyle.None;

        }

       

        #endregion



        private System.Windows.Forms.ToolStripComboBox sheetCombo;
    }
}