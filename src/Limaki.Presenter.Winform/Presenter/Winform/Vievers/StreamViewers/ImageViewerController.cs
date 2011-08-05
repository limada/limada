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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Limada.Model;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Model.Streams;
using System.IO;
using Limaki.Presenter.Winform;
using Limaki.UseCases.Viewers;namespace Limaki.UseCases.Winform.Viewers.StreamViewers {
    public class ImageViewerController : StreamViewerController {
        WinformImageDisplay _control = null;
        public override object Control {
            get {
                if (_control == null) {
                    _control = new WinformImageDisplay();
                    OnAttach (_control);
                }
                return _control;
            }
        }

        private ICollection<Int64> _imageStreamTypes = null;
        public ICollection<Int64> ImageStreamTypes {
            get {
                if (_imageStreamTypes == null) {
                    _imageStreamTypes = new Set<Int64>();
                    _imageStreamTypes.Add(StreamTypes.TIF);
                    _imageStreamTypes.Add(StreamTypes.JPG);
                    _imageStreamTypes.Add(StreamTypes.GIF);
                    _imageStreamTypes.Add(StreamTypes.PNG);
                    _imageStreamTypes.Add(StreamTypes.BMP);
                    _imageStreamTypes.Add(StreamTypes.EMF);
                    _imageStreamTypes.Add(StreamTypes.WMF);
                }
                return _imageStreamTypes;
            }
            set { _imageStreamTypes = value; }
        }

        public override bool Supports(long streamType) {
            return ImageStreamTypes.Contains(streamType);
        }

        public override bool CanSave() {
            return false;
        }

        public override void SetContent(Content<Stream> info) {
            var control = Control as WinformImageDisplay;

            if (control != null) {
                var display = control.Display;
                display.BackColor = this.BackColor;

                display.Data = Image.FromStream(info.Data);

                display.ZoomState = ZoomState.FitToScreen;
            }
            if (IsStreamOwner) {
                info.Data.Close ();
                info.Data = null;
            }
        }

        public override void Save(Content<Stream> info) { }

        public override void Dispose() {
            if (_control != null) {
                _control.Dispose();
                _control = null;
            }
        }

        public override void Clear() {
            base.Clear();
            var control = _control as WinformImageDisplay;
            if (control != null) {
                control.Display.Data = null;
            }
        }
    }
}