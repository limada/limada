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

        public ISheetManager SheetManager { get;set;}

        public override IVidgetBackend Backend {
            get { return SheetDisplay.Backend; }
        }

        public override IVidget Frontend {
            get { return SheetDisplay; }
        }

        public override bool Supports(long streamType) {
            return streamType == ContentTypes.LimadaSheet;
        }

        IGraphSceneMesh<IVisual, IVisualEdge> _mesh = null;
        IGraphSceneMesh<IVisual, IVisualEdge> Mesh { get { return _mesh ?? (_mesh = Registry.Pooled<IGraphSceneMesh<IVisual, IVisualEdge>> ()); } }

        public override void SetContent(Content<Stream> content) {
            if (SheetDisplay == null) {
                throw new ArgumentException("sheetControl must not be null");
            }

            SheetDisplay.Perform();

            if (SheetDisplay.DataId == 0) 
                SheetDisplay.DataId = Isaac.Long;
            
            SheetManager.SaveInStore(SheetDisplay.Data, SheetDisplay.Layout, SheetDisplay.DataId);
            SheetManager.RegisterSheet(SheetDisplay.Info);

            Mesh.RemoveScene (SheetDisplay.Data);

            var loadFromMemory = false;
            var isStreamOwner = this.IsStreamOwner;

            var id = content.Source is Int64 ? (Int64)content.Source : 0;
            SceneInfo stored = null;
            if (id != 0 ) {
                stored = SheetManager.GetSheetInfo(id);
                if(stored != null && id == stored.Id && !stored.State.Clean) {
                    var dialog = Registry.Factory.Create<IMessageBoxShow>();
                    if (dialog.Show(
                        "Warning",
                        string.Format("You try to load the a changed sheet again.\r\nYou'll lose all changes on sheet {0}",stored.Name), 
                        MessageBoxButtons.OkCancel)
                        == DialogResult.Cancel) {
                       loadFromMemory = true;
                    }
                }
            }

            SceneInfo sheetinfo = null;
            if (!loadFromMemory) {
                sheetinfo = SheetManager.LoadFromContent(content, SheetDisplay.Data, SheetDisplay.Layout);
            } else {
                SheetManager.LoadFromStore(SheetDisplay.Data, SheetDisplay.Layout, stored.Id);
                sheetinfo = stored;
                isStreamOwner = false;
            }
            SheetDisplay.BackendRenderer.Render ();

            SheetDisplay.Perform();
            SheetDisplay.Info = sheetinfo;

            Mesh.AddScene (SheetDisplay.Data);

            this.ContentId = SheetDisplay.DataId;
            SheetDisplay.Data.CreateMarkers();

            if (isStreamOwner) {
                content.Data.Close();
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