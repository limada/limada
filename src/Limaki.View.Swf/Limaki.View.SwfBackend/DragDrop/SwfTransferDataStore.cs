/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;
using Xwt.SwfBackend;

namespace Limaki.View.SwfBackend.DragDrop {

    public class SwfTransferDataStore : ITransferData {
        public SwfTransferDataStore (System.Windows.Forms.IDataObject data) {
            this.Data = data;
        }

        public bool HasType (TransferDataType typeId) {
            return Data.GetFormats().Any(t => DragDropConverter.ToXwtTransferType(t).Id.ToLower() == typeId.Id.ToLower());
        }

        protected System.Windows.Forms.IDataObject Data { get; set; }

        public string Text {
            get { return ((System.Windows.Forms.DataObject) Data).GetText(); }
        }

        public Uri[] Uris {
            get { return ((System.Windows.Forms.DataObject) Data).GetFileDropList().OfType<string>().Select(s => new Uri(s)).ToArray(); }
        }

        public Xwt.Drawing.Image Image {
            get { return new Xwt.Drawing.Image(((System.Windows.Forms.DataObject) Data).GetImage()); }
        }

        public object GetValue (TransferDataType type) {
            var swftype = type.ToSwf ();
            var value = Data.GetData (swftype);
            if (swftype == "HTML Format") {
                // hack to correct decoding error in Data.GetData
                var bytes = Encoding.GetEncoding ("Windows-1252").GetBytes ((string)value);
                value = Encoding.UTF8.GetString (bytes);
            }
            return value;
        }

        public T GetValue<T> () where T : class {
            var t = typeof (T);
            if (Data.GetDataPresent(t))
                return Data.GetData(t) as T;
            return null;
        }

        public IEnumerable<TransferDataType> DataTypes {
            get { return Data.GetFormats().Select(f => DragDropConverter.ToXwtTransferType(f)); }
        }
    }
}