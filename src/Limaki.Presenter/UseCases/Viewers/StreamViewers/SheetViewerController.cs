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

namespace Limaki.UseCases.Viewers.StreamViewers {
    public class SheetViewerController : StreamViewerController {

        protected VisualsDisplay _sheetControl = null;
        public VisualsDisplay SheetControl {
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

            var sheetinfo = SheetManager.LoadFromStreamInfo(streamInfo, SheetControl.Data, SheetControl.Layout);

            SheetControl.DeviceRenderer.Render ();

            SheetControl.Execute();
            SheetControl.Text = sheetinfo.Name;
            SheetControl.DataId = sheetinfo.Id;
            sheetinfo.State.CopyTo(SheetControl.Data.State);

            Registry.ApplyProperties<MarkerContextProcessor, IGraphScene<IVisual, IVisualEdge>>(SheetControl.Data);

            if (IsStreamOwner) {
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