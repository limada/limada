/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Xwt;

namespace Limaki.View.Ui.DragDrop0 {
    public abstract class StreamTransferDataHandler<TContainer, TItem> : ITransferDataHandler<TContainer, TItem>
        where TItem : class {

        public virtual Type HandledType { get { return typeof (TItem); } }

        public abstract string[] Formats { get; }

        public abstract TItem GetData(Stream stream, TContainer container);
        public abstract Stream SetData(TContainer container, TItem value);

        public virtual TItem Clone(TContainer container, TItem data) {
            var stream = SetData(container, data);
            return GetData(stream, container);
        }


        public virtual TItem GetData(TransferDataSource data, TContainer container) {
            return GetData(data.GetData(Formats[0]) as Stream, container);
        }

        public virtual bool Supports(TContainer container, TItem value) {
            return false;
        }

        public virtual void SetData(TransferDataSource data, TContainer container, TItem value) {
            if (Supports(container, value)) {
                var stream = SetData(container, value);
                data.SetData(Formats[0], stream);
            }
        }
        }

    public abstract class SerializedTransferDataHandler<TContainer, TItem> : StreamTransferDataHandler<TContainer, TItem>
        where TItem : class {


        public override TItem GetData(Stream stream, TContainer container) {
            if (stream != null) {
                //stream.Position = 0;
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as TItem;
            }
            return null;
        }


        public override Stream SetData(TContainer container, TItem value) {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            try {
                formatter.Serialize(stream, value);
            } catch (SerializationException) {
            }
            return stream;
        }


        }
}