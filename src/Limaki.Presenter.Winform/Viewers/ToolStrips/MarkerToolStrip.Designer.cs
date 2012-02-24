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

namespace Limaki.UseCases.Winform.Viewers.ToolStripViewers {
    partial class MarkerToolStrip {
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
            this.markerCombo = new System.Windows.Forms.ToolStripComboBox();
            // 
            // markerCombo
            // 
            this.markerCombo.Name = "markerCombo";
            this.markerCombo.Size = new System.Drawing.Size(121, 27);
            this.markerCombo.Width = 120;
            this.markerCombo.SelectedIndexChanged += new System.EventHandler(this.markerCombo_SelectedIndexChanged);
            this.Dock = System.Windows.Forms.DockStyle.None;
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.markerCombo});

            this.Name = "markerStrip";
            this.Size = new System.Drawing.Size(133, 27);
            this.TabIndex = 5;
        }

        #endregion

        private System.Windows.Forms.ToolStripComboBox markerCombo;
    }
}