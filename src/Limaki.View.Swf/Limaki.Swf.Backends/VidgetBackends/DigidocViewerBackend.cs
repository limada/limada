/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 */


using System.Windows.Forms;
using Limaki.Drawing;
using Limada.View.Vidgets;
using Limaki.View.Vidgets;
using Limaki.View.Swf.Visualizers;
using System.Drawing;
using Xwt.Gdi.Backend;

namespace Limaki.View.Swf.Backends {

    public partial class DigidocViewerBackend : UserControl, IDigidocViewerBackend, IZoomTarget, IDragDropControl {

        public DigidocViewerBackend () {}

        public DigidocVidget Frontend { get; set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (DigidocVidget)frontend;
            Compose();
        }

        private void Compose () {

            var panel = new Panel { Dock = System.Windows.Forms.DockStyle.Fill, BackColor = Color.White };
            this.SuspendLayout();

            var pagesDisplayBackend = Frontend.PagesDisplay.Backend as Control;
            pagesDisplayBackend.Dock = System.Windows.Forms.DockStyle.Right;
            pagesDisplayBackend.Width = Frontend.GetDefaultWidth();
            pagesDisplayBackend.TabStop = false;
            
            Frontend.Compose();

            var splitter = new System.Windows.Forms.Splitter { Dock = DockStyle.Right };
            this.Controls.AddRange(new Control[] { panel, splitter, pagesDisplayBackend });

            this.ResumeLayout(false);
            this.PerformLayout();

            this.PerformLayout();
            Application.DoEvents();

            Frontend.AttachContentViewer = contentViewer => {
                var contentControl = (contentViewer.Backend as System.Windows.Forms.Control);
                if (contentControl.Dock != DockStyle.Fill)
                    contentControl.Dock = DockStyle.Fill;
                
                if (!panel.Controls.Contains(contentControl)) {
                    panel.Controls.Clear();
                    panel.Controls.Add(contentControl);
                    Application.DoEvents();
                }
            };

        }

        void SplitContainer_KeyDown(object sender, KeyEventArgs e) {
            e.Handled = false;
        }

        #region IZoomTarget Member

        public ZoomState ZoomState {
            get { return Frontend.ZoomState; }
            set { Frontend.ZoomState = value; }
        }

        public double ZoomFactor {
            get { return Frontend.ZoomFactor; }
            set { Frontend.ZoomFactor = value; }
        }

        public void UpdateZoom () {
            Frontend.UpdateZoom();
        }

        #endregion

        #region IVidgetBackend-Implementation

        Xwt.Size IVidgetBackend.Size {
            get { return this.Size.ToXwt(); }
        }

        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate(rect.ToGdi());
        }

        Xwt.Point IDragDropControl.PointToClient (Xwt.Point source) { return PointToClient(source.ToGdi()).ToXwt(); }

        #endregion
    }
}