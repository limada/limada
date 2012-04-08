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


using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Limaki.View.Swf.DragDrop {
    public abstract class SerializedDataObjectHandler<TContainer, TItem> : StreamDataObjectHandler<TContainer, TItem>
        where TItem : class {


        public override TItem GetData(Stream stream, TContainer container) {
            if (stream != null) {
                //stream.Position = 0;
                IFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as TItem;
            }
            return null;
        }


        public override Stream SetData(TContainer container, TItem value) {
            Stream stream = new MemoryStream ();
            IFormatter formatter = new BinaryFormatter();
            try {
                formatter.Serialize(stream, value);
            } catch (SerializationException) { }
            return stream;
        }


    }
}