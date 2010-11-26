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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Limada.Model;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Model.Streams;
using Limaki.Winform.Displays;
using System.IO;

namespace Limaki.App.StreamViewers {
    public class ImageViewerController : StreamViewerController {
        ImageDisplay _control = null;
        public override Control Control {
            get {
                if (_control == null) {
                    _control = new ImageDisplay();
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

        public override bool CanView(long streamType) {
            return ImageStreamTypes.Contains(streamType);
        }

        public override bool CanSave() {
            return false;
        }

        public override void SetContent(StreamInfo<Stream> info) {
            var control = Control as ImageDisplay;

            control.SelectAction.Enabled = false;
            control.ZoomAction.Enabled = true;
            control.ScrollAction.Enabled = true;
            control.BackColor = this.BackColor;

            control.Data = Image.FromStream(info.Data);

            control.ZoomState = ZoomState.FitToScreen;

            if (IsStreamOwner) {
                info.Data.Close ();
                info.Data = null;
            }
        }

        public override void Save(StreamInfo<Stream> info) { }

        public override void Dispose() {
            if (_control != null) {
                _control.Dispose();
                _control = null;
            }
        }
        public override void Clear() {
            base.Clear();
            if (_control != null) {
                var control = _control as ImageDisplay;
                control.Clear ();

            }
        }
    }
}