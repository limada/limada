﻿/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 - 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Specialized;
using System.Linq;
using Xwt.GdiBackend;
using SD = System.Drawing;
using SWF = System.Windows.Forms;


namespace Xwt.SwfBackend {

    public static class DragDropConverter {

        public static DragDropAction ToXwt (this SWF.DragDropEffects value) {
            var action = DragDropAction.None;
            if ((value & SWF.DragDropEffects.Copy) > 0)
                action |= DragDropAction.Copy;
            if ((value & SWF.DragDropEffects.Move) > 0)
                action |= DragDropAction.Move;
            if ((value & SWF.DragDropEffects.Link) > 0)
                action |= DragDropAction.Link;
            return action;
        }

        public static SWF.DragDropEffects ToSwf (this DragDropAction value) {
            var effects = SWF.DragDropEffects.None;
            if ((value & DragDropAction.Copy) > 0)
                effects |= SWF.DragDropEffects.Copy;
            if ((value & DragDropAction.Move) > 0)
                effects |= SWF.DragDropEffects.Move;
            if ((value & DragDropAction.Link) > 0)
                effects |= SWF.DragDropEffects.Link;
            return effects;
        }

        public static string ToSwf (this TransferDataType type) {
            if (type == TransferDataType.Text)
                return SWF.DataFormats.UnicodeText;
            if (type == TransferDataType.Rtf)
                return SWF.DataFormats.Rtf;
            if (type == TransferDataType.Uri)
                return SWF.DataFormats.FileDrop;
            if (type == TransferDataType.Image)
                return SWF.DataFormats.Bitmap;
            return type.Id;
        }

        public static TransferDataType ToXwtTransferType (string type) {
            if (type == SWF.DataFormats.UnicodeText)
                return TransferDataType.Text;
            if (type == SWF.DataFormats.Rtf)
                return TransferDataType.Rtf;
            if (type == SWF.DataFormats.FileDrop)
                return TransferDataType.Uri;
            if (type == SWF.DataFormats.Bitmap)
                return TransferDataType.Image;
            return TransferDataType.FromId(type);
        }

        public static SWF.IDataObject ToSwf (this TransferDataSource data) {
            var result = new SWF.DataObject();
            foreach (var type in data.DataTypes) {
                var value = data.GetValue(type);

                if (type == TransferDataType.Text)
                    result.SetText((string) value);
                else if (type == TransferDataType.Uri) {
                    var uris = new StringCollection();
                    uris.Add(((Uri) value).LocalPath);
                    result.SetFileDropList(uris);
                } else
                    result.SetData(type.Id, value);
            }

            return result;
        }

        public static TransferDataSource ToXwt (this SWF.DataObject data) {
            var result = new TransferDataSource();
            result.DataRequestCallback = dt => data.GetData(dt.ToSwf());
                
            foreach (var item in data.GetFormats()) {
                var format = ToXwtTransferType (item);
                if (format == TransferDataType.Uri) {
                    result.DataRequestCallback = dt => {
                        var value = data.GetData (TransferDataType.Uri.ToSwf ());
                        var uris = ((string[])value).Select (f => new Uri (f)).ToArray ();
                        return uris;
                    };
                }
                result.AddType(format);
            }

            return result;
        }

      
     }
}

