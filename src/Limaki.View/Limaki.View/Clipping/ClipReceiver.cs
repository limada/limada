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


using Limaki.Actions;
using Limaki.Common;
using Limaki.View.Rendering;

namespace Limaki.View.Clipping {
    /// <summary>
    /// this class updates the Viewport
    /// and renders according to Clipper
    /// has to be the last in the GraphicsPipeline
    /// </summary>
    public class ClipReceiver : ActionBase, IClipReceiver {
        
        public virtual IClipper Clipper { get; set; }
        
        public virtual IViewport Viewport {
            get {
                if (_viewport != null) {
                    return _viewport();
                }
                return null;
            }
        }

        public virtual IBackendRenderer DeviceRenderer {
            get {
                if (_renderer != null) {
                    return _renderer();
                }
                return null;
            }
        }

        public virtual void Execute() {
            if (!Clipper.IsEmpty) {
                Viewport.Update (Clipper);
                Render();
            }
        }

        public virtual void Invoke() {
            Viewport.Update();

            if (OS.Mono) {
                //Renderer.Render ();
            }
        }

        public virtual void Done() {
            Clipper.Clear();
        }

        public virtual void Render() {
            if (Clipper.RenderAll) {
                DeviceRenderer.Render();
            } else if (!Clipper.IsEmpty) {
                DeviceRenderer.Render(Clipper);
            }

        }


        #region IGraphicsPipeline Member

        Get<IViewport> _viewport = null;
        Get<IViewport> IClipReceiver.Viewport {
            get { return _viewport; }
            set { _viewport = value; }
        }

        Get<IBackendRenderer> _renderer = null;
        Get<IBackendRenderer> IClipReceiver.Renderer {
            get { return _renderer; }
            set { _renderer = value; }
        }

        #endregion
    }
}