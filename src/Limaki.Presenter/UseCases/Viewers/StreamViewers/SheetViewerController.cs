/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
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
using Limaki.Presenter.Widgets;
using Limaki.Widgets;
using Id = System.Int64;

namespace Limaki.UseCases.Viewers.StreamViewers {
    public class SheetViewerController : StreamViewerController {

        protected WidgetDisplay _sheetControl = null;
        public WidgetDisplay sheetControl {
            get { return _sheetControl; }
            set {
                if (_sheetControl != value && value != null) {
                    this.CurrentThingId = value.SceneId;
                    _sheetControl = value;
                    OnAttach(_sheetControl);
                }
            }
        }
        public ISheetManager sheetManager = null;

        public override object Control {
            get { return sheetControl; }
        }

        public override bool CanView(long streamType) {
            return streamType == StreamTypes.LimadaSheet;
        }

        public override void SetContent(StreamInfo<Stream> info) {
            if (sheetControl == null) {
                throw new ArgumentException("sheetControl must not be null");
            }

            sheetControl.Execute();

            var sheetinfo = sheetManager
                .LoadSheet(sheetControl.Data, sheetControl.Layout, info);

            sheetControl.DeviceRenderer.Render ();

            sheetControl.Execute();
            sheetControl.Text = sheetinfo.Name;
            sheetControl.SceneId = (Id)info.Source;

            Registry.ApplyProperties<MarkerContextProcessor, Scene>(sheetControl.Data);

            if (IsStreamOwner) {
                info.Data.Close();
                info.Data = null;
            }

        }

        public override void Save(StreamInfo<Stream> info) { }

        public override bool CanSave() {return false;}



        public override void Dispose() {
            sheetControl = null;
            sheetManager = null;
        }
    }
}