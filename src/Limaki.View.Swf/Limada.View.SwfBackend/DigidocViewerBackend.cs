/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2014 Lytico
 *
 * http://www.limada.org
 */


using System.Windows.Forms;
using Limaki.Drawing;
using Limada.View.Vidgets;
using Limaki.View;
using Limaki.View.Vidgets;
using System.Drawing;
using Limaki.View.Common;
using Limaki.View.SwfBackend.VidgetBackends;

namespace Limada.View.SwfBackend {

    public partial class DigidocViewerBackend : VidgetBackend<UserControl>, IDigidocViewerBackend, IZoomTarget {

        public new DigidocVidget Frontend { get; set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (DigidocVidget)frontend;
            Compose2 ();
        }

        protected virtual void Compose2 () {

            Frontend.Compose ();
            
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            Control.SuspendLayout();
            
            var pagesDisplayBackend = Frontend.PagesDisplay.Backend as Control;
            pagesDisplayBackend.Dock = DockStyle.Right;
            pagesDisplayBackend.Width = (int)Frontend.GetDefaultWidth ();
            pagesDisplayBackend.TabStop = false;
            

            var splitter = new Splitter { Dock = DockStyle.Right };
            Control.Controls.AddRange(new Control[] { panel, splitter, pagesDisplayBackend });

            Control.ResumeLayout(false);
            Control.PerformLayout();

            Control.PerformLayout();
            Application.DoEvents();

            Frontend.AttachContentViewer = contentViewer => {
                var contentControl = (contentViewer.Backend as Control);
                if (contentControl.Dock != DockStyle.Fill)
                    contentControl.Dock = DockStyle.Fill;
                
                if (!panel.Controls.Contains(contentControl)) {
                    panel.Controls.Clear();
                    panel.Controls.Add(contentControl);
                    Application.DoEvents();
                }
            };

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

    }
}