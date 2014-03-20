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


using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Limaki.Contents;
using Limaki.Graphs;
using Limaki.Drawing.Imaging;
using Limada.View.VisualThings;
using Limaki.View.Visuals;


namespace Limaki.View.Swf.DragDrop {
    public class VisualsImageDataObjectHandler: 
        IDataObjectHandler<IGraph<IVisual,IVisualEdge>,IVisual>, IVisualsDataObjectHandler{

        public virtual string[] DataFormats {
            get { return new string[] { System.Windows.Forms.DataFormats.Dib }; }
        }

        public Type HandledType {
            get { return typeof (IVisual); }
        }

        public void SetData(IDataObject data, IGraph<IVisual, IVisualEdge> container, IVisual value) {
            
        }

        public IVisual GetData(IDataObject data, IGraph<IVisual, IVisualEdge> container) {
            DataObject dataObject = data as DataObject;
            Image image = null;
            
            if (dataObject != null) {
                if (dataObject.ContainsImage()) {
                    image = dataObject.GetImage();
                } else {
                    var stream = dataObject.GetData(DataFormats[0]) as Stream;
                    var bmpStream = new BitmapFromDibStream(stream);
                    image = new Bitmap(bmpStream);
                }
            }

            if (image != null) {
                Content<Stream> content = new Content<Stream> ();
                content.Data = new MemoryStream();

                image.Save(content.Data, ImageFormat.Jpeg);
                content.ContentType = ContentTypes.JPG;
                content.Compression = CompressionType.neverCompress;

                image.Dispose();

                Uri uri = DragDropFacade.GetUri (data);
                if (uri != null) {
                    content.Source = uri.AbsoluteUri;
                    content.Description = uri.Segments[uri.Segments.Length-1];
                }
                return new VisualThingsContentViz ().VisualOfContent (container, content);

            } else {
                return null;
            }
        }


    }
}