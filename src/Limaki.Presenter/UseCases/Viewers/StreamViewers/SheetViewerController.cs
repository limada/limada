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

using System;
using System.IO;
using Limada.Common;
using Limada.Presenter;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Model.Streams;
using Limaki.Presenter.Display;
using Limaki.Visuals;
using Id = System.Int64;

namespace Limaki.UseCases.Viewers.StreamViewers {
    public class SheetViewerController : StreamViewerController {

        protected IGraphSceneDisplay<IVisual, IVisualEdge> _sheetControl = null;
        public IGraphSceneDisplay<IVisual, IVisualEdge> SheetControl {
            get { return _sheetControl; }
            set {
                if (value != null)
                    this.CurrentThingId = value.DataId;
                if (_sheetControl != value && value != null) {
                    _sheetControl = value;
                    OnAttach(_sheetControl);
                }
            }
        }

        public ISheetManager SheetManager { get;set;}

        public override object Control {
            get { return SheetControl; }
        }

        public override bool Supports(long streamType) {
            return streamType == StreamTypes.LimadaSheet;
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

            Id id = content.Source is Id ? (Id)content.Source : 0;
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
            SheetControl.DeviceRenderer.Render ();

            SheetControl.Execute();
            SheetControl.Info = sheetinfo;
           
            this.CurrentThingId = SheetControl.DataId;
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