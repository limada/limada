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
using Limada.Presenter;
using Limaki.Common;
using Limaki.Model.Streams;
using Limaki.UseCases.Viewers;
using Limaki.Presenter.Visuals;
using Limaki.Visuals;
using Id = System.Int64;
using Limaki.Drawing;
using Limada.Common;
using Limaki.Presenter.Display;

namespace Limaki.UseCases.Viewers.StreamViewers {
    public class SheetViewerController : StreamViewerController {

        protected IGraphSceneDisplay<IVisual, IVisualEdge> _sheetControl = null;
        public IGraphSceneDisplay<IVisual, IVisualEdge> SheetControl {
            get { return _sheetControl; }
            set {
                if (_sheetControl != value && value != null) {
                    this.CurrentThingId = value.DataId;
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

        public override void SetContent(StreamInfo<Stream> streamInfo) {
            if (SheetControl == null) {
                throw new ArgumentException("sheetControl must not be null");
            }

            SheetControl.Execute();

            if (SheetControl.DataId == 0) 
                SheetControl.DataId = Isaac.Long;
            
            SheetManager.StoreInStreams(SheetControl.Data, SheetControl.Layout, SheetControl.DataId);
            var current = SheetManager.RegisterSheet(SheetControl.DataId, SheetControl.Text);
            SheetControl.Data.State.CopyTo(current.State);

            var loadfromStreamManager = false;
            var isStreamOwner = this.IsStreamOwner;

            Id id = streamInfo.Source is Id ? (Id)streamInfo.Source : 0;
            SheetInfo stored = null;
            if (id != 0) {
                var sameSheet = id == SheetControl.DataId && !SheetControl.Data.State.Clean;
                stored = SheetManager.GetSheetInfo(id);
                if (sameSheet ||(stored != null && !stored.State.Clean)) {
                    var dialog = Registry.Factory.Create<IMessageBoxShow>();
                    if (dialog.Show(
                        "Warning",
                        string.Format("You try to load the same sheet again.\r\nYou'll lose all changes on sheet {0}",stored.Name), 
                        MessageBoxButtons.OKCancel)
                        == DialogResult.Cancel) {
                       loadfromStreamManager = true;
                    }
                }
            }

            SheetInfo sheetinfo = null;
            if (loadfromStreamManager) {
                SheetManager.LoadFromStreams(SheetControl.Data, SheetControl.Layout, stored.Id);
                sheetinfo = stored;
                isStreamOwner = false;
            } else {
                sheetinfo = SheetManager.LoadFromStreamInfo(streamInfo, SheetControl.Data, SheetControl.Layout);
            }
            SheetControl.DeviceRenderer.Render ();

            SheetControl.Execute();
            SheetControl.Text = sheetinfo.Name;
            SheetControl.DataId = sheetinfo.Id;
            sheetinfo.State.CopyTo(SheetControl.Data.State);
            this.CurrentThingId = SheetControl.DataId;
            Registry.ApplyProperties<MarkerContextProcessor, IGraphScene<IVisual, IVisualEdge>>(SheetControl.Data);

            if (isStreamOwner) {
                streamInfo.Data.Close();
                streamInfo.Data = null;
            }

        }

        public override void Save(StreamInfo<Stream> info) { }

        public override bool CanSave() {return false;}

        public override void Dispose() {
            SheetControl = null;
        }
    }
}