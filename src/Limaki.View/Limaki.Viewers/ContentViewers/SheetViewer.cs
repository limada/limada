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
using Limaki.Drawing;
using Limaki.Model.Content;
using Limaki.View.Visualizers;
using Limaki.Visuals;

namespace Limaki.Viewers.StreamViewers {

    public class SheetViewer : ContentStreamViewer {

        protected IGraphSceneDisplay<IVisual, IVisualEdge> _sheetControl = null;
        public IGraphSceneDisplay<IVisual, IVisualEdge> SheetControl {
            get { return _sheetControl; }
            set {
                if (value != null)
                    this.ContentId = value.DataId;
                if (_sheetControl != value && value != null) {
                    _sheetControl = value;
                    OnAttach(_sheetControl);
                }
            }
        }

        public ISheetManager SheetManager { get;set;}

        public override object Backend {
            get { return SheetControl; }
        }

        public override bool Supports(long streamType) {
            return streamType == ContentTypes.LimadaSheet;
        }

        public override void SetContent(Content<Stream> content) {
            if (SheetControl == null) {
                throw new ArgumentException("sheetControl must not be null");
            }

            SheetControl.Execute();

            if (SheetControl.DataId == 0) 
                SheetControl.DataId = Isaac.Long;
            
            SheetManager.SaveInStore(SheetControl.Data, SheetControl.Layout, SheetControl.DataId);
            SheetManager.RegisterSheet(SheetControl.Info);

            var loadFromMemory = false;
            var isStreamOwner = this.IsStreamOwner;

            Int64 id = content.Source is Int64 ? (Int64)content.Source : 0;
            SceneInfo stored = null;
            if (id != 0 ) {
                stored = SheetManager.GetSheetInfo(id);
                if(stored != null && id == stored.Id && !stored.State.Clean) {
                    var dialog = Registry.Factory.Create<IMessageBoxShow>();
                    if (dialog.Show(
                        "Warning",
                        string.Format("You try to load the a changed sheet again.\r\nYou'll lose all changes on sheet {0}",stored.Name), 
                        MessageBoxButtons.OKCancel)
                        == DialogResult.Cancel) {
                       loadFromMemory = true;
                    }
                }
            }

            SceneInfo sheetinfo = null;
            if (!loadFromMemory) {
                sheetinfo = SheetManager.LoadFromContent(content, SheetControl.Data, SheetControl.Layout);
            } else {
                SheetManager.LoadFromStore(SheetControl.Data, SheetControl.Layout, stored.Id);
                sheetinfo = stored;
                isStreamOwner = false;
            }
            SheetControl.BackendRenderer.Render ();

            SheetControl.Execute();
            SheetControl.Info = sheetinfo;
           
            this.ContentId = SheetControl.DataId;
            Registry.ApplyProperties<MarkerContextProcessor, IGraphScene<IVisual, IVisualEdge>>(SheetControl.Data);

            if (isStreamOwner) {
                content.Data.Close();
                content.Data = null;
            }

        }

        public override void Save(Content<Stream> content) { }

        public override bool CanSave() {return false;}

        public override void Dispose() {
            SheetControl = null;
        }
    }
}