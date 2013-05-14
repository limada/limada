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
using Limada.VisualThings;
using Limaki.Drawing.Imaging;
using Limaki.Graphs;
using Limaki.Model.Content;
using Limaki.Visuals;
using Xwt;

using Image = System.Drawing.Image;
using Bitmap = System.Drawing.Bitmap;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using Xwt.Gdi.Backend;

namespace Limaki.View.Ui.DragDrop {
    public class VisualsImageTransferDataHandler :
        ITransferDataHandler<IGraph<IVisual, IVisualEdge>, IVisual>, IVisualsTransferDataHandler {

        public virtual string[] Formats {
            get { return new string[] { System.Windows.Forms.DataFormats.Dib }; }
        }

        public Type HandledType {
            get { return typeof(IVisual); }
        }

        public void SetData (TransferDataSource data, IGraph<IVisual, IVisualEdge> container, IVisual value) {

        }

        public IVisual GetData (TransferDataSource data, IGraph<IVisual, IVisualEdge> container) {
            var dataObject = data as TransferDataSource;
            Image image = null;

            if (dataObject != null) {
                if (dataObject.ContainsImage()) {
                    image = dataObject.GetImage().ToGdi();
                } else {
                    var stream = dataObject.GetData(Formats[0]) as Stream;
                    var bmpStream = new BitmapFromDibStream(stream);
                    image = new Bitmap(bmpStream);
                }
            }

            if (image != null) {
                var content = new Content<Stream>();
                content.Data = new MemoryStream();

                image.Save(content.Data, ImageFormat.Jpeg);
                content.StreamType = ContentTypes.JPG;
                content.Compression = CompressionType.neverCompress;

                image.Dispose();

                var uri = DragDropFacade.GetUri(data);
                if (uri != null) {
                    content.Source = uri.AbsoluteUri;
                    content.Description = uri.Segments[uri.Segments.Length - 1];
                }
                return new VisualThingStreamHelper().CreateFromStream(container, content);

            } else {
                return null;
            }
        }


        }
}