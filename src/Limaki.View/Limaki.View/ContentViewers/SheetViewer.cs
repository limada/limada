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

using System;
using System.IO;
using Limaki.Common;
using Limaki.Contents;
using Limaki.View.GraphScene;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Mesh;
using Limaki.View.Viz.Visualizers;
using System.Linq;

namespace Limaki.View.ContentViewers {

    public class SheetViewer : ContentStreamViewer {

        protected IGraphSceneDisplay<IVisual, IVisualEdge> _sheetDisplay = null;
        public IGraphSceneDisplay<IVisual, IVisualEdge> SheetDisplay {
            get { return _sheetDisplay; }
            set {
                if (value != null)
                    this.ContentId = value.DataId;
                if (_sheetDisplay != value && value != null) {
                    _sheetDisplay = value;
                }
            }
        }

        public ISceneManager SceneManager { get; set; }

        public override IVidgetBackend Backend {
            get { return SheetDisplay.Backend; }
        }

        public override IVidget Frontend {
            get { return SheetDisplay; }
        }

        public override bool Supports(long streamType) {
            return streamType == ContentTypes.LimadaSheet;
        }

        public override void SetContent (Content<Stream> content) {

            if (SheetDisplay == null) {
                throw new ArgumentException ($"{GetType ().Name}.{nameof (this.SetContent)}:{nameof (SheetDisplay)} must not be null");
            }

            SceneManager.Store (SheetDisplay);
            var contentId = SceneManager.IdOf (content);
            if (contentId != 0) {
                if (SceneManager.IsSceneOpen (contentId, true, true))
                    return;
            }

            var loadContent = SceneManager.DoLoadDirtyScene(contentId, true);
            var isStreamOwner = this.IsStreamOwner;

            if (loadContent) {
                SceneManager.Load (SheetDisplay, content);
            } else {
                // should load from SceneManager.SheetStore:
                SceneManager.Load (SheetDisplay, contentId);
                isStreamOwner = false;
            }

            this.ContentId = SheetDisplay.DataId;

            if (isStreamOwner) {
                if (content.Data != null)
                    content.Data.Close ();
                content.Data = null;
            }

        }

        public override void Save(Content<Stream> content) { }

        public override bool CanSave() {return false;}

        public override void Dispose() {
            SheetDisplay = null;
        }
    }
}