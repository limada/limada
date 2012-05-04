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
 * http://www.limada.org
 * 
 */


using System.Drawing;
using System.Windows.Forms;

namespace Limaki.View.Swf.DragDrop {
    public abstract class ImageDataObjectHandler<TContainer, TItem> : StreamDataObjectHandler<TContainer, TItem>
        where TItem : class {
        public abstract TItem GetData(Image image, TContainer container);

        public override TItem GetData(IDataObject data, TContainer container) {
            DataObject dataObject = data as DataObject;
            Image image = null;
            if (dataObject != null) {
                if (dataObject.ContainsImage()) {
                    image = dataObject.GetImage();
                }
            }
            if (image != null) {
                return GetData(image, container);
            } else {
                return null;
            }
        }
    }

    public abstract class TextDataObjectHandler<TContainer, TItem> : StreamDataObjectHandler<TContainer, TItem>
    where TItem : class {
        public abstract TItem GetData(string text, TContainer container);
        public virtual TItem GetData(IDataObject data, TContainer container, TextDataFormat format) {
            DataObject dataObject = data as DataObject;
            string htmlText = null;
            if (dataObject != null) {
                if (dataObject.ContainsText(format)) {
                    htmlText = dataObject.GetText(format);
                }
            }
            if (htmlText != null) {
                return GetData(htmlText, container);
            } else {
                return null;
            }
        }
    }

    public abstract class HTMLDataObjectHandler<TContainer, TItem> : TextDataObjectHandler<TContainer, TItem> 
    where TItem : class {
        public override TItem GetData(IDataObject data, TContainer container) {
            return base.GetData (data, container, TextDataFormat.Html);
        }
    }
}